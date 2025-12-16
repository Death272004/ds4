using Microsoft.AspNetCore.Mvc;
using MonitorAPI.Services;
using System.Text;

namespace MonitorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet("recent-connections")]
        public async Task<IActionResult> GetRecentConnections()
        {
            try
            {
                var bytes = await _reportService.GenerateRecentConnectionsReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"conexiones_recientes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de conexiones recientes");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }

        [HttpGet("port-usage")]
        public async Task<IActionResult> GetPortUsage()
        {
            try
            {
                var bytes = await _reportService.GeneratePortUsageReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"uso_puertos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de uso de puertos");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }

        [HttpGet("device-history")]
        public async Task<IActionResult> GetDeviceHistory()
        {
            try
            {
                var bytes = await _reportService.GenerateDeviceHistoryReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"historial_dispositivos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de historial de dispositivos");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }

        /// <summary>
        /// Descargar actividad por puerto
        /// </summary>
        [HttpGet("port-activity")]
        public async Task<IActionResult> GetPortActivity()
        {
            try
            {
                var bytes = await _reportService.GeneratePortActivityReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"actividad_puertos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de actividad de puertos");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }

        [HttpGet("connection-duration")]
        public async Task<IActionResult> GetConnectionDuration()
        {
            try
            {
                var bytes = await _reportService.GenerateConnectionDurationReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"duracion_conexiones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de duración de conexiones");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }


        [HttpGet("full-report")]
        public async Task<IActionResult> GetFullReport()
        {
            try
            {
                var bytes = await _reportService.GenerateFullReportAsync();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"reporte_completo_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte completo");
                return StatusCode(500, new { error = "Error generando reporte" });
            }
        }
    }
}
