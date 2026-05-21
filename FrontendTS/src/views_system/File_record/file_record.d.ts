/**
 * file_record
 * 文件记录
 */
export interface File_record_Class {
  /** 主键Id */
  id: string;
  /** 父级Id */
  parent_id: string | null;
  /** 排序 */
  sort: number | null;
  /** 文件名 */
  file_name: string | null;
  /** 文件扩展名 */
  file_ext: string | null;
  /** 文件完整路径 */
  file_path: string | null;
  /** Bucket名称 */
  bucket_name: string | null;
  /** MD5编码 */
  md5: string | null;
  /** 文件长度 */
  bytes_length: number | null;
  /** Referer */
  referer: string | null;
  /** 上传的Ip地址 */
  upload_ip: string | null;
  /** 软删除(10否11是) */
  is_delete: number | null;
  /** 租户 */
  tenant: string | null;
  /** 创建人账号 */
  create_account: string | null;
  /** 创建人姓名 */
  create_username: string | null;
  /** 创建时间 */
  create_time: number | null;
  /** 备注 */
  remark: string | null;
}
/**
 * file_record检索类
 * 文件记录
 */
export interface File_record_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 父级Id */
  parent_id: string | null;
  /** 文件名 */
  file_name: string | null;
  /** 文件扩展名 */
  file_ext: string | null;
  /** Bucket名称 */
  bucket_name: string | null;
  /** 租户 */
  tenant: string | null;
  /** 创建人账号 */
  create_account: string | null;
}
