// Estado de la aplicación
let autoRefreshTimer = null;

// Iconos por tipo de puerto físico
const PORT_ICONS = {
    'USB-A': 'fa-usb',
    'USB-C': 'fa-bolt',
    'HDMI': 'fa-display',
    'Audio Jack': 'fa-headphones',
    'SD Card': 'fa-sd-card',
    'Power': 'fa-plug'
};

// Inicializar aplicación
document.addEventListener('DOMContentLoaded', async () => {
    console.log('Iniciando Monitor de Puertos Físicos...');
    
    // Cargar datos iniciales
    await loadDashboard();
    
    // Iniciar refresco automático
    startAutoRefresh();
    
    // Event listener para el reporte completo
    const fullReportCard = document.getElementById('fullReportCard');
    if (fullReportCard) {
        fullReportCard.addEventListener('click', function() {
            downloadReport('full-report');
        });
        console.log('Event listener del reporte agregado correctamente');
    } else {
        console.warn('No se encontró el elemento fullReportCard');
    }
});

// Cargar dashboard completo
async function loadDashboard() {
    try {
        await Promise.all([
            loadStats(),
            loadPorts()
        ]);
    } catch (error) {
        console.error('Error cargando dashboard:', error);
        showError('Error al cargar los datos. Verifique que la API esté ejecutándose.');
    }
}

// Cargar estadísticas
async function loadStats() {
    try {
        const ports = await apiService.getPhysicalPorts();
        
        const total = ports.length;
        const connected = ports.filter(p => p.estaConectado).length;
        const free = total - connected;
        
        document.getElementById('totalDevices').textContent = total;
        document.getElementById('activeDevices').textContent = connected;
        document.getElementById('inactiveDevices').textContent = free;
    } catch (error) {
        console.error('Error cargando estadísticas:', error);
    }
}

// Cargar puertos físicos
async function loadPorts() {
    try {
        const ports = await apiService.getPhysicalPorts();
        renderPorts(ports);
    } catch (error) {
        console.error('Error cargando puertos:', error);
        document.getElementById('portsGrid').innerHTML = `
            <div class="loading">
                <i class="fas fa-exclamation-triangle"></i>
                <p>Error al cargar puertos</p>
            </div>
        `;
    }
}

// Renderizar puertos físicos
function renderPorts(ports) {
    const grid = document.getElementById('portsGrid');
    
    if (!ports || ports.length === 0) {
        grid.innerHTML = `
            <div class="loading">
                <i class="fas fa-inbox"></i>
                <p>No se detectaron puertos</p>
            </div>
        `;
        return;
    }

    grid.innerHTML = ports.map(port => {
        const icon = PORT_ICONS[port.tipoPuerto] || 'fa-plug';
        const statusClass = port.estaConectado ? 'online' : 'offline';
        const statusText = port.estaConectado ? 'Conectado' : 'Libre';
        const deviceText = port.dispositivoConectado || 'Ninguno';
        const lastDetection = port.ultimaDeteccion 
            ? formatDateTime(port.ultimaDeteccion) 
            : 'Sin actividad';
        
        return `
            <div class="peripheral-card ${statusClass}">
                <div class="peripheral-icon">
                    <i class="fas ${icon}"></i>
                </div>
                <div class="peripheral-name">${port.nombre}</div>
                <div class="peripheral-type">${port.tipoPuerto}</div>
                <span class="peripheral-status status-${statusClass}">
                    ${statusText}
                </span>
                <div class="peripheral-port">
                    <i class="fas fa-map-pin"></i> ${port.ubicacion || 'N/A'}
                </div>
                <div class="peripheral-device">
                    <strong>Dispositivo:</strong> ${deviceText}
                </div>
                <div class="peripheral-detection">
                    <small>${lastDetection}</small>
                </div>
            </div>
        `;
    }).join('');
}

// Refrescar puertos
async function refreshPorts() {
    await loadStats();
    await loadPorts();
}

// Auto-refresh
function startAutoRefresh() {
    if (autoRefreshTimer) {
        clearInterval(autoRefreshTimer);
    }
    
    autoRefreshTimer = setInterval(async () => {
        console.log('Auto-refresh...');
        await loadDashboard();
    }, AUTO_REFRESH_INTERVAL);
}

function stopAutoRefresh() {
    if (autoRefreshTimer) {
        clearInterval(autoRefreshTimer);
        autoRefreshTimer = null;
    }
}

// Utilidades de formato
function formatDateTime(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleString('es-ES', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });
}

function formatDuration(seconds) {
    if (!seconds) return '0s';
    
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;
    
    const parts = [];
    if (hours > 0) parts.push(`${hours}h`);
    if (minutes > 0) parts.push(`${minutes}m`);
    if (secs > 0 || parts.length === 0) parts.push(`${secs}s`);
    
    return parts.join(' ');
}

function showError(message) {
    alert(message);
}

// === MÓDULO DE REPORTES ===

// Abrir modal de reportes
function openReportsModal() {
    document.getElementById('reportsModal').style.display = 'block';
}

// Cerrar modal de reportes
function closeReportsModal() {
    document.getElementById('reportsModal').style.display = 'none';
}

// Descargar reporte en formato Excel
async function downloadReport(reportType) {
    const targetElement = document.getElementById('fullReportCard');
    if (!targetElement) {
        showError('No se pudo encontrar el elemento del reporte');
        return;
    }
    
    const originalContent = targetElement.innerHTML;
    
    try {
        console.log(`Descargando reporte: ${reportType} en formato Excel`);
        
        // Mostrar indicador de carga
        targetElement.innerHTML = '<i class="fas fa-spinner fa-spin" style="font-size: 3rem;"></i><h3>Generando reporte...</h3>';
        targetElement.style.pointerEvents = 'none';
        targetElement.style.opacity = '0.7';
        
        const response = await fetch(`${API_BASE_URL}/Reports/${reportType}`);
        
        if (!response.ok) {
            throw new Error(`Error al generar reporte: ${response.statusText}`);
        }
        
        // Obtener el nombre del archivo desde el header o usar uno por defecto
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = `reporte_${reportType}_${new Date().toISOString().split('T')[0]}.xlsx`;
        
        if (contentDisposition) {
            const filenameMatch = contentDisposition.match(/filename="?(.+)"?/i);
            if (filenameMatch) {
                filename = filenameMatch[1];
            }
        }
        
        // Descargar el archivo
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        
        // Restaurar botón
        targetElement.innerHTML = originalContent;
        targetElement.style.pointerEvents = 'auto';
        targetElement.style.opacity = '1';
        
        // Mostrar mensaje de éxito
        showSuccessMessage('Reporte Excel descargado exitosamente');
        
    } catch (error) {
        console.error('Error descargando reporte:', error);
        showError('Error al descargar el reporte: ' + error.message);
        
        // Restaurar botón
        targetElement.innerHTML = originalContent;
        targetElement.style.pointerEvents = 'auto';
        targetElement.style.opacity = '1';
    }
}

// Mostrar mensaje de éxito
function showSuccessMessage(message) {
    // Crear elemento de notificación
    const notification = document.createElement('div');
    notification.className = 'success-notification';
    notification.innerHTML = `
        <i class="fas fa-check-circle"></i>
        <span>${message}</span>
    `;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #10b981;
        color: white;
        padding: 15px 25px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        z-index: 10000;
        display: flex;
        align-items: center;
        gap: 10px;
        animation: slideIn 0.3s ease;
    `;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// Cerrar modal al hacer clic fuera
window.onclick = function(event) {
    const reportsModal = document.getElementById('reportsModal');
    const peripheralModal = document.getElementById('peripheralModal');
    
    if (event.target === reportsModal) {
        closeReportsModal();
    }
    if (event.target === peripheralModal) {
        closeModal();
    }
}

