using System;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportController(IImportService importService)
    {
        _importService = importService;
    }

    /// <summary>
    /// Import file điểm Excel và lưu dữ liệu vào hệ thống
    /// </summary>
    /// <param name="file">File Excel cần import</param>
    /// <param name="subjectClassId">ID lớp môn học</param>
    /// <returns>Kết quả import dữ liệu</returns>
    // POST /api/import
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImportResultDto>> ImportAsync(
    [FromForm] ImportRequestDto request)
    {
        if (request.File is null)
            return BadRequest("File is required.");

        try
        {
            var result = await _importService.ImportAsync(
                request.File,
                request.SubjectClassId);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                ex.ToString());
        }
    }

    /// <summary>
    /// Xem trước thông tin file Excel trước khi import
    /// </summary>
    /// <param name="file">File Excel cần xem trước</param>
    /// <returns>Danh sách cột và số lượng dòng trong file</returns>
    // POST /api/import/preview
    [HttpPost("preview")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImportPreviewDto>> PreviewAsync(
    [FromForm] PreviewImportRequestDto request)
    {
        if (request.File is null)
            return BadRequest("File is required.");

        try
        {
            var preview = await _importService.PreviewAsync(request.File);

            return Ok(preview);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                ex.ToString());
        }
    }
}
