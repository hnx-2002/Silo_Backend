using SqlSugar;
using System.Collections.Generic; 

namespace T2ACore;

/// <summary>
/// 角色树
/// </summary>
[SugarTable("auth_assets")]
public class RoleTree : RoleList
{
    /// <summary>
    /// 拥有的角色
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<RoleTree> Havings { get; set; }
     
}
