namespace MonitorAPI.Models;

public class PeripheralDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string Puerto { get; set; } = string.Empty;
    public string TipoPerifico { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime? UltimaActividad { get; set; }
    public string EstadoActual { get; set; } = string.Empty;
    public string UltimoEvento { get; set; } = string.Empty;
}

public class ActivityDto
{
    public int Id { get; set; }
    public string TipoEvento { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string Detalles { get; set; } = string.Empty;
}

public class PortUsageDto
{
    public int Id { get; set; }
    public string NombrePuerto { get; set; } = string.Empty;
    public string TipoPuerto { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public string? Periferico { get; set; }
    public string? TipoPeriferico { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? Duracion { get; set; }
    public bool EnUso { get; set; }
}

public class PortStatusRequest
{
    public int PortId { get; set; }
}

public class PortStatusResponse
{
    public bool IsValid { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }
    public PeripheralDto? ConnectedDevice { get; set; }
}

public class DashboardStatsDto
{
    public int TotalPerifericos { get; set; }
    public int PerifericosActivos { get; set; }
    public int PerifericosInactivos { get; set; }
    public int PuertosEnUso { get; set; }
    public List<PeripheralDto> PerifericosRecientes { get; set; } = new();
}

public class PhysicalPortDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoPuerto { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public string? Descripcion { get; set; }
    public bool EstaConectado { get; set; }
    public string? DispositivoConectado { get; set; }
    public DateTime? UltimaDeteccion { get; set; }
}
