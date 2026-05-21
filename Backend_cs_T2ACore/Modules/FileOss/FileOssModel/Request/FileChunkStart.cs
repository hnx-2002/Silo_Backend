using System;

namespace T2ACore;

/// <summary>
/// 大文件传输开始
/// </summary>
public class FileChunkStart
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

}
