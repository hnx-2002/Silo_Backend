const TrimTool = {
  /**
   * 去除字符串左边的空格
   * @param s - 要处理的字符串
   * @returns 去除左边空格后的字符串
   */
  ltrim(s: string): string {
    return s.replace(/^(\s*| *)/, '');
  },
  /**
   * 去除字符串右边的空格
   * @param s - 要处理的字符串
   * @returns 去除右边空格后的字符串
   */
  rtrim(s: string): string {
    return s.replace(/(\s*| *)$/, '');
  },
  /**
   * 去除字符串两边的空格
   * @param s - 要处理的字符串
   * @returns 去除两边空格后的字符串
   */
  trim(s: string): string {
    return this.ltrim(this.rtrim(s));
  },
  /**
   * 去除字符串所有空格
   * @param text - 要处理的字符串
   * @returns 去除所有空格后的字符串
   */
  trim_all(text: string): string {
    text = text.replace(/^\s+/, '');
    for (let i = text.length - 1; i >= 0; i--) {
      if (/\S/.test(text.charAt(i))) {
        text = text.substring(0, i + 1);
        break;
      }
    }
    return text;
  },
};
export default TrimTool;
