using System;

namespace T2ACore;

/// <summary>
/// 更新数据集请求
/// </summary>
public class Req_UpdateDataset
{
    /// <summary>
    /// 数据集名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 头像，Base64 编码
    /// </summary>
    public string Avatar { get; set; } = string.Empty;
    
    /// <summary>
    /// 数据集描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 嵌入模型名称
    /// </summary>
    public string Embedding_model { get; set; } = string.Empty;
    
    /// <summary>
    /// 权限设置
    /// </summary>
    public string Permission { get; set; } = string.Empty;
    
    /// <summary>
    /// 分块方法
    /// </summary>
    public string Chunk_method { get; set; } = string.Empty;
    
    /// <summary>
    /// 页面排名
    /// </summary>
    public int Pagerank { get; set; }
    
    /// <summary>
    /// 解析器配置
    /// </summary>
    public Req_ParserConfig Parser_config { get; set; }
}
