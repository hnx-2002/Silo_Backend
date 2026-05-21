using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 上传单个文件，返回值
/// </summary>
public class Res_UploadFile
{
    /// <summary>
    /// 上传状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 结论消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 文件完整路径
    /// </summary>
    public string FilePath { get; set; }

    // /// <summary>
    // /// 文件名
    // /// </summary>
    // public string FileName { get; set; }

    // /// <summary>
    // /// 文件扩展名
    // /// </summary>
    // public string FileExt { get; set; }

    // /// <summary>
    // /// 文件长度
    // /// </summary>
    // public int FileLength { get; set; }

    /// <summary>
    /// MD5编码
    /// </summary>
    public string MD5 { get; set; }


    /// <summary>
    /// 创建失败返回值
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Res_UploadFile Error(string error)
    {
        return new Res_UploadFile
        {
            Status = false,
            Msg = error,
            FilePath = null
        };
    }

}
