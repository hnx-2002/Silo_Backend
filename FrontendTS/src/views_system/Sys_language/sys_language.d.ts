/**
 * sys_language
 * 多语言
 */
export interface Sys_language_Class {
  /** 中文做Key */
  key: string;
  /** 中文 */
  zh: string;
  /** 英文 */
  en: string | null;
  /** 俄文 */
  ru: string | null;
  /** 德语 */
  de: string | null;
  /** 西班牙语 */
  es: string | null;
  /** 葡萄牙语 */
  pt: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 创建人姓名 */
  create_username: string | null;
  /** 创建时间 */
  create_time: number | null;
  /** 更新账号 */
  update_account: string | null;
  /** 更新人姓名 */
  update_username: string | null;
  /** 更新时间 */
  update_time: number | null;
  /** 备注 */
  remark: string | null;
}
/**
 * sys_language检索类
 * 多语言
 */
export interface Sys_language_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 中文做Key */
  key: string;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
