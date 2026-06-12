import type { Dict_silo_Class } from '@entities/dict_silo.d.ts';

export function CreateDefault_dict_silo(): Dict_silo_Class {
  return {
    id: '00000000-0000-0000-0000-000000000000',
    sort: 0,
    silo_type: '',
    silo_diameter: '',
    silo_name: '',
    diameter_val: 0,
    using_type: '',
    projects: '',
    create_account: '',
    create_username: '',
    create_time: Date.now(),
    update_account: '',
    update_username: '',
    update_time: Date.now(),
    remark: '',
  };
}
