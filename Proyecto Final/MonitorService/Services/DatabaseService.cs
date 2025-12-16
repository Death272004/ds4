using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonitorService.Models;
using Dapper;

namespace MonitorService;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            _logger.LogInformation("Conexión a base de datos establecida correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al conectar con la base de datos");
            throw;
        }
    }

    public async Task<int> GetOrCreatePeripheralAsync(PeripheralInfo peripheral)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // Buscar periférico existente
            var existingId = await connection.QueryFirstOrDefaultAsync<int?>(
                @"SELECT ID FROM Perifericos WHERE DeviceID = @DeviceID",
                new { peripheral.DeviceID });

            if (existingId.HasValue)
            {
                // Actualizar última actividad
                await connection.ExecuteAsync(
                    @"UPDATE Perifericos 
                      SET UltimaActividad = @Now, 
                          Activo = @IsActive 
                      WHERE ID = @Id",
                    new { Now = DateTime.Now, IsActive = peripheral.IsConnected, Id = existingId.Value });

                return existingId.Value;
            }

            // Obtener TipoID basado en el tipo
            var tipoId = await connection.QueryFirstOrDefaultAsync<int?>(
                @"SELECT ID FROM TiposPerifericos WHERE Nombre = @Type",
                new { Type = peripheral.Type });

            if (!tipoId.HasValue)
            {
                // Si no existe el tipo, usar genérico USB
                tipoId = await connection.QueryFirstAsync<int>(
                    @"SELECT ID FROM TiposPerifericos WHERE Nombre = 'USB'");
            }

            // Crear nuevo periférico
            var newId = await connection.QuerySingleAsync<int>(
                @"INSERT INTO Perifericos (TipoID, Nombre, DeviceID, Fabricante, Puerto, UltimaActividad, Activo)
                  OUTPUT INSERTED.ID
                  VALUES (@TipoID, @Name, @DeviceID, @Manufacturer, @Port, @LastActivity, @Active)",
                new
                {
                    TipoID = tipoId.Value,
                    peripheral.Name,
                    peripheral.DeviceID,
                    peripheral.Manufacturer,
                    peripheral.Port,
                    LastActivity = DateTime.Now,
                    Active = peripheral.IsConnected
                });

            _logger.LogInformation($"Nuevo periférico registrado: {peripheral.Name} (ID: {newId})");
            return newId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al registrar periférico: {peripheral.Name}");
            throw;
        }
    }

    public async Task RegisterActivityAsync(PeripheralEvent activity)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                @"sp_RegistrarActividadValidada",
                new
                {
                    PerifericoID = activity.PeripheralId,
                    TipoEvento = activity.EventType,
                    Estado = activity.Status,
                    Detalles = activity.Details,
                    DatosRaw = activity.RawData
                },
                commandType: System.Data.CommandType.StoredProcedure);

            _logger.LogDebug($"Actividad registrada para periférico {activity.PeripheralId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al registrar actividad del periférico {activity.PeripheralId}");
        }
    }

    public async Task RegisterPortUsageAsync(PortUsage usage)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // Primero obtener o crear el puerto
            var portId = await GetOrCreatePortAsync(connection, usage);

            await connection.ExecuteAsync(
                @"sp_RegistrarUsoPuerto",
                new
                {
                    PuertoID = portId,
                    PerifericoID = usage.PeripheralId,
                    Estado = usage.Status
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar uso de puerto");
        }
    }

    public async Task ClosePortUsageAsync(int portId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                @"UPDATE UsoPuertos
                  SET FechaFin = GETDATE(),
                      Duracion = DATEDIFF(SECOND, FechaInicio, GETDATE())
                  WHERE PuertoID = @PortId AND FechaFin IS NULL",
                new { PortId = portId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cerrar uso del puerto {portId}");
        }
    }

    private async Task<int> GetOrCreatePortAsync(SqlConnection connection, PortUsage usage)
    {
        // NO crear puertos automáticamente - solo usar puertos físicos predefinidos
        // Intentar encontrar un puerto físico existente
        var existingPort = await connection.QueryFirstOrDefaultAsync<int?>(
            @"SELECT ID FROM Puertos WHERE Nombre = @Name",
            new { Name = $"Puerto_{usage.PortId}" });

        if (existingPort.HasValue)
            return existingPort.Value;

        // Si no existe, usar el ID 1 como puerto genérico (USB-A Puerto 1)
        // En lugar de crear puertos dinámicamente
        return 1; // Puerto por defecto
    }

    public async Task UpdatePhysicalPortStatusAsync(string portName, bool isConnected, string? connectedDevice)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                @"UPDATE Puertos
                  SET EstaConectado = @IsConnected,
                      DispositivoConectado = @ConnectedDevice,
                      UltimaDeteccion = GETDATE()
                  WHERE Nombre = @PortName",
                new
                {
                    PortName = portName,
                    IsConnected = isConnected,
                    ConnectedDevice = connectedDevice
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error actualizando estado del puerto {portName}");
        }
    }
}
