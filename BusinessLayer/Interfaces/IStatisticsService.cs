using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsDto> GetOverviewAsync(string? className = null, string? subjectName = null, CancellationToken ct = default);
    Task<IReadOnlyList<ClassStatisticsDto>> GetByClassAsync(string? className = null, string? subjectName = null, CancellationToken ct = default);
    Task<IReadOnlyList<SubjectStatisticsDto>> GetBySubjectAsync(string? className = null, string? subjectName = null, CancellationToken ct = default);
    Task<PassFailStatisticsDto> GetPassFailAsync(string? className = null, string? subjectName = null, CancellationToken ct = default);
}
