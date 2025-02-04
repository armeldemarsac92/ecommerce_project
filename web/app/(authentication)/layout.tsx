import {AuthProvider} from "@/providers/auth-provider";

export default function AuthenticationLayout({ children }: { children: React.ReactNode }) {
    return (
        <AuthProvider>
            {children}
        </AuthProvider>
    )
}
