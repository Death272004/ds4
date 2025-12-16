# Configuración Global del Sistema
# Edita este archivo para cambiar configuraciones comunes

## Configuración de Base de Datos
$DATABASE_SERVER = "localhost"
$DATABASE_NAME = "MonitorPuertos"
$USE_INTEGRATED_SECURITY = $true
$DATABASE_USER = ""  # Solo si no usas Integrated Security
$DATABASE_PASSWORD = ""  # Solo si no usas Integrated Security

## Configuración del Servicio de Monitoreo
$SCAN_INTERVAL_SECONDS = 5  # Intervalo entre escaneos de periféricos
$VALIDATION_THRESHOLD = 3  # Número de detecciones consecutivas requeridas

## Configuración de la API
$API_PORT = 5000

## Configuración de Auto-refresh del Frontend
$AUTO_REFRESH_INTERVAL_MS = 5000  # Milisegundos

# ============================================
# NO EDITAR DEBAJO DE ESTA LÍNEA
# ============================================

Write-Host "Aplicando configuración..." -ForegroundColor Yellow

$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# Construir cadena de conexión
if ($USE_INTEGRATED_SECURITY) {
    $connectionString = "Server=$DATABASE_SERVER;Database=$DATABASE_NAME;Integrated Security=true;TrustServerCertificate=true;"
}
else {
    $connectionString = "Server=$DATABASE_SERVER;Database=$DATABASE_NAME;User Id=$DATABASE_USER;Password=$DATABASE_PASSWORD;TrustServerCertificate=true;"
}

# Actualizar appsettings del Servicio
$serviceAppSettings = Join-Path $rootPath "MonitorService\appsettings.json"
if (Test-Path $serviceAppSettings) {
    $json = Get-Content $serviceAppSettings -Raw | ConvertFrom-Json
    $json.ConnectionStrings.DefaultConnection = $connectionString
    $json.MonitorSettings.ScanIntervalSeconds = $SCAN_INTERVAL_SECONDS
    $json.MonitorSettings.ValidationThresholdSeconds = $VALIDATION_THRESHOLD
    $json | ConvertTo-Json -Depth 10 | Set-Content $serviceAppSettings
    Write-Host "✅ Configuración del Servicio actualizada" -ForegroundColor Green
}

# Actualizar appsettings de la API
$apiAppSettings = Join-Path $rootPath "MonitorAPI\appsettings.json"
if (Test-Path $apiAppSettings) {
    $json = Get-Content $apiAppSettings -Raw | ConvertFrom-Json
    $json.ConnectionStrings.DefaultConnection = $connectionString
    $json | ConvertTo-Json -Depth 10 | Set-Content $apiAppSettings
    Write-Host "✅ Configuración de la API actualizada" -ForegroundColor Green
}

# Actualizar config.js del Frontend
$frontendConfig = Join-Path $rootPath "MonitorAPI\wwwroot\js\config.js"
if (Test-Path $frontendConfig) {
    $configContent = @"
// Configuración de la API
const API_BASE_URL = 'http://localhost:$API_PORT/api';

// Endpoints
const API_ENDPOINTS = {
    peripherals: {
        all: `${'$'}{API_BASE_URL}/Peripherals`,
        active: `${('$')}{API_BASE_URL}/Peripherals/active`,
        byId: (id) => `${('$')}{API_BASE_URL}/Peripherals/${('$')}{id}`,
        activity: (id) => `${('$')}{API_BASE_URL}/Peripherals/${('$')}{id}/activity`,
        stats: `${('$')}{API_BASE_URL}/Peripherals/dashboard/stats`
    },
    ports: {
        recent: `${('$')}{API_BASE_URL}/Ports/recent`,
        verify: (id) => `${('$')}{API_BASE_URL}/Ports/${('$')}{id}/verify`
    }
};

// Configuración de refresco automático (en milisegundos)
const AUTO_REFRESH_INTERVAL = $AUTO_REFRESH_INTERVAL_MS;
"@
    $configContent | Set-Content $frontendConfig
    Write-Host "✅ Configuración del Frontend actualizada" -ForegroundColor Green
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Configuración Aplicada" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📊 Base de Datos:" -ForegroundColor Yellow
Write-Host "   Servidor: $DATABASE_SERVER" -ForegroundColor White
Write-Host "   Base de Datos: $DATABASE_NAME" -ForegroundColor White
Write-Host ""
Write-Host "⚙️  Servicio de Monitoreo:" -ForegroundColor Yellow
Write-Host "   Intervalo de escaneo: $SCAN_INTERVAL_SECONDS segundos" -ForegroundColor White
Write-Host "   Validaciones requeridas: $VALIDATION_THRESHOLD" -ForegroundColor White
Write-Host ""
Write-Host "🌐 API:" -ForegroundColor Yellow
Write-Host "   Puerto: $API_PORT" -ForegroundColor White
Write-Host ""
Write-Host "✨ Frontend:" -ForegroundColor Yellow
Write-Host "   Auto-refresh: $AUTO_REFRESH_INTERVAL_MS ms" -ForegroundColor White
Write-Host ""
