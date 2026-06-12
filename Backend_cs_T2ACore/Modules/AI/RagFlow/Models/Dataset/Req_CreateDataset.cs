using System;

namespace T2ACore;

/// <summary>
/// 创建数据集请求
/// </summary>
public class Req_CreateDataset
{
    /// <summary>
    /// 数据集名称（必填，唯一，不区分大小写，最多 128 字符）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 头像，Base64 编码（最多 65535 字符）
    /// 格式要求：data:image/png;base64,xxxx 或 data:image/jpeg;base64,xxxx
    /// </summary>
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 数据集描述（最多 65535 字符）
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 权限设置，"me"（仅自己）或"team"（团队成员）
    /// </summary>
    public string Permission { get; set; } = string.Empty;

    /// <summary>
    /// 页面排名，-100，默认 0
    /// </summary>
    public int Pagerank { get; set; }

    /// <summary>
    /// 默认的透明像素头像（1x1 PNG）
    /// </summary>
    private const string DefaultAvatar = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";

    /// <summary>
    /// 创建数据集请求
    /// </summary>
    /// <param name="datasetName">数据集名称（必填，唯一，不区分大小写，最多 128 字符）</param>
    /// <param name="description">数据集描述（最多 65535 字符）</param>  
    /// <returns></returns>
    public static Req_CreateDataset New(string datasetName, string description)
    {
        return new Req_CreateDataset
        {
            Name = datasetName.ToLower(),
            Avatar = DefaultAvatar,
            Description = description,
            Permission = "me",
            Pagerank = 0
        };
    }

    /// <summary>
    /// 使用自定义头像创建
    /// </summary>
    /// <param name="datasetName">数据集名称</param>
    /// <param name="description">描述</param>
    /// <param name="imageBytes">图片字节数组</param>
    /// <param name="mimeType">MIME类型，默认 image/png</param> 
    /// <returns></returns>
    public static Req_CreateDataset NewWithAvatar(string datasetName, string description,
        byte[] imageBytes, string mimeType = "image/png")
    {
        var base64 = Convert.ToBase64String(imageBytes);
        return new Req_CreateDataset
        {
            Name = datasetName.ToLower(),
            Avatar = $"data:{mimeType};base64,{base64}",
            Description = description,
            Permission = "me",
            Pagerank = 0
        };
    }
}