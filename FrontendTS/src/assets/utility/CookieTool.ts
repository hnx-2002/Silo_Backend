/**
 * Cookie工具类，用于管理Cookie的设置、获取和删除操作
 */
var CookieTool = {
  /**
   * 设置Cookie
   * @param cname - Cookie名称
   * @param cvalue - Cookie值
   * @param exHours - 过期时间（小时），默认为2小时
   * @param path - Cookie路径，默认为'/'
   */
  setCookie: function (cname: string, cvalue: string, exHours?: number, path?: string) {
    var d = new Date();
    d.setTime(d.getTime() + (exHours || 2) * 60 * 60 * 1000);
    var expires = 'expires=' + d.toUTCString();
    var cookiePath = path || '/';
    var cookieString = cname + '=' + cvalue + '; ' + expires + '; path=' + cookiePath;
    document.cookie = cookieString;
  },
  /**
   * 获取Cookie值
   * @param cname - Cookie名称
   * @returns Cookie值，如果不存在则返回空字符串
   */
  getCookie: function (cname: string) {
    var name = cname + '=';
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i]?.trim();
      if (c && c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return '';
  },
  /**
   * 删除Cookie
   * @param cname - Cookie名称
   * @param path - Cookie路径，默认为'/'
   */
  deleteCookie: function (cname: string, path?: string) {
    var d = new Date();
    d.setTime(d.getTime() - 1000);
    var expires = 'expires=' + d.toUTCString();
    var cookiePath = path || '/';
    var cookieString = cname + '=; ' + expires + '; path=' + cookiePath;
    document.cookie = cookieString;
  },
};
export default CookieTool;
