namespace T2ACore;

/// <summary>
/// 登录配置
/// </summary>
public class LoginConfig
{
    /// <summary>
    /// token受众
    /// </summary> 
    public string Audience { get; set; }

    /// <summary>
    /// token是谁颁发的
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 加密的key（SecretKey必须大于16个,是大于，不是大于等于）
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Web登录过期时间
    /// </summary>
    public int TokenExpires { get; set; }

    /// <summary>
    /// 加密盐
    /// </summary>
    public string Salt { get; set; }
}
