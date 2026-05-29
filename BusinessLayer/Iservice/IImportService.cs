using System.Threading.Tasks;
using BusinessLayer.DTOs;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.IService;

public interface IImportService
{
    Task<ImportResultDto> ImportAsync(IFormFile file, int subjectClassId);
    Task<ImportPreviewDto> PreviewAsync(IFormFile file);
}