import { createI18n } from 'vue-i18n';

const i18n = createI18n({
    legacy: false,
    locale: 'zh',
    fallbackLocale: 'zh',
    messages: {},
    allowComposition: true, // 启用 Composition API 支持（Vue 3 需要）
    missingWarn: false, // 禁用未找到 key 的警告
    fallbackWarn: false, // 禁用回退翻译的警告
});

const translate = i18n.global;

export { i18n, translate };