namespace PTools_PSilo;
//cyl added 20220623
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using T2ACore;

/// <summary>
/// 追踪Http中间件
/// </summary>
public class HttpTraceMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next"></param>
    public HttpTraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        context.Items["startTime"] = DateTime.Now
            .ToString("yyyy-MM-dd HH:mm:ss:fff:ffffff");
        await _next(context);
        HttpLog(context);
    }

    /// <summary>
    /// 记录Http系统日志
    /// </summary>
    /// <param name="context"></param>
    private static void HttpLog(HttpContext context)
    {
        //跳过预检请求、/swagger等
        if (context.Request.Method.Equals("OPTIONS") ||
            context.Request.Path.StartsWithSegments("/" + ConstPara.NAMESPACE + "/swagger"))
        {
            return;
        }
        var (account, userName) = ScopeUser.GetUserAccountName();
        HttpLogMsg.LogInfo(context, account, userName);
    }
}


