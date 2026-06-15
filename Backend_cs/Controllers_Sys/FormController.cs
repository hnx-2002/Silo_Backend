namespace PTools_PSilo.Controllers;

using Microsoft.AspNetCore.Mvc;
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
    public TPResponse<bool> IsAdmin()
    {
        var (account, _) = ScopeUser.GetUserAccountName();
        return TPResponse.New(Config.BaseConfig.AdminList.Contains(account));
    }
}
