using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Services;

public class GradeService : IGradeService
{
    private readonly IUnitOfWork _uow;

    public GradeService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IReadOnlyList<GradeDto>> GetAllAsync(CancellationToken ct = default)
    {
        var records = await _uow.GradeRecords.GetAllAsync();
        return records.Select(Map).ToList();
    }

    public async Task<GradeDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var record = await _uow.GradeRecords.GetByIdAsync(id);
        return record is null ? null : Map(record);
    }

    public async Task<IReadOnlyList<GradeDto>> GetByClassNameAsync(string className, CancellationToken ct = default)
    {
        var records = await _uow.GradeRecords.GetAllAsync();
        return records
            .Where(x => string.Equals(x.ClassName, className, StringComparison.OrdinalIgnoreCase))
            .Select(Map)
            .ToList();
    }

    public async Task<IReadOnlyList<GradeDto>> GetBySubjectNameAsync(string subjectName, CancellationToken ct = default)
    {
        var records = await _uow.GradeRecords.GetAllAsync();
        return records
            .Where(x => string.Equals(x.SubjectName, subjectName, StringComparison.OrdinalIgnoreCase))
            .Select(Map)
            .ToList();
    }

    private static GradeDto Map(ImportedGradeRecord record) => new()
    {
        Id = record.Id,
        StudentCode = record.StudentCode,
        StudentName = record.StudentName,
        ClassName = record.ClassName,
        SubjectName = record.SubjectName,
        ProcessScore = record.ProcessScore,
        MidtermScore = record.MidtermScore,
        FinalScore = record.FinalScore,
        TotalScore = record.TotalScore,
        Result = record.Result,
        ImportedAt = record.ImportedAt,
        ImportHistoryId = record.ImportHistoryId
    };
}