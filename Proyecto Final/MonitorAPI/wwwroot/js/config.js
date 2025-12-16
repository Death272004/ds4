// Configuración de la API
const API_BASE_URL = 'https://localhost:62063/api';

// Endpoints
const API_ENDPOINTS = {
    peripherals: {
        all: `${API_BASE_URL}/Peripherals`,
        active: `${API_BASE_URL}/Peripherals/active`,
        byId: (id) => `${API_BASE_URL}/Peripherals/${id}`,
        activity: (id) => `${API_BASE_URL}/Peripherals/${id}/activity`,
        stats: `${API_BASE_URL}/Peripherals/dashboard/stats`
    },
    ports: {
        recent: `${API_BASE_URL}/Ports/recent`,
        verify: (id) => `${API_BASE_URL}/Ports/${id}/verify`,
        physical: `${API_BASE_URL}/Ports/physical`
    }
};

// Configuración de refresco automático (en milisegundos)
const AUTO_REFRESH_INTERVAL = 5000; // 5 segundos
