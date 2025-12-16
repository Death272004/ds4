# Script de Ejecución - Sistema de Monitoreo de Periféricos
# Inicia ambos servicios automáticamente

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Iniciando Sistema de Monitoreo" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$servicePath = Join-Path $rootPath "MonitorService"
$apiPath = Join-Path $rootPath "MonitorAPI"

# Función para iniciar proceso en nueva ventana
function Start-ServiceInNewWindow {
    param (
        [string]$Path,
        [string]$Title,
        [string]$Command
    )
    
    if (Test-Path $Path) {
        Write-Host "🚀 Iniciando $Title..." -ForegroundColor Green
        Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$Path'; Write-Host '$Title' -ForegroundColor Cyan; $Command"
        Write-Host "✅ $Title iniciado en nueva ventana" -ForegroundColor Green
        Start-Sleep -Seconds 2
    }
    else {
        Write-Host "❌ No se encontró: $Path" -ForegroundColor Red
    }
}

# Iniciar Servicio de Monitoreo
Start-ServiceInNewWindow -Path $servicePath -Title "Servicio de Monitoreo" -Command "dotnet run"

# Iniciar Web API
Start-ServiceInNewWindow -Path $apiPath -Title "Web API" -Command "dotnet run"

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Sistema Iniciado" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📡 Servicios ejecutándose:" -ForegroundColor Yellow
Write-Host "   • Servicio de Monitoreo: Detectando periféricos" -ForegroundColor White
Write-Host "   • Web API: http://localhost:5000" -ForegroundColor White
Write-Host ""
Write-Host "🌐 Abre tu navegador en:" -ForegroundColor Yellow
Write-Host "   http://localhost:5000" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  Para detener los servicios, cierra las ventanas de PowerShell" -ForegroundColor Yellow
Write-Host ""

# Esperar 3 segundos y abrir el navegador
Write-Host "🌐 Abriendo navegador en 3 segundos..." -ForegroundColor Green
Start-Sleep -Seconds 3
Start-Process "http://localhost:5000"

Write-Host ""
Write-Host "✨ ¡Sistema listo! Presiona cualquier tecla para salir..." -ForegroundColor Green
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
