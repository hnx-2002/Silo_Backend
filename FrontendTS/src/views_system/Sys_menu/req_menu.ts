import request from '@utility/request';
import type { Sys_menu_Class, Sys_menu_Search } from './sys_menu.ts';

export function getMenus(search: Sys_menu_Search) {
  return request({
    url: `/T2ACore/Menu/GetMenus`,
    method: 'post',
    data: search,
  });
}

/**
 * 根据主键获取实体
 * @param {String} id [body传值]
 */
export function getOne(id: string) {
  return request({
    method: 'get',
    url: '/T2ACore/Menu/Get/' + id,
  });
}

/**
 * 添加一个实体
 * @param {Sys_menu_Class} inData [实体]
 */
export function Add(inData: Sys_menu_Class) {
  return request({
    method: 'post',
    url: '/T2ACore/Menu/Add',
    data: JSON.stringify(inData),
  });
}

/**
 * 修改一个实体
 * @param {Sys_menu_Class} inData [实体]
 */
export function Update(inData: Sys_menu_Class) {
  return request({
    method: 'put',
    url: '/T2ACore/Menu/Update',
    data: JSON.stringify(inData),
  });
}

/**
 * 根据主键集合删除一批实体
 * @param {String[]} ids [id字符串集合]
 */
export function delOne(ids: string[]) {
  return request({
    method: 'delete',
    url: '/T2ACore/Menu/Delete',
    data: JSON.stringify(ids),
  });
}
