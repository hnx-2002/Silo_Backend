using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 文档信息
/// </summary>
public class Res_DocumentInfo
{
    /// <summary>
    /// 文档 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 文档名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 文档位置（文件路径）
    /// </summary>
    public string Location { get; set; } = string.Empty;
    
    /// <summary>
    /// 关联的数据集 ID
    /// </summary>
    public string Dataset_id { get; set; } = string.Empty;
    
    /// <summary>
    /// 知识库 ID
    /// </summary>
    public string Knowledgebase_id { get; set; } = string.Empty;
    
    /// <summary>
    /// 分块方法
    /// </summary>
    public string Chunk_method { get; set; } = string.Empty;
    
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
    /// 文件大小（字节）
    /// </summary>
    public long Size { get; set; }
    
    /// <summary>
    /// 文件类型
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// 源类型
    /// </summary>
    public string Source_type { get; set; } = string.Empty;
    
    /// <summary>
    /// 处理状态
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 运行状态
    /// </summary>
    public string Run { get; set; } = string.Empty;
    
    /// <summary>
    /// 进度（百分比）
    /// </summary>
    public double Progress { get; set; }
    
    /// <summary>
    /// 进度消息
    /// </summary>
    public string Progress_msg { get; set; } = string.Empty;
    
    /// <summary>
    /// 处理开始时间
    /// </summary>
    public string Process_begin_at { get; set; } = string.Empty;
    
    /// <summary>
    /// 处理耗时（秒）
    /// </summary>
    public double Process_duration { get; set; }
    
    /// <summary>
    /// 分块数量
    /// </summary>
    public int Chunk_count { get; set; }
    
    /// <summary>
    /// Token 数量
    /// </summary>
    public int Token_count { get; set; }
    
    /// <summary>
    /// 缩略图
    /// </summary>
    public string Thumbnail { get; set; } = string.Empty;
    
    /// <summary>
    /// 元字段
    /// </summary>
    public Dictionary<string, object> Meta_fields { get; set; } = new Dictionary<string, object>();
}
