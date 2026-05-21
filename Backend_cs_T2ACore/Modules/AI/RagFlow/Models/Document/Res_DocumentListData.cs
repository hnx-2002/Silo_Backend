using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 文档列表数据
/// </summary>
public class Res_DocumentListData
{
    /// <summary>
    /// 文档列表
    /// </summary>
    public List<Res_DocumentInfo> Docs { get; set; } = new List<Res_DocumentInfo>();
    
    /// <summary>
    /// 文档总数
    /// </summary>
    public int Total { get; set; }
}
