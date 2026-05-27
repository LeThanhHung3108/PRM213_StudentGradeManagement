using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces;

public interface IFgFileService
{
    /// <summary>
    /// Generates an FG file from grade records and caches it, returning a download token.
    /// </summary>
    Task<FgGenerateResultDto> GenerateAsync(FgGenerateRequestDto request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the previously generated file bytes by token.
    /// Returns null if the token is unknown or expired.
    /// </summary>
    Task<FgFileDto?> GetByTokenAsync(string token, CancellationToken ct = default);
}
