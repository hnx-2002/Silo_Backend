using System;

namespace T2ACore;

/// <summary>
/// 文件操作日志基类
/// </summary>
public class FileLog
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string FileType { get; set; }

    /// <summary>
    /// 桶名称
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 请求密钥
    /// </summary>
    public string RequestToken { get; set; }


    /// <summary>
    /// 创建日志
    /// </summary> 
    /// <returns></returns>
    public string Create()
    {
        return "文件名：" + FileName + Common.EnterStr +
               "文件路径：" + FilePath + Common.EnterStr +
               "文件类型：" + FileType + Common.EnterStr +
               "桶名称：" + BucketName;
    }

    /// <summary>
    /// 创建报错日志
    /// </summary> 
    /// <param name="error">异常</param> 
    /// <returns></returns>
    public string Error(Exception error)
    {
        return "文件名：" + FileName + Common.EnterStr +
               "文件路径：" + FilePath + Common.EnterStr +
               "文件类型：" + FileType + Common.EnterStr +
               "桶名称：" + BucketName + Common.EnterStr +
               "错误原因：" + error.Message + Common.EnterStr + error.StackTrace.ToString();
    }
} 
