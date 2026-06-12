/**
 * auth_role
 * 角色定义
 */
export interface Auth_role_Class {
  /** Id */
  id: string;
  /** 父级节点 */
  parent_id: string;
  /** 排序 */
  sort: number | null;
  /** 角色编码 */
  role_code: string | null;
  /** 角色租户编码 */
  role_tenant_code: string | null;
  /** 角色名称 */
  role_title: string | null;
  /** 角色维度(g,g2,g3) */
  region: string | null;
  /** 维度名称(职位，岗位，工作组，区域等) */
  region_title: string | null;
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
 * auth_role检索类
 * 角色定义
 */
export interface Auth_role_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 父级节点 */
  parent_id: string;
  /** 角色编码 */
  role_code: string | null;
  /** 角色租户编码 */
  role_tenant_code: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
