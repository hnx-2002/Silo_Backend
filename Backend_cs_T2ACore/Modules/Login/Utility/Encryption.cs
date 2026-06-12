using System;
using System.Text;

namespace T2ACore;

/// <summary>
/// 加解密类
/// </summary>
public class Encryption
{
    /// <summary>
    /// 字符串MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(string str)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        var strResult = BitConverter.ToString(result);
        return strResult.Replace("-", "");
    }

}
