namespace T2ACore;

/// <summary>
/// MongoDB配置
/// </summary>
public class MongoDBConfig
{
    /// <summary>
    /// MongoDB数据库地址
    /// </summary>
    public string MongoUrl { get; set; }

    /// <summary>
    /// 默认数据库
    /// </summary>
    public string MongoDefaultDB { get; set; }
}