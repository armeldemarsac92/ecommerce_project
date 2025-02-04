import axios from 'axios';

export const axiosInstance  = axios.create({
    baseURL: 'https://localhost:7143/api/v1',
    timeout: 60000,
    headers: {
        'Authorization': `Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNGM5ZGY4ZS1jYmVhLTQ5ODEtOWJhYS0yYjBiMzY1Y2I1NjIiLCJlbWFpbCI6ImR1bWVzbmlsX2pvbmF0aGFuQG91dGxvb2suZnIiLCJqdGkiOiJjMmM4MWMyZC1mNDQ0LTQ3YjgtODMzZC03YmE4MjFmM2EyYjYiLCJpYXQiOjE3Mzg2NjI5MDgsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkdW1lc25pbF9qb25hdGhhbkBvdXRsb29rLmZyIiwiZ2l2ZW5fbmFtZSI6ImpvbmF0aGFuIiwiZmFtaWx5X25hbWUiOiJkdW1lc25pbCIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsInN0cmlwZV9pZCI6IiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM4NjY2NTA4LCJpc3MiOiJhdXRoLmVwaXRlY2hwcm9qZWN0LmZyIiwiYXVkIjoiYXV0aC5lcGl0ZWNocHJvamVjdC5mciJ9.1he4Z9VMmVa3lfZzQFZhfJOyPhwfgVxX8JbA36EAEsApFioIJ_HatWMSIJ-vY8v37azIKPP5njETDtN3G9z6jclN50zOg1lOqKhKh9jWKB5WesMSAUEi-a5grimN_c3dBpZp9_UI6pIp9m3QpayPgjFUWFKSm5-zo5EX0McMbfrbk8aUXcJjwp0nP3gPmmqwb0MKmoZgHYdaUDiRPQkx7d-FA0x-N6lhPrXDlDmcGcKqRQ-VgKwpp9Lmcvja9jwc8qms1mLuNIojwPsNPXhU5iHcikrcLDuUBsXi54C1WIKeakIBCZQenMHaFa0AZoyX1m_Wsvrssu70chWwUIvTkA`,
        'Content-Type': 'application/json',
        'accept': 'application/json'
    },
    httpsAgent: new (require('https')).Agent({
        rejectUnauthorized: false
    })
});