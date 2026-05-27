using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(CancellationToken ct = default);
}
