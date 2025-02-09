"use client"

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import {useAppContext} from "@/contexts/app-context";

export default function RootPage() {
    const router = useRouter();
    const { isAuthenticated } = useAppContext();

    useEffect(() => {
        if (!isAuthenticated) {
            router.replace('/sign-in');
        }else {
            router.replace('/dashboard');
        }
    }, []);

    return null;
}