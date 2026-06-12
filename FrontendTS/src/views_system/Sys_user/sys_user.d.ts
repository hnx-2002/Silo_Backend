/**
 * sys_user
 * 人员管理
 */
export interface Sys_user_Class {
  /** 主键Id */
  id: number;
  /** 人员Id */
  userId: string;
  /** 租户编码 */
  tenant: string | null;
  /** 账号 */
  account: string;
  /** 姓名 */
  userName: string;
  /** 密码 */
  password: string | null;
  /** 状态(10冻结，11正常) */
  status: number | null;
  /** 性别(10女，11男) */
  sex: number | null;
  /** 公司 */
  company: string | null;
  /** 手机号 */
  phone: string | null;
  /** 头像 */
  pic_text: string | null;
  /** 审核状态(00待处理，10不通过，11通过) */
  process_status: number | null;
  /** 申请时间 */
  apply_time: number | null;
  /** 处理人 */
  handle_account: string | null;
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
 * sys_user检索类
 * 人员管理
 */
export interface Sys_user_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 人员Id */
  userId: string;
  /** 租户编码 */
  tenant: string | null;
  /** 账号 */
  account: string;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
