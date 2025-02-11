'use client';

import { useState, useEffect } from "react";
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { AppContext, AuthenticatedUser, IAuthTokens } from "@/contexts/app-context";
import { useRouter } from "next/navigation";

export function AppProvider({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    const [authenticated_user, setAuthenticatedUser] = useState<AuthenticatedUser | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [isMounted, setIsMounted] = useState(false);

    // Gérer la redirection dans un useEffect séparé
    useEffect(() => {
        if (!isLoading && !authenticated_user) {
            router.push("/sign-in");
        }
    }, [isLoading, authenticated_user]);

    useEffect(() => {
        setIsMounted(true);

        const cachedUser = sessionStorage.getItem('authenticated_user');
        if (cachedUser) {
            setAuthenticatedUser(JSON.parse(cachedUser));
            setIsLoading(false);
            return;
        }

        const checkAuth = () => {
            try {
                const token = Cookies.get('access_token');

                if (!token) {
                    setIsLoading(false);
                    return; // Enlever le router.push d'ici
                }

                const decoded = jwtDecode<AuthenticatedUser>(token);
                const userData = {
                    email: decoded.email,
                    email_verified: decoded.email_verified,
                    family_name: decoded.family_name,
                    given_name: decoded.given_name,
                    picture: decoded.picture
                };

                sessionStorage.setItem('authenticated_user', JSON.stringify(userData));
                setAuthenticatedUser(userData);
            } catch (error) {
                console.error('Auth check failed:', error);
                logout();
            } finally {
                setIsLoading(false);
            }
        };

        checkAuth();
    }, []);

    const storeTokens = (tokens: IAuthTokens) => {
        Cookies.set('access_token', tokens.accessToken, {
            expires: tokens.expiresIn / (24 * 60 * 60),
            path: '/',
            httpOnly: false
        });

        Cookies.set('refresh_token', tokens.refreshToken, {
            expires: 7,
            path: '/',
            httpOnly: false
        });

        try {
            const decoded = jwtDecode<AuthenticatedUser>(tokens.accessToken);
            const userData = {
                email: decoded.email,
                email_verified: decoded.email_verified,
                family_name: decoded.family_name,
                given_name: decoded.given_name,
                picture: decoded.picture
            };

            sessionStorage.setItem('authenticated_user', JSON.stringify(userData));
            setAuthenticatedUser(userData);
            setIsLoading(false);
            router.push("/dashboard"); // Redirection après l'authentification
        } catch (error) {
            console.error('Token decode failed:', error);
            logout();
        }
    };

    const logout = () => {
        Cookies.remove('access_token');
        Cookies.remove('refresh_token');
        if (typeof window !== 'undefined') {
            sessionStorage.removeItem('authenticated_user');
        }
        setAuthenticatedUser(null);
        setIsLoading(false);
        router.push("/sign-in");
    };

    const getAccessToken = () => Cookies.get('access_token') || null;

    if (!isMounted) {
        return null;
    }

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