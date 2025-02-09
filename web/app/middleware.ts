import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
    const { pathname } = request.nextUrl;

    const accessToken = request.cookies.get('access_token')?.value;
    const refreshToken = request.cookies.get('refresh_token')?.value;

    if (pathname === '/') {
        return NextResponse.redirect(new URL('/dashboard', request.url));
    }

    // Bloquer immédiatement toute requête vers des routes protégées sans tokens
    if (pathname.startsWith('/dashboard')) {
        if (!accessToken && !refreshToken) {
            // Redirection immédiate
            return NextResponse.redirect(new URL('/sign-in', request.url));
        }
    }

    // Empêcher l'accès aux pages d'auth si déjà connecté
    if ((accessToken || refreshToken) && pathname.startsWith('/sign-in')) {
        return NextResponse.redirect(new URL('/dashboard', request.url));
    }

    return NextResponse.next();
}

// Être plus spécifique dans le matcher
export const config = {
    matcher: [
        '/',
        '/dashboard/:path*',
        '/sign-in/:path*',
    ]
};