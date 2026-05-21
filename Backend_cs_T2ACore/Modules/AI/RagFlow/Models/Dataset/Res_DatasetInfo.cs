using System;

namespace T2ACore;

/// <summary>
/// 数据集信息
/// </summary>
public class Res_DatasetInfo
{
    /// <summary>
    /// 数据集 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 数据集名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; } = string.Empty;
    
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 嵌入模型
    /// </summary>
    public string Embedding_model { get; set; } = string.Empty;
    
    /// <summary>
    /// 权限
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
    
    /// <summary>
    /// 创建者 ID
    /// </summary>
    public string Created_by { get; set; } = string.Empty;
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public long Create_time { get; set; }
    
    /// <summary>
    /// 创建日期
    /// </summary>
    public string Create_date { get; set; } = string.Empty;
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public long Update_time { get; set; }
    
    /// <summary>
    /// 更新日期
    /// </summary>
    public string Update_date { get; set; } = string.Empty;
    
    /// <summary>
    /// 租户 ID
    /// </summary>
    public string Tenant_id { get; set; } = string.Empty;
    
    /// <summary>
    /// 文档数量
    /// </summary>
    public int Document_count { get; set; }
    
    /// <summary>
    /// 分块数量
    /// </summary>
    public int Chunk_count { get; set; }
    
    /// <summary>
    /// Token 数量
    /// </summary>
    public int Token_num { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 语言
    /// </summary>
    public string Language { get; set; } = string.Empty;
    
    /// <summary>
    /// 相似度阈值
    /// </summary>
    public double Similarity_threshold { get; set; }
    
    /// <summary>
    /// 向量相似度权重
    /// </summary>
    public double Vector_similarity_weight { get; set; }
}
