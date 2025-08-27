import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    build: {
        outDir: 'dist', // билд будет в папку dist
    },
    server: {
        // Эти настройки нужны только для локальной разработки
        host: true,
        port: 3000,
        allowedHosts: ["localhost", "tweakypro.ru"],
        proxy: {
            '/api': {
                target: 'http://localhost:5022', // твой локальный backend
                changeOrigin: true,
                secure: false,
            },
        },
        // Для продакшн HMR отключаем
        hmr: false,
    },
});