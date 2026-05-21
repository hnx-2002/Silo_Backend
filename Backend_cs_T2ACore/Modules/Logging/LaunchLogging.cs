using Exceptionless;
using Exceptionless.Extensions.Logging;
using Exceptionless.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Web;
using System.Collections.Generic;
using System.Linq;

namespace T2ACore;


/// <summary>
/// Logging扩展
/// </summary>
public static class LaunchLogging
{
    /// <summary>
    /// 注入Logging服务
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="nlogOpt"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, NLogAspNetCoreOptions nlogOpt = null)
    {
        _ = NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
        //NLog.LogManager.Configuration = new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog"));

        builder.Logging.ClearProviders();

        // 控制落盘日志request.log 如需要，则需开启下边注释内容（request.log与sysLogOp 设计理念不一样，request.log无差别）
        // builder.Services.AddHttpLogging(o =>
        // {
        //    o.RequestBodyLogLimit = 250;
        //    o.CombineLogs = true;
        //    o.LoggingFields = HttpLoggingFields.RequestBody |
        //                        HttpLoggingFields.Duration |
        //                        HttpLoggingFields.RequestPath |
        //                        HttpLoggingFields.RequestQuery |
        //                        HttpLoggingFields.RequestHeaders |
        //                        HttpLoggingFields.ResponseStatusCode;
        // });

        // 注册并使用 nlog provider
        builder.Host.UseNLog(nlogOpt ?? NLogAspNetCoreOptions.Default);
        var nlogConfig = NLog.LogManager.Configuration; //可以在这里移除配置，来实现动态控制是否写入文件

        // 添加 exceptionless 的 provider，及相关 servier 注册
        builder.Services.AddSingleton<ILoggerProvider>(
            sp => new ExceptionlessLoggerProvider(sp.GetService<ExceptionlessClient>() ?? ExceptionlessClient.Default));
        builder.Services.AddLogging(builder => builder.AddFilter<ExceptionlessLoggerProvider>(null, Microsoft.Extensions.Logging.LogLevel.Trace));
        // 自动读取 Exceptionless 配置
        builder.Services.AddExceptionless(builder.Configuration, opt =>
        {
            opt.AddPlugin<ExceptionlessPlugin>();
            ConvertToExceptionlessConfig(opt, NLog.LogManager.Configuration, builder.Configuration);
        });


        return builder;
    }


    /// <summary>
    /// 将 nlog 配置转换为 exceptionless 的配置
    /// </summary>
    /// <param name="exceptionlessConfig"></param>
    /// <param name="nlogConfig"></param>
    /// <param name="configuration"></param>
    private static void ConvertToExceptionlessConfig(ExceptionlessConfiguration exceptionlessConfig,
        LoggingConfiguration nlogConfig, IConfiguration configuration = null)
    {
        // exceptionlessConfig.UseFileLogger("D:\\exceptionless.log");
        exceptionlessConfig.UseInMemoryStorage();
        Dictionary<string, Exceptionless.Logging.LogLevel> loggerMinLevels = [];
        // 处理 nlog 的配置，筛选出不同 logger 的最小日志等级
        foreach (var nlogRule in nlogConfig.LoggingRules)
        {
            var finalMinLevel = MapLogLevel(nlogRule.Levels.MinBy(p => p.Ordinal));
            if (nlogRule.Final)
            {
                if (loggerMinLevels.TryGetValue(nlogRule.LoggerNamePattern, out var minLevel))
                {
                    if (minLevel.Ordinal < finalMinLevel.Ordinal)
                    {
                        finalMinLevel = minLevel;
                    }
                }
            }
            // 对 black hole 的特殊处理
            if (nlogRule.Targets.Count == 0 && nlogRule.Final == true)
            {
                finalMinLevel = Exceptionless.Logging.LogLevel.Off;
            }
            loggerMinLevels[nlogRule.LoggerNamePattern] = finalMinLevel;
        }
        foreach (var loggerCnf in loggerMinLevels)
        {
            exceptionlessConfig.Settings[$"@@log:{loggerCnf.Key}"] = loggerCnf.Value.Name;
        }

        // 事件过滤在启动阶段（LaunchException）中统一注册，避免在配置转换阶段耦合事件处理。
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="nlogLogLevel"></param>
    /// <returns></returns>
    private static Exceptionless.Logging.LogLevel MapLogLevel(NLog.LogLevel nlogLogLevel)
    {
        // Map NLog log levels to Exceptionless log levels
        // Customize this based on your specific mapping requirements
        return nlogLogLevel.Name switch
        {
            "Trace" => Exceptionless.Logging.LogLevel.Trace,
            "Debug" => Exceptionless.Logging.LogLevel.Debug,
            "Info" => Exceptionless.Logging.LogLevel.Info,
            "Warn" => Exceptionless.Logging.LogLevel.Warn,
            "Error" => Exceptionless.Logging.LogLevel.Error,
            "Fatal" => Exceptionless.Logging.LogLevel.Fatal,
            _ => Exceptionless.Logging.LogLevel.Off,
        };
    }
}
