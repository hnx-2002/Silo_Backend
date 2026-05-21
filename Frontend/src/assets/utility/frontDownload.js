/**
 * 前端根据地址下载
 * lhy added 20220710
 *
 * @export
 * @param {*} filePath
 * @param {*} fileName
 */
export function frontDownload(filePath, fileName) {
    var a = document.createElement('a'); //创建一个<a></a>标签
    a.href = filePath; // 给a标签的href属性值加上地址，注意，这里是绝对路径，不用加 点.
    a.download = fileName; //设置下载文件文件名，这里加上.xlsx指定文件类型，pdf文件就指定.fpd即可
    a.style.display = 'none'; // 障眼法藏起来a标签
    document.body.appendChild(a); // 将a标签追加到文档对象中
    a.click(); // 模拟点击了a标签，会触发a标签的href的读取，浏览器就会自动下载了
    a.remove(); // 一次性的，用完就删除a标签
}

/**
 *  已获得二进制返回值，进行下载
 *
 * @export
 * @param {*} downloadRes
 */
export function downloadByBinary(downloadRes, fileName) {
    let fileSrc = window.URL.createObjectURL(downloadRes);
    frontDownload(fileSrc, fileName)
    window.URL.revokeObjectURL(fileSrc);
}

/**
 *  已获得base64文本，进行下载
 *
 * @export
 * @param {*} base64str
 */
export function downloadByBase64(base64str, fileName) {
    let downloadRes = base64ToBlob(base64str);
    downloadByBinary(downloadRes, fileName);
}

/**
 * base64文本转文件流
 *
 * @export
 * @param {*} base64str
 * @return {*}
 */
export function base64ToBlob(base64str) {
    const decodedData = atob(base64str);
    const uInt8Array = new Uint8Array(decodedData.length);
    for (let i = 0; i < decodedData.length; ++i) {
        uInt8Array[i] = decodedData.charCodeAt(i);
    }
    return new Blob([uInt8Array], { type: 'application/octet-stream' });
}
