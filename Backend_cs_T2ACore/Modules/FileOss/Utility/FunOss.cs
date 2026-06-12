using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public class FunOss
{
    /// <summary>
    /// 字符串MD5加密
    /// </summary>
    /// <param name="inStream"></param>
    /// <returns></returns>
    public static string GetFileMD5(Stream inStream)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        var result = md5.ComputeHash(inStream);
        var strResult = BitConverter.ToString(result);
        return strResult.Replace("-", "");
    }

    /// <summary>
    /// 计算字符串ETag的MD5
    /// </summary>
    /// <param name="etag"></param>
    /// <returns></returns>
    public static string GetETagMD5(string etag)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(etag);
        var result = md5.ComputeHash(inputBytes);
        var strResult = BitConverter.ToString(result);
        return strResult.Replace("-", "");
    }

    /// <summary>
    /// 获取文件头信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetContentType(string fileName)
    {
        if (fileName.Contains(".jpg"))
        {
            return "image/jpg";
        }
        else if (fileName.Contains(".jpeg"))
        {
            return "image/jpeg";
        }
        else if (fileName.Contains(".png"))
        {
            return "image/png";
        }
        else if (fileName.Contains(".gif"))
        {
            return "image/gif";
        }
        else if (fileName.Contains(".pdf"))
        {
            return "application/pdf";
        }
        else
        {
            return "application/octet-stream";
        }
    }
}
