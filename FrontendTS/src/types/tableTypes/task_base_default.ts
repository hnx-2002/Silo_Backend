import type { Task_base_Class } from '@entities/task_base.d.ts';

export function CreateDefault_task_base(): Task_base_Class {
  return {
    id: '00000000-0000-0000-0000-000000000000',
    task_title: '',
    dict_silo_id: '00000000-0000-0000-0000-000000000000',
    silo_diameter: 0,
    silo_height: 0,
    task_x: 0,
    task_y: 0,
    task_z: 0,
    rotation_angle: 0,
    status: 0,
    error_msg: '',
    create_account: '',
    create_username: '',
    create_time: Date.now(),
    update_account: '',
    update_username: '',
    update_time: Date.now(),
    remark: '',
  };
}
