import axios from 'axios';

export const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false // Pour le développement local uniquement
    })
});