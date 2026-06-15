namespace PTools_PSilo.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using T2ACore;

/// <summary>
/// 插件窗体鉴权控制器。
/// </summary>
[ApiController]
[Route("/" + ConstPara.NAMESPACE + "/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "Sys")]
public class FormController : ControllerBase
{
    /// <summary>
    /// 判断插件窗体是否可用。
    /// </summary>
    /// <returns>鉴权通过时返回true。</returns>
    [HttpPost]
    public TPResponse<bool> FormAuth()
    {
        return TPResponse.New(true);
    }

    /// <summary>
    /// 判断当前用户是否为管理员。
    /// </summary>
    /// <returns>当前用户是管理员时返回true。</returns>
    [HttpPost]
    public TPResponse<bool> IsAdmin([FromServices] IAuthBusiness authBusiness)
    {
        var (account, _) = ScopeUser.GetUserAccountName();
        var roles = authBusiness.GetRolesListForUser(account);

        var adminRoles = roles.FindAll(role => role.UpRole == "Admin");
        var hasAdminRole = adminRoles.Count > 0;

        var adminRoleChecks = new List<(
            RoleList Role,
            List<Auth_role_Class> SysRoles,
            bool RoleCodeMatched,
            bool RegionMatched)>();

        foreach (var role in adminRoles)
        {
            var sysRoles = authBusiness.SearchBy_RoleCode_Tenant(role.UpRole, role.Tenant);
            var roleCodeMatched = sysRoles.Exists(sysRole => sysRole.Role_code == role.UpRole);
            var regionMatched = sysRoles.Exists(sysRole =>
                sysRole.Role_code == role.UpRole &&
                sysRole.Region == role.Region);

            adminRoleChecks.Add((role, sysRoles, roleCodeMatched, regionMatched));
        }

        var isAdmin = hasAdminRole && adminRoleChecks.Exists(check => check.RegionMatched);
        return TPResponse.New(isAdmin);
    }
}
