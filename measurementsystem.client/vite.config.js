import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import { env } from 'process';

const target = env.ASPNETCORE_HTTPS_PORT ? `http://localhost:${env.ASPNETCORE_HTTP_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:32768';

const wsTarget = env.ASPNETCORE_HTTPS_PORT ? `ws://localhost:${env.ASPNETCORE_HTTP_PORT}` :
    env.ASPNETCORE_URLS ? `ws://${env.ASPNETCORE_URLS.split(';')[0].replace('http://', '')}` : 'ws://localhost:32768';

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '/api': {
                target,
                secure: false
            },
            '/api/monitoring': {
                target: 'http://dbrobo1.mgul.ac.ru',
                secure: false,
                ws: true,
                changeOrigin: true,
            },
        },
        port: 5173
    }
})