# Script de Instalación - Sistema de Monitoreo de Periféricos
# Ejecutar con privilegios de administrador

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Sistema de Monitoreo de Periféricos" -ForegroundColor Cyan
Write-Host "Instalación y Configuración" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# Verificar si se ejecuta como administrador
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "⚠️  ADVERTENCIA: Se recomienda ejecutar este script como Administrador" -ForegroundColor Yellow
    Write-Host ""
}

# Paso 1: Verificar SQL Server
Write-Host "📊 Paso 1: Verificando SQL Server..." -ForegroundColor Green
$sqlService = Get-Service -Name "MSSQLSERVER" -ErrorAction SilentlyContinue

if ($null -eq $sqlService) {
    Write-Host "❌ SQL Server no está instalado o no se encontró el servicio MSSQLSERVER" -ForegroundColor Red
    Write-Host "   Por favor, instala SQL Server antes de continuar." -ForegroundColor Yellow
    exit 1
}

if ($sqlService.Status -ne "Running") {
    Write-Host "⚠️  SQL Server no está ejecutándose. Iniciando..." -ForegroundColor Yellow
    Start-Service -Name "MSSQLSERVER"
    Start-Sleep -Seconds 3
}

Write-Host "✅ SQL Server está ejecutándose" -ForegroundColor Green
Write-Host ""

# Paso 2: Crear base de datos
Write-Host "📊 Paso 2: Creando base de datos..." -ForegroundColor Green
$sqlScriptPath = Join-Path $rootPath "MonitorPuertos.sql"

if (Test-Path $sqlScriptPath) {
    try {
        sqlcmd -S localhost -i $sqlScriptPath -ErrorAction Stop
        Write-Host "✅ Base de datos creada exitosamente" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Error al crear la base de datos: $_" -ForegroundColor Red
        Write-Host "   Intenta ejecutar el script SQL manualmente desde SSMS" -ForegroundColor Yellow
    }
}
else {
    Write-Host "⚠️  No se encontró el archivo MonitorPuertos.sql" -ForegroundColor Yellow
    Write-Host "   Ruta esperada: $sqlScriptPath" -ForegroundColor Yellow
}
Write-Host ""

# Paso 3: Compilar Servicio de Monitoreo
Write-Host "🔧 Paso 3: Compilando Servicio de Monitoreo..." -ForegroundColor Green
$servicePath = Join-Path $rootPath "MonitorService"

if (Test-Path $servicePath) {
    Set-Location $servicePath
    
    Write-Host "   Restaurando paquetes NuGet..." -ForegroundColor Cyan
    dotnet restore
    
    Write-Host "   Compilando proyecto..." -ForegroundColor Cyan
    dotnet build -c Release
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Servicio compilado exitosamente" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Error al compilar el servicio" -ForegroundColor Red
    }
}
else {
    Write-Host "⚠️  No se encontró la carpeta MonitorService" -ForegroundColor Yellow
}
Write-Host ""

# Paso 4: Compilar Web API
Write-Host "🌐 Paso 4: Compilando Web API..." -ForegroundColor Green
$apiPath = Join-Path $rootPath "MonitorAPI"

if (Test-Path $apiPath) {
    Set-Location $apiPath
    
    Write-Host "   Restaurando paquetes NuGet..." -ForegroundColor Cyan
    dotnet restore
    
    Write-Host "   Compilando proyecto..." -ForegroundColor Cyan
    dotnet build -c Release
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ API compilada exitosamente" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Error al compilar la API" -ForegroundColor Red
    }
}
else {
    Write-Host "⚠️  No se encontró la carpeta MonitorAPI" -ForegroundColor Yellow
}
Write-Host ""

# Volver al directorio raíz
Set-Location $rootPath

# Resumen e Instrucciones
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Instalación Completada" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Próximos Pasos:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1️⃣  Iniciar el Servicio de Monitoreo:" -ForegroundColor White
Write-Host "   cd MonitorService" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "2️⃣  En otra terminal, iniciar la Web API:" -ForegroundColor White
Write-Host "   cd MonitorAPI" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "3️⃣  Abrir el navegador en:" -ForegroundColor White
Write-Host "   http://localhost:5000" -ForegroundColor Cyan
Write-Host ""
Write-Host "📖 Para más información, consulta el archivo README.md" -ForegroundColor Yellow
Write-Host ""
Write-Host "🎉 ¡Sistema listo para usar!" -ForegroundColor Green
Write-Host ""
