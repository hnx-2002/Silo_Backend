/**
 * template_silo
 * 库型模板
 */
export interface Template_silo_Class {
  /** Id */
  id: string;
  /** 库型名称 */
  silo_name: string | null;
  /** 族类型名 */
  symbol_name: string | null;
  /** 族文件地址 */
  rfa_path: string | null;
  /** 库底板高度 */
  silo_height: number | null;
  /** 模板基点x */
  template_x: number | null;
  /** 模板基点y */
  template_y: number | null;
  /** 模板基点z */
  template_z: number | null;
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
 * template_silo检索类
 * 库型模板
 */
export interface Template_silo_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 库型名称 */
  silo_name: string | null;
  /** 族类型名 */
  symbol_name: string | null;
  /** 族文件地址 */
  rfa_path: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
