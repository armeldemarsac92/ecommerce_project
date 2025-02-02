import axios from 'axios';

export const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNGM5ZGY4ZS1jYmVhLTQ5ODEtOWJhYS0yYjBiMzY1Y2I1NjIiLCJlbWFpbCI6ImR1bWVzbmlsX2pvbmF0aGFuQG91dGxvb2suZnIiLCJqdGkiOiI1OGYwMTRhOS1hZTg5LTRmOGQtYTRjOC04NmFmOTZjNGNiMWUiLCJpYXQiOjE3Mzg1MjM2MzAsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkdW1lc25pbF9qb25hdGhhbkBvdXRsb29rLmZyIiwiZ2l2ZW5fbmFtZSI6ImpvbmF0aGFuIiwiZmFtaWx5X25hbWUiOiJkdW1lc25pbCIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsInN0cmlwZV9pZCI6IiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM4NTI3MjMwLCJpc3MiOiJhdXRoLmVwaXRlY2hwcm9qZWN0LmZyIiwiYXVkIjoiYXV0aC5lcGl0ZWNocHJvamVjdC5mciJ9.bStBiUNCoJGzpxbuuJm5ch9mKMX8ibEmRyRQ15RLESuvzcmFb_lH2klr60W1_g_EUxO49NY8ut-vNOJU6_t1oSzt05qhMaX4DJNavVlDj4meH0JEYIC8Oelu38myZCXkppXCkWmkkW15507eNFYaR2ZMSvleBr9rCFtK2V2imnv9pbmORTzhok5fyVddxT1usAwQTgrVhKt2SR5KVBQCiKOllSnRAWaX_gr5lPrW0L-fA2TBC3Q5whuC-KTVffvksRrYG8JE6RvXwhUFeioJipibzsGMEWqA1yeX8Y1-rDVFwDmzhVVt578uLjAZBK6UvjtmkOrnX_rJjvzCdL3NvA',
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});