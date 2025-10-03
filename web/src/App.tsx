import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.min.js';

import "./App.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Routes, Route } from "react-router-dom";
import AuthPage from "./pages/authPage/authPage";
import ModifierPage from './pages/modifierPage/modifier.page';
import MainLayout from './common/main.layout';
import { AuthProvider } from "./common/services/api/auth/AuthContext";
{/* import { AdminPage } from "./pages/adminPage/adminPage";*/ }


const queryClient = new QueryClient();

function App() {
    return (
            <AuthProvider>
                <QueryClientProvider client={queryClient}>
                    <Routes>
                            <Route path="/" element={<ModifierPage />}>
                            <Route path="auth" element={<AuthPage />} />
                            <Route path="modifier" element={<ModifierPage />} />
                        {/* <Route path="/admin/logs" element={<AdminPage />} />*/}

                        </Route>
                    </Routes>
                </QueryClientProvider>
            </AuthProvider>
    );
}

export default App;
