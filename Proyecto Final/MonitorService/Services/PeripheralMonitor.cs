using System.Management;
using System.Collections.Concurrent;
using MonitorService.Models;
using Microsoft.Extensions.Logging;

namespace MonitorService;

public class PeripheralMonitor : IPeripheralMonitor
{
    private readonly ILogger<PeripheralMonitor> _logger;
    private readonly ConcurrentDictionary<string, PeripheralValidation> _validationCache;
    private readonly ConcurrentDictionary<string, PeripheralInfo> _peripheralCache;

    public PeripheralMonitor(ILogger<PeripheralMonitor> logger)
    {
        _logger = logger;
        _validationCache = new ConcurrentDictionary<string, PeripheralValidation>();
        _peripheralCache = new ConcurrentDictionary<string, PeripheralInfo>();
    }

    /// <summary>
    /// Obtiene el estado actual de todos los puertos físicos de la computadora
    /// </summary>
    public async Task<Dictionary<string, PortPhysicalInfo>> GetPhysicalPortsStatusAsync()
    {
        var portsStatus = new Dictionary<string, PortPhysicalInfo>();

        await Task.Run(() =>
        {
            // Detectar puertos USB
            DetectUSBPorts(portsStatus);

            // Detectar puerto HDMI
            DetectHDMIPort(portsStatus);

            // Detectar puerto de Audio
            DetectAudioPort(portsStatus);

            // Detectar lector SD
            DetectSDCardPort(portsStatus);

            // Detectar puerto de carga
            DetectPowerPort(portsStatus);
        });

        return portsStatus;
    }

    private void DetectUSBPorts(Dictionary<string, PortPhysicalInfo> portsStatus)
    {
        try
        {
            var usbPorts = new List<string> { "USB-A Puerto 1", "USB-A Puerto 2", "USB-C Puerto 1" };
            var connectedDevices = new HashSet<string>();

            // Detectar SOLO dispositivos USB EXTERNOS conectados a puertos físicos
            // Excluir: concentradores raíz, cámaras integradas, bluetooth interno, etc.
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");
            foreach (ManagementObject hub in searcher.Get())
            {
                try
                {
                    var deviceID = hub["DeviceID"]?.ToString() ?? "";
                    var description = hub["Description"]?.ToString() ?? "Dispositivo USB";
                    var status = hub["Status"]?.ToString() ?? "Unknown";

                    // FILTROS: Excluir dispositivos internos y controladores
                    bool isInternalDevice =
                        deviceID.Contains("ROOT_HUB", StringComparison.OrdinalIgnoreCase) ||
                        deviceID.Contains("VID_04F2&PID_B6F1", StringComparison.OrdinalIgnoreCase) || // Cámara HP integrada
                        description.Contains("Concentrador raíz", StringComparison.OrdinalIgnoreCase) ||
                        description.Contains("Root Hub", StringComparison.OrdinalIgnoreCase) ||
                        description.Contains("Composite", StringComparison.OrdinalIgnoreCase) && deviceID.Contains("VID_04F2"); // Dispositivo compuesto de cámara

                    if (status == "OK" && !string.IsNullOrEmpty(deviceID) && !isInternalDevice)
                    {
                        connectedDevices.Add(description);
                    }
                }
                catch { }
            }

            // Mapear dispositivos a puertos (simplificado)
            int portIndex = 0;
            foreach (var device in connectedDevices.Take(3))
            {
                if (portIndex < usbPorts.Count)
                {
                    portsStatus[usbPorts[portIndex]] = new PortPhysicalInfo
                    {
                        PortName = usbPorts[portIndex],
                        PortType = usbPorts[portIndex].Contains("USB-C") ? "USB-C" : "USB-A",
                        IsConnected = true,
                        ConnectedDevice = device,
                        LastDetection = DateTime.Now
                    };
                    portIndex++;
                }
            }

            // Marcar puertos vacíos
            for (int i = portIndex; i < usbPorts.Count; i++)
            {
                portsStatus[usbPorts[i]] = new PortPhysicalInfo
                {
                    PortName = usbPorts[i],
                    PortType = usbPorts[i].Contains("USB-C") ? "USB-C" : "USB-A",
                    IsConnected = false,
                    ConnectedDevice = null,
                    LastDetection = DateTime.Now
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detectando puertos USB");
        }
    }

    private void DetectHDMIPort(Dictionary<string, PortPhysicalInfo> portsStatus)
    {
        try
        {
            bool hdmiConnected = false;
            string connectedMonitor = null;

            // Solo detectar monitores ACTIVAMENTE conectados y encendidos (Availability = 3)
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_DesktopMonitor WHERE Availability = 3");

            var monitors = searcher.Get();
            if (monitors.Count > 0)
            {
                foreach (ManagementObject monitor in monitors)
                {
                    var name = monitor["Name"]?.ToString();
                    var status = monitor["Status"]?.ToString();
                    var availability = monitor["Availability"]?.ToString();

                    // Availability = 3 significa "Running/Full Power" (realmente conectado y en uso)
                    if (availability == "3" && status == "OK" && !string.IsNullOrEmpty(name))
                    {
                        hdmiConnected = true;
                        connectedMonitor = name;
                        break;
                    }
                }
            }

            portsStatus["Puerto HDMI"] = new PortPhysicalInfo
            {
                PortName = "Puerto HDMI",
                PortType = "HDMI",
                IsConnected = hdmiConnected,
                ConnectedDevice = connectedMonitor,
                LastDetection = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detectando puerto HDMI");
        }
    }

    private void DetectAudioPort(Dictionary<string, PortPhysicalInfo> portsStatus)
    {
        try
        {
            bool audioConnected = false;
            string connectedAudio = null;

            // Buscar solo dispositivos externos de audio, excluyendo internos y virtuales
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_SoundDevice WHERE Status = 'OK'");

            foreach (ManagementObject device in searcher.Get())
            {
                var name = device["Name"]?.ToString() ?? "";

                // Solo dispositivos EXTERNOS (no chips de audio internos)
                bool isInternalAudio =
                    name.Contains("Realtek", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("AMD", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Intel", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Steam", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Virtual", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("High Definition Audio", StringComparison.OrdinalIgnoreCase);

                bool isPotentialExternalDevice =
                    name.Contains("Headphone", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Headset", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Bluetooth", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("USB Audio", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Wireless", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Speaker", StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrEmpty(name) && !isInternalAudio && isPotentialExternalDevice)
                {
                    audioConnected = true;
                    connectedAudio = name;
                    break;
                }
            }

            portsStatus["Jack Audio"] = new PortPhysicalInfo
            {
                PortName = "Jack Audio",
                PortType = "Audio-Jack",
                IsConnected = audioConnected,
                ConnectedDevice = connectedAudio,
                LastDetection = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detectando puerto de audio");
        }
    }

    private void DetectSDCardPort(Dictionary<string, PortPhysicalInfo> portsStatus)
    {
        try
        {
            bool sdConnected = false;
            string sdCard = null;

            // Verificar unidades REMOVIBLES con MEDIA INSERTADA (Size > 0)
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 AND Size > 0");

            foreach (ManagementObject drive in searcher.Get())
            {
                var deviceId = drive["DeviceID"]?.ToString();
                var volumeName = drive["VolumeName"]?.ToString();
                var size = drive["Size"]?.ToString();

                // Solo si tiene tamaño significa que HAY una tarjeta insertada
                if (!string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(size))
                {
                    sdConnected = true;
                    sdCard = string.IsNullOrEmpty(volumeName)
                        ? $"Tarjeta SD ({deviceId})"
                        : $"{volumeName} ({deviceId})";
                    break;
                }
            }

            portsStatus["Lector SD"] = new PortPhysicalInfo
            {
                PortName = "Lector SD",
                PortType = "SD-Card",
                IsConnected = sdConnected,
                ConnectedDevice = sdCard,
                LastDetection = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detectando lector SD");
        }
    }

    public async Task<List<PeripheralInfo>> ScanPeripheralsAsync()
    {
        var peripherals = new List<PeripheralInfo>();

        try
        {
            // Escanear dispositivos USB
            await Task.Run(() =>
            {
                peripherals.AddRange(ScanUSBDevices());
                peripherals.AddRange(ScanPointingDevices());
                peripherals.AddRange(ScanKeyboards());
                peripherals.AddRange(ScanMonitors());
                peripherals.AddRange(ScanAudioDevices());
                peripherals.AddRange(ScanPrinters());
            });

            // Validar cada periférico detectado
            foreach (var peripheral in peripherals)
            {
                await ValidatePeripheralDetectionAsync(peripheral);
            }

            _logger.LogInformation($"Escaneados {peripherals.Count} periféricos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al escanear periféricos");
        }

        return peripherals;
    }

    public async Task<List<PeripheralInfo>> GetValidatedPeripheralsAsync()
    {
        await Task.CompletedTask;
        return _peripheralCache.Values
            .Where(p => _validationCache.TryGetValue(p.DeviceID, out var validation) && validation.IsValidated)
            .ToList();
    }

    public async Task ValidatePeripheralAsync(string deviceId)
    {
        await Task.CompletedTask;
        if (_validationCache.TryGetValue(deviceId, out var validation))
        {
            validation.ConsecutiveDetections = 3; // Forzar validación
            validation.LastDetection = DateTime.Now;
        }
    }

    public void ResetValidation(string deviceId)
    {
        _validationCache.TryRemove(deviceId, out _);
        _peripheralCache.TryRemove(deviceId, out _);
    }

    private async Task ValidatePeripheralDetectionAsync(PeripheralInfo peripheral)
    {
        await Task.CompletedTask;

        var validation = _validationCache.GetOrAdd(peripheral.DeviceID, _ => new PeripheralValidation
        {
            DeviceID = peripheral.DeviceID,
            FirstDetection = DateTime.Now,
            ConsecutiveDetections = 0
        });

        validation.ConsecutiveDetections++;
        validation.LastDetection = DateTime.Now;

        // Solo agregar al cache si está validado (3+ detecciones consecutivas)
        if (validation.IsValidated)
        {
            _peripheralCache.AddOrUpdate(peripheral.DeviceID, peripheral, (_, _) => peripheral);
        }
    }

    private List<PeripheralInfo> ScanUSBDevices()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                try
                {
                    var dependent = queryObj["Dependent"].ToString();
                    var deviceId = dependent?.Split('=')[1].Replace("\"", "").Trim() ?? "";

                    using var device = new ManagementObject(dependent ?? "");
                    var name = device["Name"]?.ToString() ?? "Dispositivo USB";
                    var status = device["Status"]?.ToString() ?? "Unknown";
                    var manufacturer = device["Manufacturer"]?.ToString() ?? "Desconocido";

                    // Filtrar dispositivos internos y controladores del sistema
                    bool isInternalDevice =
                        name.Contains("Root Hub", StringComparison.OrdinalIgnoreCase) ||
                        name.Contains("Host Controller", StringComparison.OrdinalIgnoreCase) ||
                        name.Contains("Composite Device", StringComparison.OrdinalIgnoreCase) ||
                        name.Contains("Generic USB Hub", StringComparison.OrdinalIgnoreCase) ||
                        deviceId.Contains("ROOT_HUB", StringComparison.OrdinalIgnoreCase);

                    if (isInternalDevice) continue;

                    devices.Add(new PeripheralInfo
                    {
                        DeviceID = deviceId,
                        Name = name,
                        Type = "USB",
                        Manufacturer = manufacturer,
                        Status = status == "OK" ? "Online" : "Error",
                        DetectedAt = DateTime.Now,
                        IsConnected = status == "OK",
                        Port = "USB",
                        RawData = $"Status: {status}"
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error procesando dispositivo USB");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando dispositivos USB");
        }

        return devices;
    }

    private List<PeripheralInfo> ScanPointingDevices()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PointingDevice");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var deviceId = queryObj["DeviceID"]?.ToString() ?? Guid.NewGuid().ToString();
                var name = queryObj["Name"]?.ToString() ?? "Mouse";
                var manufacturer = queryObj["Manufacturer"]?.ToString() ?? "Desconocido";
                var status = queryObj["Status"]?.ToString() ?? "Unknown";

                // Filtrar touchpads y dispositivos integrados
                bool isInternal =
                    name.Contains("TouchPad", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Touch Pad", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Synaptics", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("HID-compliant mouse", StringComparison.OrdinalIgnoreCase) && manufacturer == "Desconocido";

                if (isInternal) continue;

                devices.Add(new PeripheralInfo
                {
                    DeviceID = deviceId,
                    Name = name,
                    Type = "Mouse",
                    Manufacturer = manufacturer,
                    Status = status == "OK" ? "Online" : "Error",
                    DetectedAt = DateTime.Now,
                    IsConnected = status == "OK",
                    Port = "USB",
                    RawData = $"Status: {status}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando dispositivos de entrada");
        }

        return devices;
    }

    private List<PeripheralInfo> ScanKeyboards()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Keyboard");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var deviceId = queryObj["DeviceID"]?.ToString() ?? Guid.NewGuid().ToString();
                var name = queryObj["Name"]?.ToString() ?? "Teclado";
                var status = queryObj["Status"]?.ToString() ?? "Unknown";

                // Filtrar teclado integrado de laptop (solo incluir teclados externos)
                bool isBuiltInKeyboard =
                    name.Contains("Standard", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("HID Keyboard", StringComparison.OrdinalIgnoreCase) && deviceId.Contains("&MI_");

                if (isBuiltInKeyboard) continue;

                devices.Add(new PeripheralInfo
                {
                    DeviceID = deviceId,
                    Name = name,
                    Type = "Teclado",
                    Manufacturer = "Desconocido",
                    Status = status == "OK" ? "Online" : "Error",
                    DetectedAt = DateTime.Now,
                    IsConnected = status == "OK",
                    Port = "USB",
                    RawData = $"Status: {status}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando teclados");
        }

        return devices;
    }

    private List<PeripheralInfo> ScanMonitors()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var deviceId = queryObj["DeviceID"]?.ToString() ?? Guid.NewGuid().ToString();
                var name = queryObj["Name"]?.ToString() ?? "Monitor";
                var status = queryObj["Status"]?.ToString() ?? "Unknown";
                var availability = queryObj["Availability"]?.ToString();

                // Solo monitores externos activos (no pantalla integrada de laptop)
                // Availability = 3 significa "Running/Full Power" (monitor externo en uso)
                if (availability != "3") continue;

                devices.Add(new PeripheralInfo
                {
                    DeviceID = deviceId,
                    Name = name,
                    Type = "Monitor",
                    Manufacturer = "Desconocido",
                    Status = status == "OK" ? "Online" : "Error",
                    DetectedAt = DateTime.Now,
                    IsConnected = status == "OK",
                    Port = "HDMI/DP",
                    RawData = $"Status: {status}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando monitores");
        }

        return devices;
    }

    private List<PeripheralInfo> ScanAudioDevices()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var deviceId = queryObj["DeviceID"]?.ToString() ?? Guid.NewGuid().ToString();
                var name = queryObj["Name"]?.ToString() ?? "Dispositivo de Audio";
                var manufacturer = queryObj["Manufacturer"]?.ToString() ?? "Desconocido";
                var status = queryObj["Status"]?.ToString() ?? "Unknown";

                // Filtrar chips de audio internos
                bool isInternalAudio =
                    name.Contains("Realtek", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("AMD", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Intel", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("High Definition Audio", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Virtual", StringComparison.OrdinalIgnoreCase);

                if (isInternalAudio) continue;

                devices.Add(new PeripheralInfo
                {
                    DeviceID = deviceId,
                    Name = name,
                    Type = "Auriculares",
                    Manufacturer = manufacturer,
                    Status = status == "OK" ? "Online" : "Error",
                    DetectedAt = DateTime.Now,
                    IsConnected = status == "OK",
                    Port = "Audio Jack / USB",
                    RawData = $"Status: {status}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando dispositivos de audio");
        }

        return devices;
    }

    private List<PeripheralInfo> ScanPrinters()
    {
        var devices = new List<PeripheralInfo>();

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var deviceId = queryObj["DeviceID"]?.ToString() ?? Guid.NewGuid().ToString();
                var name = queryObj["Name"]?.ToString() ?? "Impresora";
                var portName = queryObj["PortName"]?.ToString() ?? "Unknown";
                var status = queryObj["PrinterStatus"]?.ToString() ?? "Unknown";

                devices.Add(new PeripheralInfo
                {
                    DeviceID = deviceId,
                    Name = name,
                    Type = "Impresora",
                    Manufacturer = "Desconocido",
                    Status = status == "3" ? "Online" : "Offline",
                    DetectedAt = DateTime.Now,
                    IsConnected = status == "3",
                    Port = portName,
                    RawData = $"PrinterStatus: {status}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escaneando impresoras");
        }

        return devices;
    }

    /// <summary>
    /// Detecta si el cargador está conectado al puerto de alimentación
    /// </summary>
    private void DetectPowerPort(Dictionary<string, PortPhysicalInfo> portsStatus)
    {
        try
        {
            bool isCharging = false;
            string powerStatus = "Desconectado";

            // Consultar el estado de la batería para determinar si el cargador está conectado
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
            foreach (ManagementObject battery in searcher.Get())
            {
                try
                {
                    // BatteryStatus: 1=Discharging, 2=AC (Charging/Plugged), 3=Fully Charged, 4=Low, 5=Critical
                    var batteryStatus = battery["BatteryStatus"]?.ToString();
                    var estimatedChargeRemaining = battery["EstimatedChargeRemaining"]?.ToString() ?? "0";

                    // Si BatteryStatus es 2 (AC/Charging) o 3 (Fully Charged + plugged), el cargador está conectado
                    if (batteryStatus == "2" || batteryStatus == "3")
                    {
                        isCharging = true;
                        powerStatus = batteryStatus == "2" ? $"Cargando ({estimatedChargeRemaining}%)" : $"Carga completa ({estimatedChargeRemaining}%)";
                    }
                    else if (batteryStatus == "1")
                    {
                        powerStatus = $"Descargando ({estimatedChargeRemaining}%)";
                    }

                    _logger.LogDebug($"Puerto de carga - BatteryStatus: {batteryStatus}, Carga: {estimatedChargeRemaining}%");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error leyendo estado de batería individual");
                }
            }

            portsStatus["Puerto Carga"] = new PortPhysicalInfo
            {
                PortName = "Puerto Carga",
                IsConnected = isCharging,
                ConnectedDevice = isCharging ? powerStatus : null,
                LastDetection = DateTime.Now
            };

            _logger.LogInformation($"Puerto Carga: {(isCharging ? "CONECTADO" : "LIBRE")} - {powerStatus}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detectando puerto de carga");
            portsStatus["Puerto Carga"] = new PortPhysicalInfo
            {
                PortName = "Puerto Carga",
                IsConnected = false,
                ConnectedDevice = null,
                LastDetection = DateTime.Now
            };
        }
    }
}
