/** @type {import('next').NextConfig} */
const nextConfig = {
    output: 'standalone',
    reactStrictMode: false,
    images: {
        remotePatterns: [
            {
                protocol: 'https',
                hostname: 'placehold.co',
            }
        ],
    },
    publicRuntimeConfig: {
        API_BASE_URL: process.env.NEXT_PUBLIC_API_BASE_URL,
        AUTH_BASE_URL: process.env.NEXT_PUBLIC_AUTH_BASE_URL,
    },
}

module.exports = nextConfig
