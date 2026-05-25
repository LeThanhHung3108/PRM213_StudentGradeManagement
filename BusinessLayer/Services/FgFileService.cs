using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.IRepository;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services;

public class FgFileService : IFgFileService
{
    private readonly IUnitOfWork _uow;
    private readonly IEnumerable<IFgFileFormatter> _formatters;
    private readonly IMemoryCache _cache;

    // Tokens expire after 15 minutes — long enough for a user to click download
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(15);
    private const string CacheKeyPrefix = "fg_token_";

    public FgFileService(
        IUnitOfWork uow,
        IEnumerable<IFgFileFormatter> formatters,
        IMemoryCache cache)
    {
        _uow = uow;
        _formatters = formatters;
        _cache = cache;
    }

    public async Task<FgGenerateResultDto> GenerateAsync(
        FgGenerateRequestDto request,
        CancellationToken ct = default)
    {
        // Resolve formatter
        var formatter = _formatters.FirstOrDefault(f => f.FileFormat == request.Format)
            ?? throw new InvalidOperationException($"No formatter registered for format '{request.Format}'.");

        // Fetch records
        var records = await _uow.GradeRecords.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(request.Semester))
        {
            records = records
                .Where(r => string.Equals(r.ImportedAt.ToString("yyyy-MM"), request.Semester, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Format
        var content = formatter.FormatRecords(records);

        // Build file name
        var semesterPart = string.IsNullOrWhiteSpace(request.Semester) ? "all" : request.Semester;
        var fileName = $"FG_{semesterPart}_{DateTime.UtcNow:yyyyMMdd_HHmmss}{formatter.FileExtension}";

        var fileDto = new FgFileDto
        {
            FileName    = fileName,
            ContentType = formatter.ContentType,
            Content     = content
        };

        // Cache under a new token
        var token = Guid.NewGuid().ToString("N");
        _cache.Set(CacheKeyPrefix + token, fileDto, TokenLifetime);

        return new FgGenerateResultDto
        {
            DownloadToken = token,
            FileName      = fileName,
            ContentType   = formatter.ContentType,
            RecordCount   = records.Count,
            GeneratedAt   = DateTime.UtcNow
        };
    }

    public Task<FgFileDto?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        _cache.TryGetValue<FgFileDto>(CacheKeyPrefix + token, out var file);
        return Task.FromResult(file);
    }
}
