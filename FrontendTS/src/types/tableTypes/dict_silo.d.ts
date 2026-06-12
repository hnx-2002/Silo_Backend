/**
 * dict_silo
 * 库型
 */
export interface Dict_silo_Class {
  /** 主键Id */
  id: string;
  /** 排序 */
  sort: number | null;
  /** 库型 */
  silo_type: string | null;
  /** 直径 */
  silo_diameter: string | null;
  /** 库型名称 */
  silo_name: string | null;
  /** 库体直径 */
  diameter_val: number | null;
  /** 使用类别 */
  using_type: string | null;
  /** 参考项目 */
  projects: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 创建人 */
  create_username: string | null;
  /** 创建时间 */
  create_time: number | null;
  /** 更新账号 */
  update_account: string | null;
  /** 更新人 */
  update_username: string | null;
  /** 更新时间 */
  update_time: number | null;
  /** 备注 */
  remark: string | null;
}
/**
 * dict_silo检索类
 * 库型
 */
export interface Dict_silo_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 库型 */
  silo_type: string | null;
  /** 直径 */
  silo_diameter: string | null;
  /** 库型名称 */
  silo_name: string | null;
  /** 使用类别 */
  using_type: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
