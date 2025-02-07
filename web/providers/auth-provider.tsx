'use client';

import {useEffect, useState} from "react";
import {AuthContext, IAuthData} from "@/contexts/auth-context";
import {useAppContext} from "@/contexts/app-context";
import {useRouter} from "next/navigation";

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    const [authData, setAuthData] = useState<IAuthData | undefined>(undefined);
    const [contextLoading, setContextLoading] = useState(false);
    const {isAuthenticated} = useAppContext();

    const updateAuthData = (data: IAuthData) => {
        setAuthData(prevAuthData => ({ ...prevAuthData, ...data }));
    };

    const updateContextLoading = (isLoading: boolean) => {
        setContextLoading(isLoading);
    }


    if(isAuthenticated) {
        router.replace("/dashboard")
    }else {
        return (
            <AuthContext.Provider
                value={{
                    authData,
                    contextLoading,
                    updateContextLoading,
                    updateAuthData
                }}
            >
                {children}
            </AuthContext.Provider>
        );
    }
}