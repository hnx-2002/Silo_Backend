var TrimTool = {
    //去除字符串左空格
    ltrim: function (s) {
        return s.replace(/^(\s*| *)/, '');
    },
    //去除字符串右空格
    rtrim: function (s) {
        return s.replace(/(\s*| *)$/, '');
    },
    //去除字符串左右空格
    trim: function (s) {
        return this.ltrim(this.rtrim(s));
    },
    //去除字符串中所有空格
    trim_all: function (text) {
        text = text.replace(/^\s+/, '');
        for (var i = text.length - 1; i >= 0; i--) {
            if (/\S/.test(text.charAt(i))) {
                text = text.substring(0, i + 1);
                break;
            }
        }
        return text;
    },
};
export default TrimTool;
