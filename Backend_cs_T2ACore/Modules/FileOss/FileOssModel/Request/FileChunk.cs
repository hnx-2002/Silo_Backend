using System;

namespace T2ACore;

/// <summary>
/// 文件分块
/// </summary>
public class FileChunk
{
    /// <summary>
    /// 向OSS申请的uploadId
    /// </summary>
    public string UploadId { get; set; }

    /// <summary>
    /// 任务Id
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 文件分片
    /// </summary>
    public string FileChunkContent { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }
}
