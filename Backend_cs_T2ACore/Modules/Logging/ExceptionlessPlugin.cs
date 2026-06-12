using Exceptionless;
using Exceptionless.Plugins; 

namespace T2ACore;

/// <summary>
/// exceptionless 添加用户信息
/// </summary>
public class ExceptionlessPlugin : IEventPlugin
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Run(EventPluginContext context)
    {
        var userInfo = ScopeUser.GetUserInfo();
        if (userInfo != null)
        {
            context.Event.SetUserIdentity(ConvertUserInfo(userInfo));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tpUserInfo"></param>
    /// <returns></returns>
    public static Exceptionless.Models.Data.UserInfo ConvertUserInfo(UserInfo tpUserInfo)
    {
        Exceptionless.Models.Data.UserInfo exUserinfo = new()
        {
            Identity = tpUserInfo.UserId.ToString(),
            Name = $"{tpUserInfo.UserName}({tpUserInfo.Account})",
        };
        // exUserinfo.Data = new Exceptionless.Models.DataDictionary()
        // {
        //     { "UUID", tpUserInfo.UserId },
        //     { "IsInitPass", tpUserInfo.IsInitPass },
        //     { "Mobile", tpUserInfo.Mobile },
        //     { "Email", tpUserInfo.Gender },
        //     { "exp", tpUserInfo.exp }
        // };
        return exUserinfo;
    }
}

