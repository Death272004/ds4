using Dapper;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace MonitorAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly string _connectionString;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IConfiguration configuration, ILogger<ReportService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("ConnectionString not found");
            _logger = logger;

            // Configurar licencia de EPPlus (no comercial)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Generar reporte de conexiones recientes (últimas 50)
        /// </summary>
        public async Task<byte[]> GenerateRecentConnectionsReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT TOP 50
                    up.ID,
                    p.Nombre AS Puerto,
                    pe.Nombre AS Dispositivo,
                    up.FechaConexion,
                    up.FechaDesconexion,
                    CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 'Conectado'
                        ELSE 'Desconectado'
                    END AS Estado,
                    CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 
                            DATEDIFF(SECOND, up.FechaConexion, GETDATE())
                        ELSE 
                            DATEDIFF(SECOND, up.FechaConexion, up.FechaDesconexion)
                    END AS DuracionSegundos
                FROM UsoPuertos up
                INNER JOIN Puertos p ON up.PuertoID = p.ID
                LEFT JOIN Perifericos pe ON up.PerifericoID = pe.ID
                ORDER BY up.FechaConexion DESC";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Conexiones Recientes",
                "Últimas 50 conexiones registradas en el sistema",
                new[] { "ID", "Puerto", "Dispositivo", "Fecha Conexión", "Fecha Desconexión", "Estado", "Duración (seg)" }
            );
        }

        /// <summary>
        /// Generar reporte de uso de puertos (estadísticas)
        /// </summary>
        public async Task<byte[]> GeneratePortUsageReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT 
                    p.Nombre AS Puerto,
                    p.TipoPuerto,
                    COUNT(up.ID) AS TotalConexiones,
                    COUNT(DISTINCT up.PerifericoID) AS DispositivosUnicos,
                    SUM(CASE WHEN up.FechaDesconexion IS NULL THEN 1 ELSE 0 END) AS ConexionesActivas,
                    AVG(CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 
                            DATEDIFF(SECOND, up.FechaConexion, GETDATE())
                        ELSE 
                            DATEDIFF(SECOND, up.FechaConexion, up.FechaDesconexion)
                    END) AS DuracionPromedioSegundos
                FROM Puertos p
                LEFT JOIN UsoPuertos up ON p.ID = up.PuertoID
                GROUP BY p.Nombre, p.TipoPuerto
                ORDER BY TotalConexiones DESC";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Uso por Puerto",
                "Estadísticas de uso de cada puerto",
                new[] { "Puerto", "Tipo Puerto", "Total Conexiones", "Dispositivos Únicos", "Conexiones Activas", "Duración Promedio (seg)" }
            );
        }

        /// <summary>
        /// Generar historial completo de dispositivos
        /// </summary>
        public async Task<byte[]> GenerateDeviceHistoryReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT 
                    pe.Nombre AS Dispositivo,
                    pe.Tipo,
                    pe.Fabricante,
                    pe.Modelo,
                    COUNT(up.ID) AS TotalConexiones,
                    MAX(up.FechaConexion) AS UltimaConexion,
                    SUM(CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 
                            DATEDIFF(SECOND, up.FechaConexion, GETDATE())
                        ELSE 
                            DATEDIFF(SECOND, up.FechaConexion, up.FechaDesconexion)
                    END) AS TiempoTotalSegundos
                FROM Perifericos pe
                LEFT JOIN UsoPuertos up ON pe.ID = up.PerifericoID
                GROUP BY pe.Nombre, pe.Tipo, pe.Fabricante, pe.Modelo
                ORDER BY TotalConexiones DESC";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Historial de Dispositivos",
                "Información de todos los dispositivos detectados",
                new[] { "Dispositivo", "Tipo", "Fabricante", "Modelo", "Total Conexiones", "Última Conexión", "Tiempo Total (seg)" }
            );
        }

        /// <summary>
        /// Generar actividad detallada por puerto
        /// </summary>
        public async Task<byte[]> GeneratePortActivityReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT 
                    ap.FechaHora,
                    p.Nombre AS Puerto,
                    ap.TipoEvento,
                    ap.Descripcion
                FROM ActividadPerifericos ap
                INNER JOIN Puertos p ON ap.PuertoID = p.ID
                ORDER BY ap.FechaHora DESC";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Actividad de Puertos",
                "Registro completo de eventos en los puertos",
                new[] { "Fecha y Hora", "Puerto", "Tipo de Evento", "Descripción" }
            );
        }

        /// <summary>
        /// Generar reporte de duración de conexiones
        /// </summary>
        public async Task<byte[]> GenerateConnectionDurationReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT 
                    p.Nombre AS Puerto,
                    pe.Nombre AS Dispositivo,
                    up.FechaConexion,
                    up.FechaDesconexion,
                    CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 
                            DATEDIFF(SECOND, up.FechaConexion, GETDATE())
                        ELSE 
                            DATEDIFF(SECOND, up.FechaConexion, up.FechaDesconexion)
                    END AS DuracionSegundos,
                    CASE 
                        WHEN up.FechaDesconexion IS NULL THEN 'En uso'
                        ELSE 'Finalizado'
                    END AS Estado
                FROM UsoPuertos up
                INNER JOIN Puertos p ON up.PuertoID = p.ID
                LEFT JOIN Perifericos pe ON up.PerifericoID = pe.ID
                ORDER BY DuracionSegundos DESC";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Duración de Conexiones",
                "Tiempo de uso de cada conexión",
                new[] { "Puerto", "Dispositivo", "Fecha Conexión", "Fecha Desconexión", "Duración (seg)", "Estado" }
            );
        }

        /// <summary>
        /// Generar reporte completo con toda la información
        /// </summary>
        public async Task<byte[]> GenerateFullReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
                SELECT 
                    p.Nombre AS Puerto,
                    p.TipoPuerto,
                    p.Ubicacion,
                    CASE WHEN p.EstaConectado = 1 THEN 'Sí' ELSE 'No' END AS Conectado,
                    p.DispositivoConectado,
                    p.UltimaDeteccion,
                    COUNT(up.ID) AS TotalConexiones
                FROM Puertos p
                LEFT JOIN UsoPuertos up ON p.ID = up.PuertoID
                GROUP BY p.Nombre, p.TipoPuerto, p.Ubicacion, p.EstaConectado, p.DispositivoConectado, p.UltimaDeteccion
                ORDER BY p.Nombre";

            var data = await connection.QueryAsync(query);

            return GenerateExcel(
                data,
                "Reporte Completo",
                "Vista general del sistema de monitoreo de puertos",
                new[] { "Puerto", "Tipo", "Ubicación", "Conectado", "Dispositivo Actual", "Última Detección", "Total Conexiones" }
            );
        }

        /// <summary>
        /// Genera archivo Excel a partir de datos dinámicos
        /// </summary>
        private byte[] GenerateExcel(IEnumerable<dynamic> data, string reportTitle, string description, string[] columnHeaders)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(reportTitle);

            var dataList = data.ToList();
            int totalColumns = columnHeaders.Length;

            // Configurar título del reporte
            worksheet.Cells[1, 1].Value = reportTitle;
            worksheet.Cells[1, 1, 1, totalColumns].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
            worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
            worksheet.Row(1).Height = 25;

            // Descripción
            worksheet.Cells[2, 1].Value = description;
            worksheet.Cells[2, 1, 2, totalColumns].Merge = true;
            worksheet.Cells[2, 1].Style.Font.Italic = true;
            worksheet.Row(2).Height = 18;

            // Fecha de generación
            worksheet.Cells[3, 1].Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            worksheet.Cells[3, 1, 3, totalColumns].Merge = true;
            worksheet.Cells[3, 1].Style.Font.Size = 9;
            worksheet.Cells[3, 1].Style.Font.Color.SetColor(Color.Gray);
            worksheet.Row(3).Height = 15;

            // Encabezados de columnas (fila 5)
            for (int i = 0; i < totalColumns; i++)
            {
                var cell = worksheet.Cells[5, i + 1];
                cell.Value = columnHeaders[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            worksheet.Row(5).Height = 20;

            // Datos
            int row = 6;
            foreach (var item in dataList)
            {
                var dict = item as IDictionary<string, object>;
                if (dict == null) continue;

                int col = 1;
                foreach (var value in dict.Values)
                {
                    var cell = worksheet.Cells[row, col];

                    if (value == null || value is DBNull)
                    {
                        cell.Value = "N/A";
                    }
                    else if (value is DateTime dateValue)
                    {
                        cell.Value = dateValue;
                        cell.Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    }
                    else
                    {
                        cell.Value = value;
                    }

                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    col++;
                }
                row++;
            }

            // Total de registros
            int totalRow = row + 1;
            worksheet.Cells[totalRow, 1].Value = $"Total de registros: {dataList.Count}";
            worksheet.Cells[totalRow, 1, totalRow, totalColumns].Merge = true;
            worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
            worksheet.Cells[totalRow, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[totalRow, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

            // Ajustar ancho de columnas solo si hay datos
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }
            else
            {
                // Si no hay datos, ajustar manualmente las columnas de encabezado
                for (int i = 1; i <= totalColumns; i++)
                {
                    worksheet.Column(i).AutoFit();
                }
            }

            // Congelar paneles (mantener encabezados visibles)
            worksheet.View.FreezePanes(6, 1);

            return package.GetAsByteArray();
        }
    }
}
