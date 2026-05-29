using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using DataAccessLayer.Entity;
using DataAccessLayer.Enums;
using DataAccessLayer.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;

namespace BusinessLayer.Service;

public class ImportService : IImportService
{
    private readonly IImportRepository _repository;

    public ImportService(IImportRepository repository)
    {
        _repository = repository;
    }

    public async Task<ImportResultDto> ImportAsync(IFormFile file, int subjectClassId)
    {
        if (file == null) throw new ArgumentException("File is required.", nameof(file));
        if (file.Length == 0) throw new ArgumentException("File is empty.", nameof(file));

        var ext = Path.GetExtension(file.FileName);
        if (!string.Equals(ext, ".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only .xlsx files are supported.", nameof(file));

        var subjectClass = await _repository.GetSubjectClassByIdAsync(subjectClassId);
        if (subjectClass == null) throw new InvalidOperationException($"SubjectClass with id {subjectClassId} not found.");

        var result = new ImportResultDto();
        var entitiesToAdd = new List<ScoreRecord>();
        var errors = new List<string>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null) throw new ArgumentException("The Excel file does not contain any worksheets.");

            var dimension = worksheet.Dimension;
            if (dimension == null) throw new ArgumentException("The Excel worksheet is empty.");

            var colCount = dimension.Columns;
            var rowCount = dimension.Rows;
            result.TotalRows = Math.Max(0, rowCount - 1); 

            var headers = new List<string>(colCount);
            for (var c = 1; c <= colCount; c++)
            {
                var cell = worksheet.Cells[1, c].Text?.Trim() ?? string.Empty;
                headers.Add(cell);
            }

            var rollIndex = headers.FindIndex(h => string.Equals(h, "RollNumber", StringComparison.OrdinalIgnoreCase) || string.Equals(h, "Roll Number", StringComparison.OrdinalIgnoreCase));
            var nameIndex = headers.FindIndex(h => string.Equals(h, "FullName", StringComparison.OrdinalIgnoreCase) || string.Equals(h, "Full Name", StringComparison.OrdinalIgnoreCase));

            if (rollIndex < 0) throw new ArgumentException("Header 'RollNumber' is required.");
            if (nameIndex < 0) throw new ArgumentException("Header 'FullName' is required.");

            var dynamicIndices = Enumerable.Range(0, headers.Count)
                                           .Where(i => i != rollIndex && i != nameIndex)
                                           .ToList();

            for (var r = 2; r <= rowCount; r++)
            {
                try
                {
                    var rollCell = worksheet.Cells[r, rollIndex + 1].Text?.Trim();
                    var nameCell = worksheet.Cells[r, nameIndex + 1].Text?.Trim();

                    if (string.IsNullOrWhiteSpace(rollCell))
                    {
                        errors.Add($"Row {r}: RollNumber is empty. Skipped.");
                        result.FailedRows++;
                        continue;
                    }

                    var scoreRecord = new ScoreRecord
                    {
                        SubjectClassId = subjectClassId,
                        RollNumber = rollCell!,
                        FullName = string.IsNullOrWhiteSpace(nameCell) ? null : nameCell,
                        ImportedDate = DateTime.UtcNow.ToLocalTime()
                    };

                    var componentValues = new List<double>();
                    foreach (var idx in dynamicIndices)
                    {
                        var header = headers[idx];
                        var cellText = worksheet.Cells[r, idx + 1].Text?.Trim();

                        if (string.IsNullOrWhiteSpace(cellText))
                        {
                            continue;
                        }

                        if (!double.TryParse(cellText, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed) &&
                            !double.TryParse(cellText, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                        {
                            continue;
                        }

                        componentValues.Add(parsed);

                        scoreRecord.ScoreComponents.Add(new ScoreComponent
                        {
                            ComponentName = header,
                            ScoreValue = parsed,
                            Weight = 1
                        });
                    }

                    double finalGrade = componentValues.Count > 0 ? componentValues.Average() : 0;
                    if (componentValues.Count > 0)
                    {
                        finalGrade = componentValues.Average();
                    }

                    scoreRecord.FinalGrade = finalGrade;
                    scoreRecord.Status = finalGrade >= 5.0 ? ScoreStatus.Pass : ScoreStatus.Fail;

                    entitiesToAdd.Add(scoreRecord);
                    result.SuccessRows++;
                }
                catch (Exception exRow)
                {
                    errors.Add($"Row {r}: {exRow.Message}");
                    result.FailedRows++;
                }
            }
        }

        if (entitiesToAdd.Count > 0)
        {
            await using var tx = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddScoreRecordsAsync(entitiesToAdd);
                await _repository.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        result.ErrorMessages = errors;
        result.TotalRows = result.SuccessRows + result.FailedRows + Math.Max(0, result.TotalRows - (result.SuccessRows + result.FailedRows));
        return result;
    }

    public async Task<ImportPreviewDto> PreviewAsync(IFormFile file)
    {
        if (file == null) throw new ArgumentException("File is required.", nameof(file));
        if (file.Length == 0) throw new ArgumentException("File is empty.", nameof(file));

        var ext = Path.GetExtension(file.FileName);
        if (!string.Equals(ext, ".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only .xlsx files are supported.", nameof(file));

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        if (worksheet == null) throw new ArgumentException("The Excel file does not contain any worksheets.");

        var dimension = worksheet.Dimension;
        if (dimension == null) return new ImportPreviewDto { Headers = Array.Empty<string>(), TotalRows = 0, SampleRows = Array.Empty<IDictionary<string, string?>>() };

        var colCount = dimension.Columns;
        var rowCount = dimension.Rows;

        var headers = new List<string>(colCount);
        for (var c = 1; c <= colCount; c++)
        {
            headers.Add(worksheet.Cells[1, c].Text?.Trim() ?? string.Empty);
        }

        var rollIndex = headers.FindIndex(h => string.Equals(h, "RollNumber", StringComparison.OrdinalIgnoreCase) || string.Equals(h, "Roll Number", StringComparison.OrdinalIgnoreCase));
        var nameIndex = headers.FindIndex(h => string.Equals(h, "FullName", StringComparison.OrdinalIgnoreCase) || string.Equals(h, "Full Name", StringComparison.OrdinalIgnoreCase));

        if (rollIndex < 0 || nameIndex < 0)
        {
            throw new ArgumentException("Headers must include 'RollNumber' and 'FullName'.");
        }

        var sampleRows = new List<IDictionary<string, string?>>();
        var maxSamples = Math.Min(10, Math.Max(0, rowCount - 1));
        for (var r = 2; r <= 1 + maxSamples; r++)
        {
            var rowDict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            for (var c = 1; c <= colCount; c++)
            {
                rowDict[headers[c - 1]] = worksheet.Cells[r, c].Text?.Trim();
            }
            sampleRows.Add(rowDict);
        }

        return new ImportPreviewDto
        {
            Headers = headers,
            TotalRows = Math.Max(0, rowCount - 1),
            SampleRows = sampleRows
        };
    }
}