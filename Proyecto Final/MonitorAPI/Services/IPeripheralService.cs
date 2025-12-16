using MonitorAPI.Models;

namespace MonitorAPI.Services;

public interface IPeripheralService
{
    Task<List<PeripheralDto>> GetAllPeripheralsAsync();
    Task<List<PeripheralDto>> GetActivePeripheralsAsync();
    Task<PeripheralDto?> GetPeripheralByIdAsync(int id);
    Task<List<ActivityDto>> GetPeripheralActivityAsync(int peripheralId, int limit = 50);
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
