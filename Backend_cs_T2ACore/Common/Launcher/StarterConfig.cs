using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Runtime.InteropServices;

namespace T2ACore;

/// <summary>
/// 初始化配置
/// </summary>
public static class StarterConfig
{
    /// <summary>
    /// 初始化配置文件
    /// </summary>
    /// <param name="builder"></param>
    public static void InitAppSettings(this WebApplicationBuilder builder)
    {
        //鉴权配置
        Config.AuthConfig = builder.Configuration
            .GetSection("AuthConfig").Get<AuthConfig>();

        //基础配置
        Config.BaseConfig = builder.Configuration
            .GetSection("BaseConfig").Get<BaseConfig>();

        //登录配置
        Config.LoginConfig = builder.Configuration
            .GetSection("LoginConfig").Get<LoginConfig>();

        //数据库配置
        Config.DBConfig = builder.Configuration
            .GetSection("DBConfig").Get<DBConfig>();

        ////外部服务配置
        //Config.ExServiceConfig = builder.Configuration
        //    .GetSection("ExServiceConfig").Get<ExServiceConfig>();

        //图数据库配置
        Config.Neo4jConfig = builder.Configuration
            .GetSection("Neo4jConfig").Get<Neo4jConfig>();

        //redis配置
        Config.RedisConfig = builder.Configuration
            .GetSection("RedisConfig").Get<RedisConfig>();

        //mongoDB配置
        Config.MongoDBConfig = builder.Configuration
            .GetSection("MongoDBConfig").Get<MongoDBConfig>();

        //Rabbitmq配置
        Config.RabbitMQConfig = builder.Configuration
            .GetSection("RabbitMQConfig").Get<RabbitMQConfig>();

        //Oss配置
        Config.OssConfig = builder.Configuration
            .GetSection("OssConfig").Get<OssConfig>();

        //RestSharp配置
        Config.RestSharpConfig = builder.Configuration
            .GetSection("RestSharpConfig").Get<RestSharpConfig>();

        //Cron配置
        Config.CronConfig = builder.Configuration
            .GetSection("CronConfig").Get<CronConfig>();

        //企业微信通用配置信息
        Config.EWeChatConfig = builder.Configuration
            .GetSection("EWeChatConfig").Get<EWeChatConfig>();

    }

    /// <summary>
    /// 刷新配置文件
    /// </summary>
    /// <param name="app"></param>  
    /// <param name="builder"></param> 
    public static void ReloadAppSettings(this WebApplication app, WebApplicationBuilder builder)
    {
        //cyl 20220428 added 用于热更新appsettings.json
        ChangeToken.OnChange(
            () => app.Configuration.GetReloadToken(),
            () =>
            {
                //初始化配置
                builder.InitAppSettings();
            }
        );
    }

    /// <summary>
    /// 初始化基本设置
    /// </summary>
    public static void InitFunCommonData()
    {
        //识别文件夹层级符
        Common.LevelStr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";
        //识别换行符
        Common.EnterStr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\r\n" : "\n";
        //服务根路径
        Common.CurrentDllPath = AppDomain.CurrentDomain.BaseDirectory;
        //初始时间
        Common.StartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

}
