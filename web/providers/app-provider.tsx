// providers/AppProvider.tsx
'use client';

import { useState, useEffect } from "react";
import Cookies from 'js-cookie';
import {jwtDecode} from 'jwt-decode';
import {AppContext, AuthenticatedUser, IAuthTokens} from "@/contexts/app-context";

export function AppProvider({ children }: { children: React.ReactNode }) {
    const [authenticated_user, setAuthenticatedUser] = useState<AuthenticatedUser | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const storeTokens = (tokens: IAuthTokens) => {
        Cookies.set('access_token', tokens.accessToken, {
            expires: tokens.expiresIn / (24 * 60 * 60),
            path: '/',
            httpOnly: false
            /*secure: process.env.NODE_ENV === 'production',*/
        });

        Cookies.set('refresh_token', tokens.refreshToken, {
            expires: 7,
            path: '/',
            httpOnly: false
            /*secure: process.env.NODE_ENV === 'production',*/
        });

        setIsLoading(true)
    };

    const logout = () => {
        Cookies.remove('access_token');
        Cookies.remove('refresh_token');
        setAuthenticatedUser(null);
    };

    const getAccessToken = () => Cookies.get('access_token') || null;

    useEffect(() => {
        const checkAuth = () => {
            try {
                const token = Cookies.get('access_token');
                console.log("Token from cookie:", token); // Pour debug

                if (!token) {
                    setIsLoading(false);
                    return;
                }

                const decoded = jwtDecode<AuthenticatedUser>(token);
                console.log("Decoded token:", decoded); // Pour debug

                setAuthenticatedUser({
                    email: decoded.email,
                    email_verified: decoded.email_verified,
                    family_name: decoded.family_name,
                    given_name: decoded.given_name,
                });
            } catch (error) {
                console.error('Auth check failed:', error);
                logout();
            } finally {
                setIsLoading(false);
            }
        };

        checkAuth();
    }, []);

    return (
        <AppContext.Provider
            value={{
                authenticated_user,
                isAuthenticated: !!authenticated_user,
                isLoading,
                storeTokens,
                logout,
                getAccessToken
            }}
        >
            {children}
        </AppContext.Provider>
    );
}