import { getLanguages } from '@request/Language';
import { i18n } from './i18n';

/**
 * 初始化字典
 *
 * @return {*}
 */
function initDictionary() {
    getLanguages().then((x) => {
        i18n.global.setLocaleMessage('zh', x.zh);
        i18n.global.setLocaleMessage('en', x.en);
        i18n.global.setLocaleMessage('de', x.de);
        i18n.global.setLocaleMessage('ru', x.ru);
    });
}

/**
 * 初始化登录页字典
 *
 */
function initLoginDictionary() {
    i18n.global.setLocaleMessage('zh', {
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
    i18n.global.setLocaleMessage('de', {
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
    i18n.global.setLocaleMessage('en', {
        欢迎来到: 'Welcome to ',
        登录: 'Login',
        登录中: 'Login...',
        登录失败: 'Login failed',
        请输入: 'Please input',
        账号禁用: 'Account banned',
        验证码错误: 'Captcha error',
        验证失败: 'Verification failed',
        租户: 'Tenant',
        账号: 'Account',
        密码: 'Password',
        验证码: 'Captcha',
        本系统由天津水泥工业设计研究院有限公司数字化研究所研发和运维:
            'This platform is developed and maintained by TCDRI Digital Research Institute.',
    });
    i18n.global.setLocaleMessage('ru', {
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
}

export default {
    initDictionary,
    initLoginDictionary, 
};
