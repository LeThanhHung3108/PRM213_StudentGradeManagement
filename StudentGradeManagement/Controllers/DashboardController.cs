using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace StudentGradeManagement.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _dashboardService.GetDashboardAsync(ct);
        return Ok(result);
    }
}