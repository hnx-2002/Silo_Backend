namespace T2ACore;

/// <summary>
/// RestSharp配置
/// </summary>
public class RestSharpConfig
{
    /// <summary>
    /// 超时时间
    /// </summary>
    public int TimeOut { get; set; }

    /// <summary>
    /// 系统秘钥，用于获取token
    /// </summary>
    public string SecretKey { get; set; }

}