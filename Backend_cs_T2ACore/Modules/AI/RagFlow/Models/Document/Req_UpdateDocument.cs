using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 更新文档请求
/// </summary>
public class Req_UpdateDocument
{
    /// <summary>
    /// 文档名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 元字段
    /// </summary>
    public Dictionary<string, object> Meta_fields { get; set; } = new Dictionary<string, object>();
    
    /// <summary>
    /// 分块方法
    /// </summary>
    public string Chunk_method { get; set; } = string.Empty;
    
    /// <summary>
    /// 解析器配置
    /// </summary>
    public Req_ParserConfig Parser_config { get; set; }
}
