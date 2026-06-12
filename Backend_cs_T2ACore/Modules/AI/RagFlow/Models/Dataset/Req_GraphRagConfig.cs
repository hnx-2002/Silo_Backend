using System;

namespace T2ACore;

/// <summary>
/// GraphRAG 配置
/// </summary>
public class Req_GraphRagConfig
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// 关系抽取模型
    /// </summary>
    public string RelationExtractionModel { get; set; } = string.Empty;
    
    /// <summary>
    /// 最大节点数
    /// </summary>
    public int MaxNodes { get; set; }
    
    /// <summary>
    /// 最大关系数
    /// </summary>
    public int MaxRelations { get; set; }
}
