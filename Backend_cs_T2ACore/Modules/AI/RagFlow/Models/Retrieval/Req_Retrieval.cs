using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 检索请求
/// </summary>
public class Req_Retrieval
{
    /// <summary>
    /// 用户查询或查询关键词（必填）
    /// </summary>
    public string Question { get; set; } = string.Empty;

    /// <summary>
    /// 要搜索的数据集 ID 列表
    /// </summary>
    public List<string> Dataset_ids { get; set; } = new List<string>();

    /// <summary>
    /// 要搜索的文档 ID 列表
    /// </summary>
    public List<string> Document_ids { get; set; } = new List<string>();

    /// <summary>
    /// 页码，默认 1
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量，默认 30
    /// </summary>
    public int Page_size { get; set; } = 30;

    /// <summary>
    /// 最小相似度分数，默认 0.2
    /// </summary>
    public double Similarity_threshold { get; set; } = 0.2;

    /// <summary>
    /// 向量余弦相似度权重，默认 0.3
    /// </summary>
    public double Vector_similarity_weight { get; set; } = 0.3;

    /// <summary>
    /// 参与向量余弦计算的分块数量，默认 1024
    /// </summary>
    public int Top_k { get; set; } = 100;

    /// <summary>
    /// 重排序模型 ID
    /// </summary>
    public string Rerank_id { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用基于关键词的匹配，默认 false
    /// </summary>
    public bool Keyword { get; set; }

    /// <summary>
    /// 是否启用匹配词高亮，默认 false
    /// </summary>
    public bool Highlight { get; set; }
}
