/**
 * 检查iframe的某个方法，
 *
 * @export
 * @param iframeId iframe的dom的id
 * @param methodName 方法名
 * @param callback 回调，返回iframe的window
 */
export function checkIframeMethodOnLoad(
  iframeId: string,
  methodName: string,
  callback: (window: Window) => void
) {
  let intervalId = setInterval(checkMethod, 300);
  function checkMethod() {
    console.log('checking...');
    let iframe = document.getElementById(iframeId) as HTMLIFrameElement;

    // if (iframe.contentWindow && iframe.contentWindow[methodName]) {
    if (iframe.contentWindow && methodName in iframe.contentWindow) {
      callback(iframe.contentWindow);
      clearInterval(intervalId);
    }
  }
}
