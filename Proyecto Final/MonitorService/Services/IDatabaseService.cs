using MonitorService.Models;

namespace MonitorService;

public interface IDatabaseService
{
    Task InitializeDatabaseAsync();
    Task<int> GetOrCreatePeripheralAsync(PeripheralInfo peripheral);
    Task RegisterActivityAsync(PeripheralEvent activity);
    Task RegisterPortUsageAsync(PortUsage usage);
    Task ClosePortUsageAsync(int portId);
    Task UpdatePhysicalPortStatusAsync(string portName, bool isConnected, string? connectedDevice);
}
