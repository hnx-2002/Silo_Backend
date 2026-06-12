using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 解析器配置
/// </summary>
public class Req_ParserConfig
{
    /// <summary>
    /// 自动提取关键词数量（0-32，默认 0）
    /// </summary>
    public int AutoKeywords { get; set; }
    
    /// <summary>
    /// 自动提问数量，1-10，默认 0
    /// </summary>
    public int AutoQuestions { get; set; }
    
    /// <summary>
    /// 分块 token 数量，1-2048，默认 128
    /// </summary>
    public int ChunkTokenNum { get; set; }
    
    /// <summary>
    /// 分隔符（默认"\n!?.;。！？；"）
    /// </summary>
    public string Delimiter { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否将 Excel 转换为 HTML（默认 false）
    /// </summary>
    public bool Html4excel { get; set; }
    
    /// <summary>
    /// 布局识别（默认"DeepDOC"）
    /// </summary>
    public string LayoutRecognize { get; set; } = string.Empty;
    
    /// <summary>
    /// 标签知识库 ID 列表
    /// </summary>
    public List<string> TagKbIds { get; set; } = new List<string>();
    
    /// <summary>
    /// 任务页面大小（仅 PDF，默认 12）
    /// </summary>
    public int TaskPageSize { get; set; }
    
    /// <summary>
    /// RAPTOR 配置
    /// </summary>
    public Req_RaptorConfig Raptor { get; set; }
    
    /// <summary>
    /// GraphRAG 配置
    /// </summary>
    public Req_GraphRagConfig GraphRag { get; set; }
}
