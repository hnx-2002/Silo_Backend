import request from '@utility/request';
import type { File_record_Search } from './file_record.ts';

export function MultiPagedSearch(search: File_record_Search) {
  return request({
    url: `/T2ACore/Oss/MultiPagedSearch`,
    method: 'post',
    data: search,
  });
}

/**
 * 根据主键获取实体
 */
export function GetOne(id: string) {
  return request({
    method: 'get',
    url: '/T2ACore/Oss/Get/' + id,
  });
}
