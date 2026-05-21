using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 添加分块请求
/// </summary>
public class Req_AddChunk
{
    /// <summary>
    /// 分块内容（必填）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 关键词列表
    /// </summary>
    public List<string> Important_keywords { get; set; } = new List<string>();

    /// <summary>
    /// 问题列表
    /// </summary>
    public List<string> Questions { get; set; } = new List<string>();

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="content"></param>
    /// <param name="keyWords"></param>
    /// <returns></returns>
    public static Req_AddChunk New(string content, List<string> keyWords)
    {
        var newModel = new Req_AddChunk();
        newModel.Content = content;
        newModel.Important_keywords = keyWords;
        return newModel;
    }
}
