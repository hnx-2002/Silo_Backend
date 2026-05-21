using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 分块列表数据
/// </summary>
public class Res_ChunkListData
{
    /// <summary>
    /// 分块列表
    /// </summary>
    public List<Res_ChunkInfo> Chunks { get; set; } = new List<Res_ChunkInfo>();
    
    /// <summary>
    /// 文档信息
    /// </summary>
    public Res_DocumentInfo Doc { get; set; }
    
    /// <summary>
    /// 分块总数
    /// </summary>
    public int Total { get; set; }
}
