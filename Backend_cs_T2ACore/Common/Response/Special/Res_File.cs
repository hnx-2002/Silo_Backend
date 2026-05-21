using System;
using System.IO;

namespace T2ACore;

/// <summary>
/// 文件返回值
/// </summary>
public class Res_File<T>
{
    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件流
    /// </summary>
    public string FileBase64 { get; set; }

    /// <summary>
    /// 额外信息
    /// </summary>
    public T ExInfo { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileStream"></param>
    /// <param name="exInfo"></param>
    /// <returns></returns>
    public static Res_File<T> New(string fileName, MemoryStream fileStream, T exInfo)
    {
        var res = new Res_File<T>();
        res.Status = true;
        res.Message = "OK";
        res.FileName = fileName;
        res.FileBase64 = Convert.ToBase64String(fileStream.ToArray());
        res.ExInfo = exInfo;
        return res;
    }

    /// <summary>
    /// 失败的构造函数
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="url"></param>
    /// <param name="exInfo"></param>
    /// <returns></returns>
    public static Res_File<T> Bad(string msg, string url, T exInfo)
    {
        var res = new Res_File<T>();
        res.Status = false;
        res.Message = msg;
        res.FileName = "";
        res.FileBase64 = null;
        res.ExInfo = exInfo;
        return res;
    }

}
