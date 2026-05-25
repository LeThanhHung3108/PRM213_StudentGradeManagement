using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace StudentGradeManagement.Controllers;

[ApiController]
[Route("api/grades")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GradeDto>>> GetAll(CancellationToken ct)
    {
        return Ok(await _gradeService.GetAllAsync(ct));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GradeDto>> GetById(int id, CancellationToken ct)
    {
        var grade = await _gradeService.GetByIdAsync(id, ct);
        return grade is null ? NotFound() : Ok(grade);
    }

    [HttpGet("by-class/{className}")]
    public async Task<ActionResult<IReadOnlyList<GradeDto>>> GetByClass(string className, CancellationToken ct)
    {
        return Ok(await _gradeService.GetByClassNameAsync(className, ct));
    }

    [HttpGet("by-subject/{subjectName}")]
    public async Task<ActionResult<IReadOnlyList<GradeDto>>> GetBySubject(string subjectName, CancellationToken ct)
    {
        return Ok(await _gradeService.GetBySubjectNameAsync(subjectName, ct));
    }
}