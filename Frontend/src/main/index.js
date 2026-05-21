import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './index.vue';
import { i18n } from '/src/assets/utility/i18n.js';
import dict from '/src/assets/utility/localization.js';
import { overrideLocalStorageEvent } from '/src/assets/utility/localstorage.js';
import '/src/assets/theme/tcdri.scss';

dict.initDictionary();
overrideLocalStorageEvent(); //重写localStorage事件
var app = createApp(App);
app.use(i18n);
app.use(createPinia());
app.mount('#app');

