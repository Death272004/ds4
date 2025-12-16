using Microsoft.AspNetCore.Mvc;
using MonitorAPI.Models;
using MonitorAPI.Services;

namespace MonitorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeripheralsController : ControllerBase
{
    private readonly IPeripheralService _peripheralService;
    private readonly ILogger<PeripheralsController> _logger;

    public PeripheralsController(
        IPeripheralService peripheralService,
        ILogger<PeripheralsController> logger)
    {
        _peripheralService = peripheralService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los periféricos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<PeripheralDto>>> GetAll()
    {
        try
        {
            var peripherals = await _peripheralService.GetAllPeripheralsAsync();
            return Ok(peripherals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener periféricos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtener solo periféricos activos
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<List<PeripheralDto>>> GetActive()
    {
        try
        {
            var peripherals = await _peripheralService.GetActivePeripheralsAsync();
            return Ok(peripherals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener periféricos activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtener periférico por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PeripheralDto>> GetById(int id)
    {
        try
        {
            var peripheral = await _peripheralService.GetPeripheralByIdAsync(id);

            if (peripheral == null)
                return NotFound($"Periférico con ID {id} no encontrado");

            return Ok(peripheral);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener periférico {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtener actividad de un periférico
    /// </summary>
    [HttpGet("{id}/activity")]
    public async Task<ActionResult<List<ActivityDto>>> GetActivity(int id, [FromQuery] int limit = 50)
    {
        try
        {
            var activities = await _peripheralService.GetPeripheralActivityAsync(id, limit);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener actividad del periférico {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtener estadísticas del dashboard
    /// </summary>
    [HttpGet("dashboard/stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
    {
        try
        {
            var stats = await _peripheralService.GetDashboardStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas del dashboard");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
