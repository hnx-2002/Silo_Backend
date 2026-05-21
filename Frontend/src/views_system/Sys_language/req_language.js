import request from '@utility/request';

export function GetAllLanguages(key) {
    return request({
        url: `/T2ACore/Language/GetAllLanguages?zh=` + key,
        method: 'get',
    });
}

/**
 * 根据主键获取实体
 */
export function GetOne(key) {
    return request({
        method: 'get',
        url: '/T2ACore/Language/Get/' + key,
    });
}

/**
 * 添加一个实体
 * @param {Object} inData [实体]
 */
export function Add(inData) {
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
export function Update(inData) {
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
export function Delete(keys) {
    return request({
        method: 'delete',
        url: '/T2ACore/Language/Delete',
        data: JSON.stringify(keys),
    });
}
