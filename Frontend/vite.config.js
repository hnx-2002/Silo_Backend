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
                //dict_silo: resolve(__dirname + '/gen/pages/', 'dict_silo.html'),
                //rfa_resource: resolve(__dirname + '/gen/pages/', 'rfa_resource.html'),
                //task_base: resolve(__dirname + '/gen/pages/', 'task_base.html'),
                //task_result: resolve(__dirname + '/gen/pages/', 'task_result.html'),
                //template_silo: resolve(__dirname + '/gen/pages/', 'template_silo.html'),
            },
        },
        outDir: '../Backend_cs/wwwroot/PTools_PSilo/',
    },
    server: {
        cors: true,
    },
});
