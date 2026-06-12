import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './about.vue';
import { i18n } from '@utility/i18n';
import dict from '@utility/localization.ts';
import { overrideLocalStorageEvent } from '@utility/localstorage.ts';
import '../assets/theme/tcdri.scss';

dict.initDictionary();
overrideLocalStorageEvent();
var app = createApp(App);
app.use(i18n);
app.use(createPinia());
app.mount('#app');
