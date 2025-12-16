using MonitorService.Models;

namespace MonitorService;

public interface IPeripheralMonitor
{
    Task<List<PeripheralInfo>> ScanPeripheralsAsync();
    Task<List<PeripheralInfo>> GetValidatedPeripheralsAsync();
    Task ValidatePeripheralAsync(string deviceId);
    void ResetValidation(string deviceId);
    Task<Dictionary<string, PortPhysicalInfo>> GetPhysicalPortsStatusAsync();
}
