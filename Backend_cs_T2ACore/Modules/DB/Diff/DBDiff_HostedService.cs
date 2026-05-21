using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 对比数据库的启动执行过程
/// </summary>
public sealed class DBDiff_HostedService : IHostedService
{
    private readonly IServiceProvider _root;
    private readonly Assembly _assembly;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="root"></param> 
    /// <param name="assembly"></param> 
    public DBDiff_HostedService(IServiceProvider root, Assembly assembly)
    {
        _root = root;
        _assembly = assembly;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                using var scope = _root.CreateScope();
                var biz = scope.ServiceProvider
                    .GetRequiredService<IDBHelper>();

                var db = biz.GetDB("Default");
                var diffs = FunSugar.GetDbDiffInfos(db, _assembly);

                if (diffs == null)
                {
                    FunConsole.Log("连接字符串缺失", ConsoleColor.DarkRed);
                }
                else
                {
                    var diffStr = JsonConvert.SerializeObject(diffs);
                    FunConsole.Log(diffStr, ConsoleColor.Cyan); 
                } 
            }
            catch (Exception ex)
            {
                FunConsole.Log(ex.ToString(), ConsoleColor.DarkRed);
            }
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
