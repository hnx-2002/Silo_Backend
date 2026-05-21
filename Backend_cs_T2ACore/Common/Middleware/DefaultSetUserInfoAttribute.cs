//wrx cyl added 20230225

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Linq;

namespace T2ACore;

/// <summary>
/// [新增]给实体类传参提供由token带上的账号/姓名及时间
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DefaultSetAddUserInfoAttribute : Attribute, IActionFilter
{
    /// <summary>
    /// 执行之后
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    /// <summary>
    /// 执行中
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.GetUserInfo((para, account, username, dt) =>
        {
            para.GetType().GetProperty("Create_account")?.SetValue(para, account, null);
            para.GetType().GetProperty("Create_username")?.SetValue(para, username, null);
            para.GetType().GetProperty("Create_time")?.SetValue(para, dt, null);

            para.GetType().GetProperty("Update_account")?.SetValue(para, account, null);
            para.GetType().GetProperty("Update_username")?.SetValue(para, username, null);
            para.GetType().GetProperty("Update_time")?.SetValue(para, dt, null);
        });
    }
}


/// <summary>
/// [更新]给实体类传参提供由token带上的账号/姓名及时间
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DefaultSetUpdateUserInfoAttribute : Attribute, IActionFilter
{
    /// <summary>
    /// 执行之后
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    /// <summary>
    /// 执行中
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.GetUserInfo((para, account, username, dt) =>
        {
            para.GetType().GetProperty("Update_account")?.SetValue(para, account, null);
            para.GetType().GetProperty("Update_username")?.SetValue(para, username, null);
            para.GetType().GetProperty("Update_time")?.SetValue(para, dt, null);
        });
    }
}

/// <summary>
/// UserInfo扩展
/// </summary>
public static class UserInfoExtension
{
    /// <summary>
    /// 获取用户信息,并赋值给满足实体类名以"_Class"结尾的类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="action"></param>
    public static void GetUserInfo(this ActionExecutingContext context,
           Action<object, string, string, long> action)
    {
        var (account, username) = ScopeUser.GetUserAccountName();
        var dt = FunCommon.GetStandardTimeStamp();

        var args = context.ActionArguments.Values.ToList();
        foreach (var arg in args)
        {
            var argType = arg.GetType().FullName;
            if (!arg.GetType().IsGenericType)
            {
                if (arg.GetType().Name.EndsWith("_Class"))
                {
                    action(arg, account, username, dt);
                }
            }
            else
            {
                if (arg is IEnumerable)
                {
                    foreach (var item in (IEnumerable)arg)
                    {
                        if (item.GetType().Name.EndsWith("_Class"))
                        {
                            action(item, account, username, dt);
                        }
                    }
                }

            }
        }

    }

}
