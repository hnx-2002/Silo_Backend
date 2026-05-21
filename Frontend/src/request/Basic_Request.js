import request from '@utility/request';
import { frontDownload } from '@utility/frontDownload.js';

const NAMESPACE = import.meta.env.VITE_NAMESPACE;

/**
 * 根据主键获取实体
 * @param {String} id [body传值]
 * @param {String} pattern [特征名]
 */
export function GetOne(id, pattern) {
    return request({
        method: 'get',
        url: '/' + NAMESPACE + '/' + pattern + '/Get/' + id,
    });
}

/**
 * 分页高级检索，全字段匹配
 * @param {Object} inData [查询实体]
 * @param {String} pattern [特征名]
 */
export function MultiPagedSearch(inData, pattern) {
    return request({
        method: 'post',
        url: '/' + NAMESPACE + '/' + pattern + '/MultiPagedSearch',
        data: JSON.stringify(inData),
    });
}

/**
 * 添加一个实体
 * @param {Object} inData [实体]
 * @param {String} pattern [特征名]
 */
export function Add(inData, pattern) {
    return request({
        method: 'post',
        url: '/' + NAMESPACE + '/' + pattern + '/Add',
        data: JSON.stringify(inData),
    });
}

/**
 * 修改一个实体
 * @param {Object} inData [实体]
 * @param {String} pattern [特征名]
 */
export function Update(inData, pattern) {
    return request({
        method: 'put',
        url: '/' + NAMESPACE + '/' + pattern + '/Update',
        data: JSON.stringify(inData),
    });
}

/**
 * 根据主键集合删除一批实体
 * @param {String} ids [id字符串集合]
 * @param {String} pattern [特征名]
 */
export function Delete(ids, pattern) {
    return request({
        method: 'delete',
        url: '/' + NAMESPACE + '/' + pattern + '/Delete',
        data: JSON.stringify(ids),
    });
}

/**
 * 获取选项
 *
 * @param {String} pattern [特征名]
 */
export function GetOptions(pattern) {
    return request({
        method: 'get',
        url: '/' + NAMESPACE + '/' + pattern + '/GetOptions',
    });
}

/**
 * 下载导入Excel的模板
 * @param {String} pattern [特征名]
 */
export function DownloadExcelTemplate(pattern) {
    let url =
        import.meta.env.VITE_API_URL + '/' + NAMESPACE + '/' + pattern + '/DownloadExcelTemplate';
    frontDownload(url, pattern + '导入模板.xlsx');
}

/**
 * 导入Excel
 * @param {String} pattern [特征名]
 * @param {Object} inData [导入文件信息]
 */
export function ImportExcelData(pattern, inData) {
    return request({
        method: 'post',
        url: '/' + NAMESPACE + '/' + pattern + '/ImportExcelData',
        data: inData,
    });
}
