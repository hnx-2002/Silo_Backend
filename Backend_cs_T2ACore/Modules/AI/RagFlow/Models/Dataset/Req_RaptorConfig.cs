using System;

namespace T2ACore;

/// <summary>
/// Raptor 配置
/// </summary>
public class Req_RaptorConfig
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// 最大层级
    /// </summary>
    public int MaxLevels { get; set; }
    
    /// <summary>
    /// 摘要模型
    /// </summary>
    public string SummaryModel { get; set; } = string.Empty;
}
