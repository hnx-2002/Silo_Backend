using System;

namespace T2ACore;

/// <summary>
/// 字符串扩展 加密解密相关
/// </summary>
public static class LoginStringExtension
{
    #region 加密解密

    /// <summary>
    /// 将字符串MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(this string str)
    {
        return Encryption.MD5(string.IsNullOrEmpty(str) ? "" : str.Trim());
    }

    #endregion

    /// <summary>
    /// 忽略大小写，判定两个字符串是否相等
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string str1, string str2)
    {
        return null == str1
               ? null == str2
               : str1.Equals(str2, StringComparison.CurrentCultureIgnoreCase);
    }


}
