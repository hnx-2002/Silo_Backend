import request from '@utility/request';

export function MultiPagedSearch(search) {
    return request({
        url: `/T2ACore/Oss/MultiPagedSearch`,
        method: 'post',
        data: search,
    });
}

/**
 * 根据主键获取实体
 */
export function GetOne(id) {
    return request({
        method: 'get',
        url: '/T2ACore/Oss/Get/' + id,
    });
}
