/**
 * 适配后端 LanDict 类的语言字典类型
 * 后端字段是大写（ZH, EN, RU, DE, ES, PT），HTTP 返回到前端自动转换为小写
 */
export interface LanDict {
  /**
   * 中文
   */
  zh: Record<string, string>;
  /**
   * 德文
   */
  de: Record<string, string>;
  /**
   * 俄文
   */
  ru: Record<string, string>;
  /**
   * 英文
   */
  en: Record<string, string>;
  /**
   * 西班牙语
   */
  es: Record<string, string>;
  /**
   * 葡萄牙语
   */
  pt: Record<string, string>;
}
