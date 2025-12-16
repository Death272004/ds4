namespace MonitorAPI.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateRecentConnectionsReportAsync();
        Task<byte[]> GeneratePortUsageReportAsync();
        Task<byte[]> GenerateDeviceHistoryReportAsync();
        Task<byte[]> GeneratePortActivityReportAsync();
        Task<byte[]> GenerateConnectionDurationReportAsync();
        Task<byte[]> GenerateFullReportAsync();
    }
}
