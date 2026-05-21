/**
 * task_base
 * 建模任务
 */
export interface Task_base_Class {
  /** Id */
  id: string;
  /** 任务标题 */
  task_title: string | null;
  /** 库型 */
  silo_type: string | null;
  /** 储库直径 */
  silo_diameter: number | null;
  /** 库底板高度 */
  silo_height: number | null;
  /** 项目基点x */
  task_x: number | null;
  /** 项目基点y */
  task_y: number | null;
  /** 项目基点z */
  task_z: number | null;
  /** 旋转角度 */
  rotation_angle: number | null;
  /** 状态 */
  status: number | null;
  /** 错误信息 */
  error_msg: string | null;
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
 * task_base检索类
 * 建模任务
 */
export interface Task_base_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 任务标题 */
  task_title: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
