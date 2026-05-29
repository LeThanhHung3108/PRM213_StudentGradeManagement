using System.Threading.Tasks;
using BusinessLayer.DTOs;

namespace BusinessLayer.IService;

public interface ISubjectClassService
{
    Task<PagedResultDto<SubjectClassDto>> GetPagedAsync(int pageNumber, int pageSize, string? search);
    Task<SubjectClassDto?> GetByIdAsync(int id);
    Task<SubjectClassDto> CreateAsync(CreateSubjectClassDto dto);
    Task UpdateAsync(int id, UpdateSubjectClassDto dto);
    Task DeleteAsync(int id);
}