using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces;

public interface IGradeService
{
    Task<IReadOnlyList<GradeDto>> GetAllAsync(CancellationToken ct = default);
    Task<GradeDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<GradeDto>> GetByClassNameAsync(string className, CancellationToken ct = default);
    Task<IReadOnlyList<GradeDto>> GetBySubjectNameAsync(string subjectName, CancellationToken ct = default);
}