using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

///
/// Controller for managing student scores.
///
[ApiController]
[Route("api/scores")]
public class ScoreController : ControllerBase
{
    private readonly IScoreService _service;

    public ScoreController(IScoreService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lấy danh sách điểm có phân trang, tìm kiếm và lọc.
    ///</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ScoreRecordDto>>> GetPagedAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? subjectClassId = null,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null)
    {
        var result = await _service.GetPagedAsync(pageNumber, pageSize, subjectClassId, search, status);
        return Ok(result);
    }

    /// <summary> 
    /// Lấy chi tiết điểm theo id. 
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScoreDetailDto>> GetByIdAsync(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    /// <summary> 
    /// Cập nhật điểm sinh viên. 
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateScoreDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _service.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary> 
    /// Xóa bản ghi điểm theo id. 
    /// </summary>
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