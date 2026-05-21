import { defineConfig } from 'vite';
import { resolve } from 'path';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  envDir: 'env',
  resolve: {
    alias: {
      '@utility': '/src/assets/utility',
      '@common': '/src/components',
      '@request': '/src/request',
      '@entities': '/src/types/tableTypes',
    },
  },
  build: {
    rollupOptions: {
      input: {
        main: resolve(__dirname, 'index.html'),
        admin: resolve(__dirname, 'admin.html'),
        about: resolve(__dirname, 'about.html'),
        error: resolve(__dirname, 'error.html'),
        login: resolve(__dirname, 'login.html'),
        system: resolve(__dirname, 'system.html'),
      },
    },
    outDir: '../Backend_cs/wwwroot/PTools_PSilo/',
  },
  server: {
    cors: true,
    proxy: {
      '/PTools_PSilo/MessageHub': {
        target: 'http://localhost:6140', // 后端SignalR服务地址
        changeOrigin: true, // 修改请求头中的Origin字段为目标地址
        ws: true, // 处理WebSocket请求
      },
    },
  },
});

