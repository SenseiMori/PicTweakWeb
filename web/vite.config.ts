import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    build: {
        outDir: 'dist', 
    },
    server: {
        host: true,
        port: 3000,
        allowedHosts: ["localhost", "tweakypro.ru"],
        proxy: {
            '/api': {
                target: 'http://localhost:5022', 
                changeOrigin: true,
                secure: false,
            },
        },
        hmr: false,
    },
});