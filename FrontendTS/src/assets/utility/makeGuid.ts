/**
 * 生成带有时间戳的 GUID
 * 格式: CE{YYYY}{MM}-{DD}{HH}-{MM}{SS}-{FFF}A-{S4}{S4}{S4}
 * 示例: CE20260109-1430-2512-345A-1A2B3C4D5E6F
 */
export function makeTPGuid(): string {
  /**
   * 生成 4 位十六进制随机数
   */
  function S4(): string {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1).toUpperCase();
  }

  let now = new Date();
  let yyyy = now.getFullYear().toString();
  let MM = (now.getMonth() + 1).toString().padStart(2, '0');
  let dd = now.getDate().toString().padStart(2, '0');
  let HH = now.getHours().toString().padStart(2, '0');
  let mm = now.getMinutes().toString().padStart(2, '0');
  let ss = now.getSeconds().toString().padStart(2, '0');
  let fff = now.getMilliseconds().toString().padStart(3, '0');

  return (
    'CE' + yyyy + MM + '-' + dd + HH + '-' + mm + ss + '-' + fff + 'A' + '-' + S4() + S4() + S4()
  );
}
