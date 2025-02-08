'use client';

import { useEffect, useState } from 'react';
import { useAppContext } from "@/contexts/app-context";
import Loading from "@/app/loading";

export function AuthGuard({ children }: { children: React.ReactNode }) {
    const [isMounted, setIsMounted] = useState(false);
    const { authenticated_user, isLoading } = useAppContext();

    useEffect(() => {
        setIsMounted(true);
    }, []);

    // Ne rien afficher pendant le premier rendu côté serveur
    if (!isMounted) {
        return <Loading />;
    }

    // Afficher le loader pendant la vérification d'auth
    if (isLoading) {
        return <Loading />;
    }

    return <>{children}</>;
}