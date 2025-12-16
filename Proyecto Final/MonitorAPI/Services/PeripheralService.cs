using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MonitorAPI.Models;

namespace MonitorAPI.Services;

public class PeripheralService : IPeripheralService
{
    private readonly string _connectionString;

    public PeripheralService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
    }

    public async Task<List<PeripheralDto>> GetAllPeripheralsAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT 
                ID as Id,
                Nombre,
                DeviceID as DeviceId,
                Puerto,
                TipoPerifico,
                Icono,
                Activo,
                UltimaActividad,
                EstadoActual,
                UltimoEvento
            FROM vw_EstadoActualPerifericos
            ORDER BY UltimaActividad DESC";

        var peripherals = await connection.QueryAsync<PeripheralDto>(query);
        return peripherals.ToList();
    }

    public async Task<List<PeripheralDto>> GetActivePeripheralsAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT 
                ID as Id,
                Nombre,
                DeviceID as DeviceId,
                Puerto,
                TipoPerifico,
                Icono,
                Activo,
                UltimaActividad,
                EstadoActual,
                UltimoEvento
            FROM vw_EstadoActualPerifericos
            WHERE Activo = 1
            ORDER BY UltimaActividad DESC";

        var peripherals = await connection.QueryAsync<PeripheralDto>(query);
        return peripherals.ToList();
    }

    public async Task<PeripheralDto?> GetPeripheralByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT 
                ID as Id,
                Nombre,
                DeviceID as DeviceId,
                Puerto,
                TipoPerifico,
                Icono,
                Activo,
                UltimaActividad,
                EstadoActual,
                UltimoEvento
            FROM vw_EstadoActualPerifericos
            WHERE ID = @Id";

        return await connection.QueryFirstOrDefaultAsync<PeripheralDto>(query, new { Id = id });
    }

    public async Task<List<ActivityDto>> GetPeripheralActivityAsync(int peripheralId, int limit = 50)
    {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT TOP (@Limit)
                ID as Id,
                TipoEvento,
                Estado,
                FechaHora,
                Detalles
            FROM ActividadPerifericos
            WHERE PerifericoID = @PeripheralId AND Validado = 1
            ORDER BY FechaHora DESC";

        var activities = await connection.QueryAsync<ActivityDto>(
            query,
            new { PeripheralId = peripheralId, Limit = limit });

        return activities.ToList();
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        var stats = new DashboardStatsDto();

        stats.TotalPerifericos = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Perifericos");

        stats.PerifericosActivos = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Perifericos WHERE Activo = 1");

        stats.PerifericosInactivos = stats.TotalPerifericos - stats.PerifericosActivos;

        stats.PuertosEnUso = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM UsoPuertos WHERE FechaFin IS NULL AND Validado = 1");

        var query = @"
            SELECT TOP 10
                ID as Id,
                Nombre,
                DeviceID as DeviceId,
                Puerto,
                TipoPerifico,
                Icono,
                Activo,
                UltimaActividad,
                EstadoActual,
                UltimoEvento
            FROM vw_EstadoActualPerifericos
            ORDER BY UltimaActividad DESC";

        var recientes = await connection.QueryAsync<PeripheralDto>(query);
        stats.PerifericosRecientes = recientes.ToList();

        return stats;
    }
}
