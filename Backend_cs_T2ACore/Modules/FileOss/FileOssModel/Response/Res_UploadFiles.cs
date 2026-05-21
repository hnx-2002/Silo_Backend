using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 上传批量文件，返回值
/// </summary>
public class Res_UploadFiles
{
    /// <summary>
    /// 状态消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 桶名称
    /// </summary>
    public string DiskName { get; set; }

    /// <summary>
    /// 上传详情
    /// </summary>
    public List<Res_UploadFile> UploadDetail { get; set; }

    /// <summary>
    /// 上传失败详情
    /// </summary>
    public List<Res_UploadFile> FailedDetail { get; set; }


    /// <summary>
    /// 创建失败返回值
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Res_UploadFiles Error(string error)
    {
        return new Res_UploadFiles
        {
            Msg = error,
            DiskName = null,
            UploadDetail = null,
            FailedDetail = null
        };
    }
}
