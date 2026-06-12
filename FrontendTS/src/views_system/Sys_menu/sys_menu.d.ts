/**
 * sys_menu
 * 菜单管理
 */
export interface Sys_menu_Class {
  /** Id */
  id: string;
  /** 父级Id */
  parent_id: string;
  /** 菜单编号 */
  code: string;
  /** 系统编号(工程bim,设计design) */
  sys_code: string | null;
  /** 菜单名 */
  label: string;
  /** 菜单别名 */
  alias: string | null;
  /** 地址栏路径 */
  path: string | null;
  /** 组件地址 */
  component: string | null;
  /** 重定向地址 */
  redirect: string | null;
  /** 图标地址 */
  icon: string | null;
  /** 是否固定(10否11是) */
  is_fixed: number;
  /** 是否缓存(10否11是) */
  is_keep_alive: number;
  /** 是否打开新页面(10否11是) */
  is_open_new: number;
  /** 是否隐藏注册(菜单栏中不可见)(10否11是) */
  is_hidden: number;
  /** 只有一级子路由，父路由是否显示在菜单中(10否11是) */
  is_always_show: number;
  /** 排序 */
  sort: number;
  /** 状态(10失效11正常) */
  status: number;
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
 * sys_menu检索类
 * 菜单管理
 */
export interface Sys_menu_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 父级Id */
  parent_id: string;
  /** 菜单编号 */
  code: string;
  /** 菜单名 */
  label: string;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
