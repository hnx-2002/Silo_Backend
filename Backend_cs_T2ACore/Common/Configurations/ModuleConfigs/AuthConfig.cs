using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 鉴权配置
/// </summary>
public class AuthConfig
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public List<string> RoleTitle { get; set; }

    /// <summary>
    /// 资源类型
    /// </summary>
    public List<string> Assets { get; set; }

}