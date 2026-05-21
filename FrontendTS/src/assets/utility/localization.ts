import { getLanguages } from '@request/Language.ts';
import { translate } from './i18n.ts';

/**
 * 本地化工具模块
 * 提供初始化多语言字典的功能，支持中文、英文、德文、俄文四种语言
 * 主要用于应用的国际化和本地化处理
 */

/**
 * 初始化字典
 * 从服务器获取语言包并设置到 i18n 实例中
 * 支持中文(zh)、英文(en)、德文(de)、俄文(ru)四种语言
 */
function initDictionary() {
  getLanguages().then((x) => {
    translate.setLocaleMessage('zh', x.zh);
    translate.setLocaleMessage('en', x.en);
    translate.setLocaleMessage('de', x.de);
    translate.setLocaleMessage('ru', x.ru);
  });
}

/**
 * 初始化登录页字典
 * 设置登录页面所需的本地化文本，包括欢迎语、登录按钮、输入框提示等
 * 支持中文(zh)、英文(en)、德文(de)、俄文(ru)四种语言
 */
function initLoginDictionary() {
  translate.setLocaleMessage('zh', {
    欢迎来到: '欢迎来到',
    登录: '登录',
    登录中: '登录中',
    登录失败: '登录失败',
    请输入: '请输入',
    账号禁用: '账号禁用',
    验证码错误: '验证码错误',
    验证失败: '验证失败',
    租户: '租户',
    账号: '账号',
    密码: '密码',
    验证码: '验证码',
    本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维:
      '本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维',
  });
  translate.setLocaleMessage('de', {
    欢迎来到: 'Willkommen bei',
    登录: 'Anmelden',
    登录中: 'Anmeldung...',
    登录失败: 'Anmeldung fehlgeschlagen',
    请输入: 'Bitte geben Sie ein',
    账号禁用: 'Konto gesperrt',
    验证码错误: 'Captcha-Fehler',
    验证失败: 'Verifizierung fehlgeschlagen',
    租户: 'Mandant',
    账号: 'Konto',
    密码: 'Passwort',
    验证码: 'Captcha',
    本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维:
      'Diese Plattform wird entwickelt und gewartet vom Digital Research Institute der Tianjin Cement Industry Design Research Institute Co., Ltd.',
  });
  translate.setLocaleMessage('en', {
    欢迎来到: 'Welcome to ',
    登录: 'Login',
    登录中: 'Login...',
    登录失败: 'Login failed',
    请输入: 'Please input',
    账号禁用: 'Account banned',
    验证码错误: 'Captcha error',
    验证失败: 'Verification failed',
    租户: 'Tenant',
    账号: '账号',
    密码: 'Password',
    验证码: 'Captcha',
    本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维:
      'This platform is developed and maintained by TCDRI Digital Research Institute.',
  });
  translate.setLocaleMessage('ru', {
    欢迎来到: 'Добро пожаловать на',
    登录: 'Войти',
    登录中: 'Вход...',
    登录失败: 'Вход не удался',
    请输入: 'Пожалуйста, введите',
    账号禁用: 'Аккаунт заблокирован',
    验证码错误: 'Ошибка капчи',
    验证失败: 'Ошибка проверки',
    租户: 'Тенант',
    账号: 'Аккаунт',
    密码: 'Пароль',
    验证码: 'Капча',
    本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维:
      'Эта платформа разработана и обслуживается Цифровым научно-исследовательским институтом TCDRI.',
  });
}

/**
 * 导出本地化工具对象
 * @property {Function} initDictionary - 初始化完整字典（从服务器获取）
 * @property {Function} initLoginDictionary - 初始化登录页字典（本地静态数据）
 */
export default {
  initDictionary,
  initLoginDictionary,
};
