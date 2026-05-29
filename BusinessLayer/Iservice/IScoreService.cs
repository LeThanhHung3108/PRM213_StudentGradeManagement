using System.Threading.Tasks;
using BusinessLayer.DTOs;

namespace BusinessLayer.IService;

public interface IScoreService
{
    Task<PagedResultDto<ScoreRecordDto>> GetPagedAsync(int pageNumber, int pageSize, int? subjectClassId, string? search, string? status);
    Task<ScoreDetailDto?> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateScoreDto dto);
    Task DeleteAsync(int id);
}