using System.Threading.Tasks;
using BusinessLayer.DTOs;

namespace BusinessLayer.IService;

public interface IStatisticsService
{
    Task<StatisticsDto> GetStatisticsAsync(int? subjectClassId);
    Task<PagedResultDto<TopStudentDto>> GetTopStudentsAsync(int pageNumber, int pageSize, int? subjectClassId);
    Task<PagedResultDto<TopStudentDto>> GetFailedStudentsAsync(int pageNumber, int pageSize, int? subjectClassId);
}