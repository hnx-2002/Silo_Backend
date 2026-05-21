var CookieTool = {
    setCookie: function (cname, cvalue, exHours, path) {
        var d = new Date();
        d.setTime(d.getTime() + (exHours || 2) * 60 * 60 * 1000);
        var expires = 'expires=' + d.toGMTString();
        // 如果没有指定path，则默认为'/'
        var path = path || '/';
        var cookieString = cname + '=' + cvalue + '; ' + expires + '; path=' + path;
        document.cookie = cookieString;
    },
    getCookie: function (cname) {
        var name = cname + '=';
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return '';
    },
    deleteCookie: function (cname, path) {
        // 删除cookie的方法
        var d = new Date();
        d.setTime(d.getTime() - 1000); // 设置为过去的时间，即立即过期
        var expires = 'expires=' + d.toGMTString();
        var path = path || '/'; // 如果没有指定路径，则使用根路径
        var cookieString = cname + '=; ' + expires + '; path=' + path;
        document.cookie = cookieString;
    },
};
export default CookieTool;
