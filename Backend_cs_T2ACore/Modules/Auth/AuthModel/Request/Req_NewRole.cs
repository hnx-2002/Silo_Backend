using System;

namespace T2ACore;

/// <summary>
/// 新建角色
/// </summary>
public class Req_NewRole
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid Parent_id { get; set; }

    ///// <summary>
    ///// 角色维度
    ///// </summary>
    //public int RegionType { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public string Role_code { get; set; }

    /// <summary>
    /// 角色标题
    /// </summary>
    public string Role_title { get; set; }

    /// <summary>
    /// 转为Table实体类
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="account"></param>
    /// <param name="username"></param>
    /// <param name="region"></param>
    /// <param name="regionTitle"></param>
    /// <returns></returns>
    public Auth_role_Class Cast(string tenant, string account,
        string username, string region, string regionTitle)
    {
        var newModel = new Auth_role_Class();
        newModel.Id = Id;
        newModel.Parent_id = Parent_id;
        newModel.Role_code = Role_code;
        newModel.Role_tenant_code = tenant;
        newModel.Role_title = Role_title;
        newModel.Region = region;
        newModel.Region_title = regionTitle;
        newModel.Create_account = account;
        newModel.Create_username = username;
        newModel.Create_time = FunCommon.GetStandardTimeStamp();
        newModel.Update_account = account;
        newModel.Update_username = username;
        newModel.Update_time = FunCommon.GetStandardTimeStamp();
        newModel.Remark = "";

        return newModel;
    }
}
