using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 分块信息
/// </summary>
public class Res_ChunkInfo
{
    /// <summary>
    /// 分块 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 分块内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 内容分词
    /// </summary>
    public string Content_ltks { get; set; } = string.Empty;

    /// <summary>
    /// 文档 ID
    /// </summary>
    public string Document_id { get; set; } = string.Empty;

    /// <summary>
    /// 文档名称
    /// </summary>
    public string Docnm_kwd { get; set; } = string.Empty;

    /// <summary>
    /// 数据集 ID
    /// </summary>
    public string Dataset_id { get; set; } = string.Empty;

    /// <summary>
    /// 知识库 ID
    /// </summary>
    public string Kb_id { get; set; } = string.Empty;

    /// <summary>
    /// 图片 ID
    /// </summary>
    public string Image_id { get; set; } = string.Empty;

    /// <summary>
    /// 关键词
    /// </summary>
    public List<string> Important_keywords { get; set; } = new List<string>();

    /// <summary>
    /// 关键词列表
    /// </summary>
    public List<string> Important_keywords_list { get; set; } = new List<string>();

    /// <summary>
    /// 问题列表
    /// </summary>
    public List<string> Questions { get; set; } = new List<string>();

    /// <summary>
    /// 位置
    /// </summary>
    public List<string> Positions { get; set; } = new List<string>();

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool Available { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public string Create_time { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间戳
    /// </summary>
    public double Create_timestamp { get; set; }

    /// <summary>
    /// 相似度
    /// </summary>
    public double Similarity { get; set; }

    /// <summary>
    /// 向量相似度
    /// </summary>
    public double Vector_similarity { get; set; }

    /// <summary>
    /// 术语相似度
    /// </summary>
    public double Term_similarity { get; set; }

    /// <summary>
    /// 高亮内容
    /// </summary>
    public string Highlight { get; set; } = string.Empty;
}
