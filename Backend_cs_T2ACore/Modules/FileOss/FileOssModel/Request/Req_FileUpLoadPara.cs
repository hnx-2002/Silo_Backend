namespace T2ACore;

/// <summary>
/// 文件上传参数实体类
/// </summary>
public class Req_FileUpLoadPara
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件内容
    /// </summary>
    public byte[] FileContent { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string ContentType { get; set; }
}
