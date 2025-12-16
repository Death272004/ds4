using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonitorService.Models;

namespace MonitorService;

public class MonitorWorker : BackgroundService
{
    private readonly ILogger<MonitorWorker> _logger;
    private readonly IPeripheralMonitor _peripheralMonitor;
    private readonly IDatabaseService _databaseService;
    private readonly Dictionary<string, PeripheralInfo> _previousState;
    private readonly Dictionary<string, PortPhysicalInfo> _previousPortsState;

    public MonitorWorker(
        ILogger<MonitorWorker> logger,
        IPeripheralMonitor peripheralMonitor,
        IDatabaseService databaseService)
    {
        _logger = logger;
        _peripheralMonitor = peripheralMonitor;
        _databaseService = databaseService;
        _previousState = new Dictionary<string, PeripheralInfo>();
        _previousPortsState = new Dictionary<string, PortPhysicalInfo>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Monitor Worker iniciado");

        // Inicializar conexión a BD
        await _databaseService.InitializeDatabaseAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Escanear estado de PUERTOS FÍSICOS
                var currentPorts = await _peripheralMonitor.GetPhysicalPortsStatusAsync();

                // Procesar cambios en los puertos
                await ProcessPortChangesAsync(currentPorts);

                // También escanear periféricos
                var currentPeripherals = await _peripheralMonitor.ScanPeripheralsAsync();
                var validatedPeripherals = await _peripheralMonitor.GetValidatedPeripheralsAsync();
                await ProcessPeripheralChangesAsync(validatedPeripherals);

                // Esperar 5 segundos antes del próximo escaneo
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el ciclo de monitoreo");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        _logger.LogInformation("Monitor Worker detenido");
    }

    private async Task ProcessPeripheralChangesAsync(List<PeripheralInfo> currentPeripherals)
    {
        var currentDeviceIds = currentPeripherals.Select(p => p.DeviceID).ToHashSet();
        var previousDeviceIds = _previousState.Keys.ToHashSet();

        // Detectar nuevos periféricos conectados
        foreach (var peripheral in currentPeripherals)
        {
            if (!_previousState.ContainsKey(peripheral.DeviceID))
            {
                await HandlePeripheralConnectedAsync(peripheral);
            }
            else
            {
                await HandlePeripheralActiveAsync(peripheral);
            }
        }

        // Detectar periféricos desconectados
        foreach (var deviceId in previousDeviceIds)
        {
            if (!currentDeviceIds.Contains(deviceId))
            {
                await HandlePeripheralDisconnectedAsync(_previousState[deviceId]);
            }
        }

        // Actualizar estado anterior
        _previousState.Clear();
        foreach (var peripheral in currentPeripherals)
        {
            _previousState[peripheral.DeviceID] = peripheral;
        }
    }

    private async Task HandlePeripheralConnectedAsync(PeripheralInfo peripheral)
    {
        _logger.LogInformation($"Periférico conectado: {peripheral.Name} ({peripheral.Type})");

        var peripheralId = await _databaseService.GetOrCreatePeripheralAsync(peripheral);

        await _databaseService.RegisterActivityAsync(new PeripheralEvent
        {
            PeripheralId = peripheralId,
            EventType = "Conectado",
            Status = "Online",
            Details = $"Conectado en puerto {peripheral.Port}",
            RawData = peripheral.RawData,
            Timestamp = DateTime.Now
        });

        await _databaseService.RegisterPortUsageAsync(new PortUsage
        {
            PortId = peripheralId, // Simplificado - usar ID del periférico como referencia
            PeripheralId = peripheralId,
            Status = "Ocupado",
            StartTime = DateTime.Now
        });
    }

    private async Task HandlePeripheralActiveAsync(PeripheralInfo peripheral)
    {
        var peripheralId = await _databaseService.GetOrCreatePeripheralAsync(peripheral);

        // Solo registrar si el estado cambió
        var previous = _previousState[peripheral.DeviceID];
        if (previous.Status != peripheral.Status)
        {
            await _databaseService.RegisterActivityAsync(new PeripheralEvent
            {
                PeripheralId = peripheralId,
                EventType = "EnUso",
                Status = peripheral.Status,
                Details = $"Estado actualizado: {peripheral.Status}",
                RawData = peripheral.RawData,
                Timestamp = DateTime.Now
            });
        }
    }

    private async Task HandlePeripheralDisconnectedAsync(PeripheralInfo peripheral)
    {
        _logger.LogWarning($"Periférico desconectado: {peripheral.Name} ({peripheral.Type})");

        var peripheralId = await _databaseService.GetOrCreatePeripheralAsync(peripheral);

        await _databaseService.RegisterActivityAsync(new PeripheralEvent
        {
            PeripheralId = peripheralId,
            EventType = "Desconectado",
            Status = "Offline",
            Details = "Dispositivo desconectado del sistema",
            RawData = peripheral.RawData,
            Timestamp = DateTime.Now
        });

        await _databaseService.ClosePortUsageAsync(peripheralId);
    }

    private async Task ProcessPortChangesAsync(Dictionary<string, PortPhysicalInfo> currentPorts)
    {
        foreach (var port in currentPorts)
        {
            var portName = port.Key;
            var portInfo = port.Value;

            // Verificar si el estado del puerto cambió
            if (_previousPortsState.TryGetValue(portName, out var previousPort))
            {
                // Si cambió de desconectado a conectado
                if (!previousPort.IsConnected && portInfo.IsConnected)
                {
                    _logger.LogInformation($"✅ {portName}: Dispositivo CONECTADO - {portInfo.ConnectedDevice}");
                    await _databaseService.UpdatePhysicalPortStatusAsync(portName, true, portInfo.ConnectedDevice);
                }
                // Si cambió de conectado a desconectado
                else if (previousPort.IsConnected && !portInfo.IsConnected)
                {
                    _logger.LogWarning($"❌ {portName}: Dispositivo DESCONECTADO");
                    await _databaseService.UpdatePhysicalPortStatusAsync(portName, false, null);
                }
            }
            else
            {
                // Primera vez que vemos este puerto
                _logger.LogInformation($"🔍 {portName}: Estado inicial - {(portInfo.IsConnected ? "CONECTADO" : "LIBRE")}");
                await _databaseService.UpdatePhysicalPortStatusAsync(portName, portInfo.IsConnected, portInfo.ConnectedDevice);
            }

            // Actualizar estado anterior
            _previousPortsState[portName] = portInfo;
        }
    }
}
