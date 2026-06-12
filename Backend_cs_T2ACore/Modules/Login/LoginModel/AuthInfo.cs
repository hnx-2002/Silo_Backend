using Newtonsoft.Json;

namespace T2ACore;

/// <summary>
/// 鉴权信息
/// </summary>
public class AuthInfo
{
    /// <summary>
    /// 账号
    /// </summary>
    public string UserAccount { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string Tenant { get; set; }

    /// <summary>
    /// 当前类转为Json
    /// </summary>
    /// <returns></returns>
    public string ToJsonString()
    {
        return JsonConvert.SerializeObject(this);
    }

}
