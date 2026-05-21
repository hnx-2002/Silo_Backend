<script setup>
import { onMounted } from 'vue';
import LoginPage from '@common/login/LoginForm.vue';
import Theme from '@common/theme/theme.vue';
import { translate } from '@utility/i18n.js';

const NAMESPACE = import.meta.env.VITE_NAMESPACE;

onMounted(() => {});
</script>

<template>
    <Theme>
        <LoginPage :welcomeTitle="translate.t('欢迎来到') + translate.t(NAMESPACE)"></LoginPage>
    </Theme>
</template>

<style>
#app {
    width: 100%;
    height: 100%;
    margin: 0;
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
}
</style>
