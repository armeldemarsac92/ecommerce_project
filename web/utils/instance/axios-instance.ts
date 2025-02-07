import axios from 'axios';

const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNGM5ZGY4ZS1jYmVhLTQ5ODEtOWJhYS0yYjBiMzY1Y2I1NjIiLCJlbWFpbCI6ImR1bWVzbmlsX2pvbmF0aGFuQG91dGxvb2suZnIiLCJqdGkiOiIyYjdhNWRmYS1jMjdjLTQ2MDEtOWM3ZS1lNzNmNmViMTUwMGEiLCJpYXQiOjE3Mzg5NjQxMTYsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkdW1lc25pbF9qb25hdGhhbkBvdXRsb29rLmZyIiwiZ2l2ZW5fbmFtZSI6ImpvbmF0aGFuIiwiZmFtaWx5X25hbWUiOiJkdW1lc25pbCIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsInN0cmlwZV9pZCI6IiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM4OTY3NzE2LCJpc3MiOiJhdXRoLmVwaXRlY2hwcm9qZWN0LmZyIiwiYXVkIjoiYXV0aC5lcGl0ZWNocHJvamVjdC5mciJ9.ERG-V7p4qCAcE7-iXANTuuRqjKxhOTJxq4px5xdjor8F683nPljzHCddpxieJl8fdu--bu2sMoiSMqo33KpQgt5qFcDMaCtMTmhcakES5FhH9Z_ci7PL18C_4-5Q1DHP9DllPrY17F9l9au2BMiekNlfWinpe--wsgkE54qCgJq_U5UuYO0GuMbrM-mE1L9UfhQGZszT0YJ6KGNXZdlTwv6YwEP0e_JpGtkdsgGdXydYXGlfbR6OyNhXACU3yi9u075y_NLTZTZQm98vdbZrzRqJKz96NkqaXx4muUZZ-LMQ_EulZuAH1nFmNE16qhM-HlwgwlGCsIcBPnZpguGtmQ',
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