import request from '@utility/request';
import type { File_record_Search } from './file_record.ts';

const NAMESPACE = import.meta.env.VITE_NAMESPACE;

export function MultiPagedSearch(search: File_record_Search) {
  return request({
    url: '/' + NAMESPACE + '/Oss/MultiPagedSearch',
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
    url: '/' + NAMESPACE + '/Oss/Get/' + id,
  });
}
