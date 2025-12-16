using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MonitorAPI.Models;

namespace MonitorAPI.Services;

public class PortService : IPortService
{
    private readonly string _connectionString;
    private readonly IPeripheralService _peripheralService;

    public PortService(IConfiguration configuration, IPeripheralService peripheralService)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
        _peripheralService = peripheralService;
    }

    public async Task<List<PortUsageDto>> GetRecentPortUsageAsync(int limit = 50)
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT TOP (@Limit)
                ID as Id,
                NombrePuerto,
                TipoPuerto,
                Ubicacion,
                Periferico,
                TipoPeriferico,
                Estado,
                FechaInicio,
                FechaFin,
                Duracion,
                EnUso
            FROM vw_PuertosRecientes
            ORDER BY FechaInicio DESC";

        var usage = await connection.QueryAsync<PortUsageDto>(query, new { Limit = limit });
        return usage.ToList();
    }

    public async Task<PortStatusResponse> VerifyPortStatusAsync(int portId)
    {
        using var connection = new SqlConnection(_connectionString);

        // Obtener información del puerto
        var portQuery = @"
            SELECT 
                pt.ID,
                pt.Nombre,
                pt.TipoPuerto,
                pt.Activo,
                up.PerifericoID,
                up.Estado,
                up.FechaInicio
            FROM Puertos pt
            LEFT JOIN UsoPuertos up ON pt.ID = up.PuertoID AND up.FechaFin IS NULL AND up.Validado = 1
            WHERE pt.ID = @PortId";

        var portInfo = await connection.QueryFirstOrDefaultAsync(portQuery, new { PortId = portId });

        if (portInfo == null)
        {
            return new PortStatusResponse
            {
                IsValid = false,
                Status = "NotFound",
                Message = "Puerto no encontrado",
                CheckedAt = DateTime.Now
            };
        }

        // Verificar actividad reciente (últimos 30 segundos) para evitar lecturas residuales
        var recentActivityQuery = @"
            SELECT COUNT(*) 
            FROM ActividadPerifericos ap
            INNER JOIN Perifericos p ON ap.PerifericoID = p.ID
            WHERE p.Puerto = (SELECT Nombre FROM Puertos WHERE ID = @PortId)
                AND ap.FechaHora >= DATEADD(SECOND, -30, GETDATE())
                AND ap.Validado = 1
                AND ap.Estado IN ('Online', 'Ocupado', 'EnUso')";

        var recentActivity = await connection.ExecuteScalarAsync<int>(
            recentActivityQuery,
            new { PortId = portId });

        // El puerto está realmente ocupado solo si:
        // 1. Tiene un uso activo en UsoPuertos
        // 2. Y tiene actividad reciente (últimos 30 segundos)
        bool isReallyOccupied = portInfo.PerifericoID != null && recentActivity > 0;

        var response = new PortStatusResponse
        {
            IsValid = true,
            CheckedAt = DateTime.Now
        };

        if (isReallyOccupied)
        {
            response.Status = "Ocupado";
            response.Message = "Puerto en uso activo";

            // Obtener información del dispositivo conectado
            if (portInfo.PerifericoID != null)
            {
                response.ConnectedDevice = await _peripheralService.GetPeripheralByIdAsync(
                    (int)portInfo.PerifericoID);
            }
        }
        else if (portInfo.PerifericoID != null && recentActivity == 0)
        {
            response.Status = "Residual";
            response.Message = "Lectura residual detectada - sin actividad reciente";
        }
        else
        {
            response.Status = "Libre";
            response.Message = "Puerto disponible";
        }

        return response;
    }

    public async Task<List<PhysicalPortDto>> GetAllPhysicalPortsAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT 
                ID as Id,
                Nombre,
                TipoPuerto,
                Ubicacion,
                Descripcion,
                EstaConectado,
                DispositivoConectado,
                UltimaDeteccion
            FROM Puertos
            ORDER BY 
                CASE TipoPuerto
                    WHEN 'USB-A' THEN 1
                    WHEN 'USB-C' THEN 2
                    WHEN 'HDMI' THEN 3
                    WHEN 'Audio-Jack' THEN 4
                    WHEN 'SD-Card' THEN 5
                    WHEN 'Power' THEN 6
                    ELSE 7
                END";

        var ports = await connection.QueryAsync<PhysicalPortDto>(query);
        return ports.ToList();
    }
}
