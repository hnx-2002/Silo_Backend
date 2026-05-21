/**
 * task_result
 * 任务结果
 */
export interface Task_result_Class {
  /** Id */
  id: string;
  /** 任务Id */
  task_base_id: string;
  /** 排序 */
  sort: number | null;
  /** 布置标题 */
  layout_title: string | null;
  /** 族id */
  rfa_resource_id: string | null;
  /** 布置类型 */
  layout_type: string | null;
  /** 布置x坐标 */
  location_x: number | null;
  /** 布置y坐标 */
  location_y: number | null;
  /** 布置z坐标 */
  location_z: number | null;
  /** 旋转轴x方向 */
  normal_x: number | null;
  /** 旋转轴y方向 */
  normal_y: number | null;
  /** 旋转轴z方向 */
  normal_z: number | null;
  /** 旋转角度 */
  rotate_angle: number | null;
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
 * task_result检索类
 * 任务结果
 */
export interface Task_result_Search {
  /** 页码 */
  page: number;
  /** 单页数量 */
  pageSize: number;
  /** 任务Id */
  task_base_id: string;
  /** 布置标题 */
  layout_title: string | null;
  /** 族id */
  rfa_resource_id: string | null;
  /** 创建账号 */
  create_account: string | null;
  /** 更新账号 */
  update_account: string | null;
}
