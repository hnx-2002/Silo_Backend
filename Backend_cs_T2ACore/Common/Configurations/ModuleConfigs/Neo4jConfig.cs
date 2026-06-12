namespace T2ACore;

/// <summary>
/// 数据库配置
/// </summary>
public class Neo4jConfig
{ 
    /// <summary>
    /// 数据库地址
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 数据库用户名
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    public string Pass { get; set; }

    /// <summary>
    /// 数据库库名
    /// </summary>
    public string Neo4jDB { get; set; }
     

}