using Microsoft.AspNetCore.Mvc;
using MonitorAPI.Models;
using MonitorAPI.Services;

namespace MonitorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortsController : ControllerBase
{
    private readonly IPortService _portService;
    private readonly ILogger<PortsController> _logger;

    public PortsController(
        IPortService portService,
        ILogger<PortsController> logger)
    {
        _portService = portService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener puertos usados recientemente
    /// </summary>
    [HttpGet("recent")]
    public async Task<ActionResult<List<PortUsageDto>>> GetRecentUsage([FromQuery] int limit = 50)
    {
        try
        {
            var usage = await _portService.GetRecentPortUsageAsync(limit);
            return Ok(usage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener uso reciente de puertos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verificar estado de un puerto (sin lecturas residuales)
    /// </summary>
    [HttpPost("{id}/verify")]
    public async Task<ActionResult<PortStatusResponse>> VerifyStatus(int id)
    {
        try
        {
            var status = await _portService.VerifyPortStatusAsync(id);

            if (!status.IsValid)
                return NotFound(status);

            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al verificar estado del puerto {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtener todos los puertos físicos de la computadora con su estado actual
    /// </summary>
    [HttpGet("physical")]
    public async Task<ActionResult<List<PhysicalPortDto>>> GetPhysicalPorts()
    {
        try
        {
            var ports = await _portService.GetAllPhysicalPortsAsync();
            return Ok(ports);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puertos físicos");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}