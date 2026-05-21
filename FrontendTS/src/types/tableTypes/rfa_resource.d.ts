/**
 * rfa_resource
 * 族资源
 */
export interface Rfa_resource_Class {
  /** 主键Id */
  id: string;
  /** 族编码 */
  rfa_code: string | null;
  /** 族类型名 */
  symbol_name: string | null;
  /** 族文件地址 */
  rfa_path: string | null;
  /** 族文件名称 */
  file_name: string | null;
  /** 族文件大小 */
  file_size: number | null;
  /** 说明 */
  note: string | null;
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
 * rfa_resource检索类
 * 族资源
 */
export interface Rfa_resource_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 族编码 */
  rfa_code: string | null;
  /** 族类型名 */
  symbol_name: string | null;
  /** 族文件地址 */
  rfa_path: string | null;
  /** 族文件名称 */
  file_name: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
