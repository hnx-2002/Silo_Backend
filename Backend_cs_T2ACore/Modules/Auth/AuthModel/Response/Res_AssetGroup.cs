using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 资源返回值
/// </summary>
public class Res_AssetGroup
{
    /// <summary>
    /// 角色维度 g，g2...
    /// </summary>
    public string RoleRegion { get; set; }

    /// <summary>
    /// 租户 Value2
    /// </summary>
    public string Tenant { get; set; }

    /// <summary>
    /// 资源集合
    /// </summary>
    public List<Res_Asset> Assets { get; set; } = new List<Res_Asset>();
}
