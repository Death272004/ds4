// API Service
class ApiService {
    async fetchData(url) {
        try {
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error fetching data:', error);
            throw error;
        }
    }

    async postData(url, data = {}) {
        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error posting data:', error);
            throw error;
        }
    }

    // Periféricos
    async getAllPeripherals() {
        return this.fetchData(API_ENDPOINTS.peripherals.all);
    }

    async getActivePeripherals() {
        return this.fetchData(API_ENDPOINTS.peripherals.active);
    }

    async getPeripheralById(id) {
        return this.fetchData(API_ENDPOINTS.peripherals.byId(id));
    }

    async getPeripheralActivity(id, limit = 20) {
        return this.fetchData(`${API_ENDPOINTS.peripherals.activity(id)}?limit=${limit}`);
    }

    async getDashboardStats() {
        return this.fetchData(API_ENDPOINTS.peripherals.stats);
    }

    // Puertos
    async getRecentPorts(limit = 50) {
        return this.fetchData(`${API_ENDPOINTS.ports.recent}?limit=${limit}`);
    }

    async verifyPortStatus(portId) {
        return this.postData(API_ENDPOINTS.ports.verify(portId));
    }

    async getPhysicalPorts() {
        return this.fetchData(API_ENDPOINTS.ports.physical);
    }
}

const apiService = new ApiService();
