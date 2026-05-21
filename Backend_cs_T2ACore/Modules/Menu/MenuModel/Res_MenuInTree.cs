using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 树形菜单返回值
/// </summary>
public class Res_MenuInTree
{
    /// <summary>
    /// 接口状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 树形菜单
    /// </summary>
    public List<Menu_InTree> Datas { get; set; }
}
