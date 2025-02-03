import axios from 'axios';

const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});

const authAxiosInstance  = axios.create({
    baseURL: 'https://localhost:7073/api',
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false // Pour le développement local uniquement
    })
});

export {
    axiosInstance,
    authAxiosInstance
}