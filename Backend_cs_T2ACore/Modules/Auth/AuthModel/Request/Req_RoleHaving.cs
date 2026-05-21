using System;

namespace T2ACore;

/// <summary>
/// 有继承关系的角色编码
/// </summary>
public class Req_RoleHaving
{
    /// <summary>
    /// 角色编码
    /// </summary>
    public string RoleCode { get; set; }

    /// <summary>
    /// 拥有的角色
    /// </summary>
    public string HavingRole { get; set; }

    /// <summary>
    /// 创建数据库实体
    /// </summary>  
    /// <param name="optAccount">操作人账号</param>
    /// <param name="optUserName">操作人姓名</param>
    /// <param name="tenant">操作人租户</param>
    /// <param name="region">角色维度</param>
    /// <returns></returns>
    public Auth_assets_Class Cast(string optAccount,
        string optUserName, string tenant, string region)
    {

        if (string.IsNullOrEmpty(RoleCode) ||
            string.IsNullOrEmpty(HavingRole))
        {
            return null;
        }
         
        var newModel = new Auth_assets_Class();
        newModel.Create_account = optAccount;
        newModel.Create_username = optUserName;
        newModel.Create_time = FunCommon.GetStandardTimeStamp();
        newModel.Remark = "";
        newModel.Section = "g";
        newModel.Type = region;
        newModel.Value1 = RoleCode;
        newModel.Value2 = HavingRole;
        newModel.Value3 = tenant;
        return newModel;
    }

}
