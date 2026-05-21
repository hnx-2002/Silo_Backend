/**
 * sys_tenant
 * 租户管理
 */
export interface Sys_tenant_Class {
  /** Id */
  id: string;
  /** 编码 */
  code: string | null;
  /** 名称 */
  title: string | null;
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
 * sys_tenant检索类
 * 租户管理
 */
export interface Sys_tenant_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 编码 */
  code: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
