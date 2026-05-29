using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _service;

    public StatisticsController(IStatisticsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<StatisticsDto>> GetAsync([FromQuery] int? subjectClassId = null)
    {
        var dto = await _service.GetStatisticsAsync(subjectClassId);
        return Ok(dto);
    }

    [HttpGet("top-students")]
    public async Task<ActionResult<PagedResultDto<TopStudentDto>>> GetTopStudentsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? subjectClassId = null)
    {
        var result = await _service.GetTopStudentsAsync(pageNumber, pageSize, subjectClassId);
        return Ok(result);
    }

    [HttpGet("failed-students")]
    public async Task<ActionResult<PagedResultDto<TopStudentDto>>> GetFailedStudentsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? subjectClassId = null)
    {
        var result = await _service.GetFailedStudentsAsync(pageNumber, pageSize, subjectClassId);
        return Ok(result);
    }
}