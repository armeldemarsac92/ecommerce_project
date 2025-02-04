'use client';

import {useState} from "react";
import {AuthContext, IAuthData} from "@/contexts/auth-context";

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [authData, setAuthData] = useState<IAuthData | undefined>(undefined);
    const [contextLoading, setContextLoading] = useState(false);

    const updateAuthData = (data: IAuthData) => {
        setAuthData(prevAuthData => ({ ...prevAuthData, ...data }));
    };

    const updateContextLoading = (isLoading: boolean) => {
        setContextLoading(isLoading);
    }

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