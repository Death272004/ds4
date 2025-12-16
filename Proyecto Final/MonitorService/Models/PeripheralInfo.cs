namespace MonitorService.Models;

public class PeripheralInfo
{
    public string DeviceID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
    public bool IsConnected { get; set; }
    public string RawData { get; set; } = string.Empty;
}

public class PeripheralEvent
{
    public int PeripheralId { get; set; }
    public string EventType { get; set; } = string.Empty; // Conectado, Desconectado, EnUso
    public string Status { get; set; } = string.Empty; // Online, Offline, Ocupado
    public string Details { get; set; } = string.Empty;
    public string RawData { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class PortUsage
{
    public int PortId { get; set; }
    public int? PeripheralId { get; set; }
    public string Status { get; set; } = string.Empty; // Ocupado, Libre
    public DateTime StartTime { get; set; }
}

public class PeripheralValidation
{
    public string DeviceID { get; set; } = string.Empty;
    public int ConsecutiveDetections { get; set; }
    public DateTime FirstDetection { get; set; }
    public DateTime LastDetection { get; set; }
    public bool IsValidated => ConsecutiveDetections >= 3;
}

public class PortPhysicalInfo
{
    public string PortName { get; set; } = string.Empty;
    public string PortType { get; set; } = string.Empty; // USB-A, USB-C, HDMI, Audio-Jack, SD-Card
    public bool IsConnected { get; set; }
    public string? ConnectedDevice { get; set; }
    public DateTime LastDetection { get; set; }
}
