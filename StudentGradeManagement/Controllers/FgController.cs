using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace StudentGradeManagement.Controllers;

[ApiController]
[Route("api/fg")]
public class FgController : ControllerBase
{
    private readonly IFgFileService _fgFileService;

    public FgController(IFgFileService fgFileService)
    {
        _fgFileService = fgFileService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] FgGenerateRequestDto request, CancellationToken ct)
    {
        var result = await _fgFileService.GenerateAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("download/{token}")]
    public async Task<IActionResult> Download(string token, CancellationToken ct)
    {
        var file = await _fgFileService.GetByTokenAsync(token, ct);
        if (file is null)
        {
            return NotFound();
        }

        return File(file.Content, file.ContentType, file.FileName);
    }
}