import axios from 'axios';

const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNGM5ZGY4ZS1jYmVhLTQ5ODEtOWJhYS0yYjBiMzY1Y2I1NjIiLCJlbWFpbCI6ImR1bWVzbmlsX2pvbmF0aGFuQG91dGxvb2suZnIiLCJqdGkiOiI5YjdhYmQzZi1kY2JhLTRkOWMtOTRiYi0wYzNkZTQxMjM5NjciLCJpYXQiOjE3Mzg4NzgxMzIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkdW1lc25pbF9qb25hdGhhbkBvdXRsb29rLmZyIiwiZ2l2ZW5fbmFtZSI6ImpvbmF0aGFuIiwiZmFtaWx5X25hbWUiOiJkdW1lc25pbCIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsInN0cmlwZV9pZCI6IiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM4ODgxNzMyLCJpc3MiOiJhdXRoLmVwaXRlY2hwcm9qZWN0LmZyIiwiYXVkIjoiYXV0aC5lcGl0ZWNocHJvamVjdC5mciJ9.OuW1e1XFutEa4EDgjq9svLa7txxxlsIa2NeZJ-ITlJCGwAkx6MHzefFfgjr10Esk10hxDg26-bOstu_M2IPLOjRe_labO1H1p2jmUc7_QmFEtHlMpz1HXzuAl-AcN8Klx9Nf6OcCLBN3LO_rEhg6ivz1nc7EAsLZjF2dz8eXq3zJvv1qe25ZL8lltLwMcFv4E6NLkdaVRuLQzb3nIM6SKafxjFXL1eaMjWjdHMtuOP4RcmryEj3KyDKxXIpJ1OUTzaHExVu08AYlP7TNh4cp1XlGQ06pJRFqu3Edpe9c4kcIpiqKTvNM5RPJ6rJ8Rqs4_YzhYgFXRsZNf0H-pcRk1Q',
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