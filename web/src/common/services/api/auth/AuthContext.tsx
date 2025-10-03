//common/services/api/auth/AuthContext.tsx
import React, { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import api from "./api";

interface AuthContextType {
    isAuthorized: boolean;
    setIsAuthorized: (authorized: boolean) => void;
    userId: string | null; 
    setUserId: (id: string | null) => void; 
    guestId: string | null; 
    setGuestId: (id: string | null) => void; 
    loading: boolean; 
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
    return ctx;
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [isAuthorized, setIsAuthorized] = useState<boolean>(false);
    const [userId, setUserId] = useState<string | null>(null);
    const [guestId, setGuestId] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    const getRequestConfig = () => {
        const token = localStorage.getItem("accessToken");
        const config: any = { withCredentials: true };
        if (token) config.headers = { Authorization: `Bearer ${token}` };
        return config;
    };

    useEffect(() => {
        let mounted = true;

        const checkAuthorization = async () => {
            try {
                setLoading(true);
                const resp = await api.get("/users/isAuthorize", getRequestConfig());
                const data = resp.data;

                if (!mounted) return;

                if (data?.isAuthorized) {
                    setIsAuthorized(true);
                    setUserId(data.id || null);
                    setGuestId(null);
                    console.log("✅ Авторизован, userId:", data.id);
                } else {
                    setIsAuthorized(false);
                    setUserId(null);

                    if (data?.guestId) {
                        setGuestId(data.guestId);
                        localStorage.setItem("guestId", data.guestId);
                        console.log("✅ Гость, guestId:", data.guestId);
                    }
                }
            } catch (err) {
                console.error("❌ isAuthorize check failed", err);
                setIsAuthorized(false);
                setUserId(null);
                setGuestId(null);
            } finally {
                if (mounted) setLoading(false);
            }
        };

        checkAuthorization();
        return () => { mounted = false; };
    }, []);

    return (
        <AuthContext.Provider value={{
            isAuthorized,
            setIsAuthorized,
            userId,
            setUserId,
            guestId,
            setGuestId,
            loading
        }}>
            {children}
        </AuthContext.Provider>
    );
};


