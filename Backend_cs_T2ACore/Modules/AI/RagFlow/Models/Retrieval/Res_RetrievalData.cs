using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 检索数据
/// </summary>
public class Res_RetrievalData
{
    /// <summary>
    /// 分块列表
    /// </summary>
    public List<Res_ChunkInfo> Chunks { get; set; } = new List<Res_ChunkInfo>();
    
    /// <summary>
    /// 文档聚合
    /// </summary>
    public List<Res_DocumentAggregate> DocAggs { get; set; } = new List<Res_DocumentAggregate>();
    
    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; set; }
}
