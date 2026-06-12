import type { Task_result_Class } from '@entities/task_result.d.ts';

export function CreateDefault_task_result(): Task_result_Class {
  return {
    id: '00000000-0000-0000-0000-000000000000',
    task_base_id: '00000000-0000-0000-0000-000000000000',
    sort: 0,
    layout_title: '',
    rfa_resource_id: '00000000-0000-0000-0000-000000000000',
    layout_type: '',
    location_x: 0,
    location_y: 0,
    location_z: 0,
    normal_x: 0,
    normal_y: 0,
    normal_z: 0,
    rotate_angle: 0,
    create_account: '',
    create_username: '',
    create_time: Date.now(),
    update_account: '',
    update_username: '',
    update_time: Date.now(),
    remark: '',
  };
}
