using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 更新分块请求
/// </summary>
public class Req_UpdateChunk
{
    /// <summary>
    /// 分块内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// 关键词列表
    /// </summary>
    public List<string> Important_keywords { get; set; } = new List<string>();
    
    /// <summary>
    /// 是否可用
    /// </summary>
    public bool Available { get; set; }
}
