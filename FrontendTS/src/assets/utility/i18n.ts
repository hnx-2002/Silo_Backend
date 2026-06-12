// 导入 Vue I18n 的 createI18n 方法，用于创建国际化实例
import { createI18n } from 'vue-i18n';
import type { Composer } from 'vue-i18n';
import type { LanDict } from '../../types/LanDict.ts';

// 创建 i18n 实例，配置国际化相关参数
const i18n = createI18n<LanDict, keyof LanDict>({
  // 使用 Composition API 模式，而非传统 Options API 模式
  legacy: false,
  // 设置默认语言为中文
  locale: 'zh',
  // 设置回退语言，当当前语言缺少翻译时使用中文
  fallbackLocale: 'zh',
  // 定义各语言的翻译消息对象
  messages: {
    zh: {},
    en: {},
    ru: {},
    de: {},
    es: {},
    pt: {},
  },
  // 关闭缺失翻译时的警告信息
  missingWarn: false,
  // 关闭回退语言时的警告信息
  fallbackWarn: false,
});

// 导出全局翻译对象，用于在组件中进行翻译操作
const translate = i18n.global as unknown as Composer<LanDict, {}, {}, keyof LanDict>;

// 导出 i18n 实例和翻译对象，供其他模块使用
export { i18n, translate };
