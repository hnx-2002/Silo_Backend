using System;

namespace T2ACore;

/// <summary>
/// 文档聚合
/// </summary>
public class Res_DocumentAggregate
{
    /// <summary>
    /// 文档 ID
    /// </summary>
    public string Document_id { get; set; } = string.Empty;
    
    /// <summary>
    /// 文档名称
    /// </summary>
    public string Document_name { get; set; } = string.Empty;
    
    /// <summary>
    /// 聚合内容
    /// </summary>
    public string Aggregate_content { get; set; } = string.Empty;
    
    /// <summary>
    /// 相关分块数量
    /// </summary>
    public int Related_chunk_count { get; set; }
}
