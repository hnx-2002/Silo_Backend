import request from '@utility/request.ts';
import type { Sys_language_Class } from './sys_language.d.ts';

/**
 * 获取所有语言配置
 */
export function GetAllLanguages(key: string) {
  return request({
    url: `/T2ACore/Language/GetAllLanguages?zh=` + key,
    method: 'get',
  });
}

/**
 * 根据主键获取实体
 */
export function GetOne(key: string) {
  return request({
    method: 'get',
    url: '/T2ACore/Language/Get/' + key,
  });
}

/**
 * 添加一个实体
 * @param {Object} inData [实体]
 */
export function Add(inData: Sys_language_Class) {
  return request({
    method: 'post',
    url: '/T2ACore/Language/Add',
    data: JSON.stringify(inData),
  });
}

/**
 * 修改一个实体
 * @param {Object} inData [实体]
 */
export function Update(inData: Sys_language_Class) {
  return request({
    method: 'put',
    url: '/T2ACore/Language/Update',
    data: JSON.stringify(inData),
  });
}

/**
 * 根据主键集合删除一批实体
 * @param {String} keys [key字符串集合]
 */
export function Delete(keys: string[]) {
  return request({
    method: 'delete',
    url: '/T2ACore/Language/Delete',
    data: JSON.stringify(keys),
  });
}
