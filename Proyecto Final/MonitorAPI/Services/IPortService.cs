using MonitorAPI.Models;

namespace MonitorAPI.Services;

public interface IPortService
{
    Task<List<PortUsageDto>> GetRecentPortUsageAsync(int limit = 50);
    Task<PortStatusResponse> VerifyPortStatusAsync(int portId);
    Task<List<PhysicalPortDto>> GetAllPhysicalPortsAsync();
}
