import axios from 'axios';

export const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Authorization': `Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNGM5ZGY4ZS1jYmVhLTQ5ODEtOWJhYS0yYjBiMzY1Y2I1NjIiLCJlbWFpbCI6ImR1bWVzbmlsX2pvbmF0aGFuQG91dGxvb2suZnIiLCJqdGkiOiJmNzM2YmE3Ny00OTIzLTQ0NzItODM3MC1kYzczYTUyZGY4YmQiLCJpYXQiOjE3Mzg1OTczMDQsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkdW1lc25pbF9qb25hdGhhbkBvdXRsb29rLmZyIiwiZ2l2ZW5fbmFtZSI6ImpvbmF0aGFuIiwiZmFtaWx5X25hbWUiOiJkdW1lc25pbCIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsInN0cmlwZV9pZCI6IiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM4NjAwOTA0LCJpc3MiOiJhdXRoLmVwaXRlY2hwcm9qZWN0LmZyIiwiYXVkIjoiYXV0aC5lcGl0ZWNocHJvamVjdC5mciJ9.xtQbNidQpdDBO5jSRufMTFXpO-GSgKF4R-cAkeMwT345u2EjmZ5sZrujQIcFG_pRY51U7YFtXDgbbIHcEmgFtPu38sQm9JsgZUfnvLSTpr6fvnZkPFtZbnoAkpH8XVmeLx1ewY_zRWe1cKqi6_F7UJNAjlgRuU0xL5o7liORrSMVav-Vv7TPLirJAXCUzVHd80c99q_a0LCuY_Qv73KL24XveNlUlaCbzh09Etj4oo9ECq8AIvHcImJmEV86Gmjk7A3H0G1G7pK41lOTSprZ5GH-yjn7u7geBUhKa7mCudbNxx1h8iVcffmRDJUKx4gfWXC7kpC4o5Yiw-RxvQv0Vg`,
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});