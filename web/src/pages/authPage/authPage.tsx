import React, { useState } from "react";
import api from "../../common/services/api/auth/api";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../common/services/api/auth/AuthContext";

const AuthPage: React.FC = () => {
    const { isAuthorized, setIsAuthorized, setUserId, setGuestId, loading } = useAuth();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [nickname, setNickname] = useState("");
    const [mode, setMode] = useState<"login" | "register">("login");
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const getRequestConfig = () => {
        const token = localStorage.getItem("accessToken");
        const config: any = { withCredentials: true };
        if (token) config.headers = { Authorization: `Bearer ${token}` };
        return config;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
            if (mode === "login") {
                const resp = await api.post(
                    "/users/login",
                    { email, password },
                    getRequestConfig()
                );

                const token = resp.data.token;
                const userId = resp.data.id; 

                if (token) {
                    localStorage.setItem("accessToken", token);
                }

                if (userId) {
                    setUserId(userId);
                    console.log("✅ Logged in, userId:", userId);
                }

                setIsAuthorized(true);
                setGuestId(null);
                localStorage.removeItem("guestId"); 

                setEmail("");
                setPassword("");
                navigate("/");

            } else {
                const resp = await api.post(
                    "/users/register",
                    { nickname, email, password },
                    getRequestConfig()
                );

                const token = resp.data.token;
                const userId = resp.data.id;

                if (token) {
                    localStorage.setItem("accessToken", token);
                }

                if (userId) {
                    setUserId(userId);
                    console.log("✅ Registered and logged in, userId:", userId);
                }

                setIsAuthorized(true);
                setGuestId(null);
                localStorage.removeItem("guestId");

                setEmail("");
                setPassword("");
                setNickname("");
                navigate("/");
            }
        } catch (err: any) {
            console.error("❌ Auth error:", err);
            setError(err.response?.data?.error || "Ошибка авторизации");
        }
    };


    const handleLogout = async () => {
        try {
            const resp = await api.post("/users/logout", {}, getRequestConfig());

            // ✅ Получаем новый guestId из ответа
            const newGuestId = resp.data?.guestId;

            if (newGuestId) {
                setGuestId(newGuestId);
                localStorage.setItem("guestId", newGuestId);
                console.log("✅ Logged out, new guestId:", newGuestId);
            }
        } catch (e) {
            console.error("❌ Logout error:", e);
        }

        localStorage.removeItem("accessToken");
        setIsAuthorized(false);
        setUserId(null);
        setError(null);
        navigate("/");
    };

    if (loading) {
        return (
            <div className="d-flex justify-content-center align-items-center vh-100">
                <div className="spinner-border" role="status">
                    <span className="visually-hidden">Загрузка...</span>
                </div>
            </div>
        );
    }

    if (isAuthorized) {
        return (
            <div className="d-flex justify-content-center align-items-center vh-100">
                <div className="card shadow p-4" style={{ maxWidth: "400px", width: "100%" }}>
                    <h4 className="text-center mb-4">Вы успешно авторизованы!</h4>
                    <button className="btn btn-danger w-100" onClick={handleLogout}>
                        Выйти
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
            <div className="card shadow p-4" style={{ maxWidth: "400px", width: "100%" }}>
                <h3 className="text-center mb-4">
                    {mode === "login" ? "Вход" : "Регистрация"}
                </h3>
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <input
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Email"
                            type="email"
                            className="form-control"
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Пароль"
                            className="form-control"
                            required
                        />
                    </div>
                    {mode === "register" && (
                        <div className="mb-3">
                            <input
                                value={nickname}
                                onChange={(e) => setNickname(e.target.value)}
                                placeholder="Ник"
                                type="text"
                                className="form-control"
                                required
                            />
                        </div>
                    )}
                    {error && <div className="alert alert-danger">{error}</div>}
                    <button type="submit" className="btn btn-primary w-100 mb-2">
                        {mode === "login" ? "Войти" : "Зарегистрироваться"}
                    </button>
                    <button
                        type="button"
                        className="btn btn-link w-100"
                        onClick={() => {
                            setMode(mode === "login" ? "register" : "login");
                            setError(null);
                        }}
                    >
                        {mode === "login"
                            ? "Нет аккаунта? Зарегистрироваться"
                            : "Уже есть аккаунт? Войти"}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default AuthPage;
