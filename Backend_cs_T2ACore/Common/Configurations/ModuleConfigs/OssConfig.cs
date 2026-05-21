namespace T2ACore;

/// <summary>
/// Oss相关配置项
/// </summary>
public class OssConfig
{
    /// <summary>
    /// Oss类型
    /// </summary>
    public string OssType { get; set; }

    /// <summary>
    /// 节点
    /// </summary>
    public string Endpoint { get; set; }

    /// <summary>
    /// 访问资源前缀
    /// </summary>
    public string VisitPreUrl { get; set; }

    /// <summary>
    /// Access
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// Secret
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// 桶名称，需要填写
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 是否开启https
    /// </summary>
    public bool IsHttps { get; set; }

    /// <summary>
    /// 服务前缀
    /// </summary>
    public string Prefix { get; set; }
}