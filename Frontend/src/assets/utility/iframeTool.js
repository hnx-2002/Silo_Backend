//
/**
 * 检查iframe的某个方法，
 *
 * @export
 * @param {*} iframeId iframe的dom的id
 * @param {*} methodName 方法名
 * @param {*} callback 回调，返回iframe的window
 * @return {*}
 */
export function checkIframeMethodOnLoad(iframeId, methodName, callback) {
    let intervalId = setInterval(checkMethod, 300);
function checkMethod()
{
    console.log('checking...');
    let iframe = document.getElementById(iframeId);

    if (iframe.contentWindow && iframe.contentWindow[methodName])
    {
        callback(iframe.contentWindow);
        clearInterval(intervalId);
    }
}
}
