using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitorService;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/monitor-service.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Iniciando Monitor de Periféricos Service");

    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = "Monitor de Periféricos";
    });

    builder.Services.AddSingleton<IPeripheralMonitor, PeripheralMonitor>();
    builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
    builder.Services.AddHostedService<MonitorWorker>();

    builder.Services.AddSerilog();

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
