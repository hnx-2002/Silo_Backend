/**
 * 表单验证工具类
 */

/**
 * 数字字段必填验证规则
 * 只在值为 undefined、null 或空字符串时才报错
 * @param message 错误提示信息
 * @returns 验证规则对象
 */
export function createNumberRequiredRule(message: string) {
  return {
    required: true,
    trigger: ['input', 'blur'],
    validator: (rule: any, value: any) => {
      if (value === undefined || value === null || value === '') {
        return new Error(message);
      }
      return true;
    },
  };
}

/**
 * 字符串字段必填验证规则
 * 只在值为 undefined、null 或空字符串时才报错
 * @param message 错误提示信息
 * @returns 验证规则对象
 */
export function createStringRequiredRule(message: string) {
  return {
    required: true,
    message,
    trigger: ['input', 'blur'],
  };
}
