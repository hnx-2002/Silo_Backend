import type { Rfa_resource_Class } from '@entities/rfa_resource.d.ts';

export function CreateDefault_rfa_resource(): Rfa_resource_Class {
  return {
    id: '00000000-0000-0000-0000-000000000000',
    dict_silo_id: '00000000-0000-0000-0000-000000000000',
    symbol_name: '',
    rfa_path: '',
    file_name: '',
    file_size: 0,
    note: '',
    template_x: 0,
    template_y: 0,
    template_z: 0,
    create_account: '',
    create_username: '',
    create_time: Date.now(),
    update_account: '',
    update_username: '',
    update_time: Date.now(),
    remark: '',
  };
}
