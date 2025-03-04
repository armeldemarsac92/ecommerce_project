import axios, {AxiosError, InternalAxiosRequestConfig} from 'axios';
import Cookies from 'js-cookie';

interface RetryConfig extends InternalAxiosRequestConfig {
    _retry?: boolean;
}

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || 'https://api.epitechproject.fr/api/v1';
const AUTH_BASE_URL = process.env.NEXT_PUBLIC_AUTH_BASE_URL || 'https://auth.epitechproject.fr/api';


const axiosInstance = axios.create({
    baseURL: API_BASE_URL,
    timeout: 60000,

    headers: {
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});

const authAxiosInstance = axios.create({
    baseURL: AUTH_BASE_URL,
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});

// Variable pour éviter les appels multiples au refresh
let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any | null, token: string | null = null) => {
    failedQueue.forEach(prom => {
        if (error) {
            prom.reject(error);
        } else {
            prom.resolve(token);
        }
    });

    failedQueue = [];
};

// Intercepteur pour ajouter le token à chaque requête
axiosInstance.interceptors.request.use(
    (config) => {
        const token = Cookies.get('access_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Intercepteur pour gérer les erreurs 401
axiosInstance.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as RetryConfig;

        if (!originalRequest) {
            return Promise.reject(error);
        }

        const accessToken = Cookies.get('access_token');

        // Si erreur 401 et qu'on n'a pas déjà essayé de refresh
        if (error.response?.status === 401 && !originalRequest._retry || accessToken === null || accessToken === undefined) {
            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                })
                    .then(token => {
                        originalRequest.headers.Authorization = `Bearer ${token}`;
                        return axios(originalRequest);
                    })
                    .catch(err => {
                        return Promise.reject(err);
                    });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            try {
                const refreshToken = Cookies.get('refresh_token');
                const response = await authAxiosInstance.post('/auth/refresh', { refreshToken });

                const { accessToken, refreshToken: newRefreshToken } = response.data;

                // Mettre à jour les cookies
                Cookies.set('access_token', accessToken);
                Cookies.set('refresh_token', newRefreshToken);

                // Mettre à jour le header de la requête originale
                originalRequest.headers.Authorization = `Bearer ${accessToken}`;

                // Traiter la file d'attente
                processQueue(null, accessToken);

                return axios(originalRequest);
            } catch (err) {
                processQueue(err, null);
                // En cas d'échec du refresh, déconnexion
                Cookies.remove('access_token');
                Cookies.remove('refresh_token');
                window.location.href = '/sign-in';
                return Promise.reject(err);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    }
);

export { axiosInstance, authAxiosInstance };