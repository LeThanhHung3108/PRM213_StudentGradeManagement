using System;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/subjectclasses")]
public class SubjectClassController : ControllerBase
{
    private readonly ISubjectClassService _service;

    public SubjectClassController(ISubjectClassService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lấy danh sách lớp môn học có phân trang
    /// </summary>
    // GET /api/subjectclasses?pageNumber=1&pageSize=10&search=...
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<SubjectClassDto>>> GetPagedAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _service.GetPagedAsync(pageNumber, pageSize, search);
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin lớp môn học theo ID
    /// </summary>
    // GET /api/subjectclasses/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubjectClassDto>> GetByIdAsync(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// Tạo mới lớp môn học
    /// </summary>
    // POST /api/subjectclasses
    [HttpPost]
    public async Task<ActionResult<SubjectClassDto>> CreateAsync([FromBody] CreateSubjectClassDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await _service.CreateAsync(createDto);
            return Ok(created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Cập nhật thông tin lớp môn học
    /// </summary>
    // PUT /api/subjectclasses/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateSubjectClassDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _service.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
        
    /// <summary>
    /// Xóa lớp môn học theo ID
    /// </summary>
    // DELETE /api/subjectclasses/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}