/**
 * auth_assets
 * 权限配置
 */
export interface Auth_assets_Class {
  /** Id */
  Id: number;
  /** 创建账号 */
  create_account: string | null;
  /** 创建人姓名 */
  create_username: string | null;
  /** 创建时间 */
  create_time: number | null;
  /** 备注 */
  remark: string | null;
  /** 分类(g,p) */
  Section: string | null;
  /** 类型(g,g2,g3,p,p2,p3) */
  Type: string | null;
  /** 账号或角色编码 */
  Value1: string | null;
  /** 所拥有的角色编码或租户 */
  Value2: string | null;
  /** 租户或资源类别（Menu，Controller） */
  Value3: string | null;
  /** 特征值 */
  Value4: string | null;
  /** 特征值 */
  Value5: string | null;
  /** 特征值 */
  Value6: string | null;
  /** 特征值 */
  Value7: string | null;
  /** 特征值 */
  Value8: string | null;
  /** 特征值 */
  Value9: string | null;
  /** 特征值 */
  Value10: string | null;
  /** 特征值 */
  Value11: string | null;
  /** 特征值 */
  Value12: string | null;
}
/**
 * auth_assets检索类
 * 权限配置
 */
export interface Auth_assets_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 创建账号 */
  create_account: string | null;
}
