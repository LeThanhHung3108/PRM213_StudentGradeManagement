using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace StudentGradeManagement.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> Overview(CancellationToken ct)
    {
        var result = await _statisticsService.GetOverviewAsync(ct: ct);
        return Ok(result);
    }

    [HttpGet("by-class")]
    public async Task<IActionResult> ByClass([FromQuery] string className, CancellationToken ct)
    {
        var result = await _statisticsService.GetByClassAsync(className: className, ct: ct);
        return Ok(result);
    }

    [HttpGet("by-subject")]
    public async Task<IActionResult> BySubject([FromQuery] string subjectName, CancellationToken ct)
    {
        var result = await _statisticsService.GetBySubjectAsync(subjectName: subjectName, ct: ct);
        return Ok(result);
    }

    [HttpGet("pass-fail")]
    public async Task<IActionResult> PassFail(CancellationToken ct)
    {
        var result = await _statisticsService.GetPassFailAsync(ct: ct);
        return Ok(result);
    }
}