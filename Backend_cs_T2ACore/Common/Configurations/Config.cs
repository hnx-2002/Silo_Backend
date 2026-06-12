using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace T2ACore;

/// <summary>
/// 全局配置类
/// </summary>
public static class Config
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static IConfiguration Configuration { get; set; }

    /// <summary>
    /// 鉴权配置
    /// </summary>
    public static AuthConfig AuthConfig { get; set; }

    /// <summary>
    /// 基本通用配置
    /// </summary>
    public static BaseConfig BaseConfig { get; set; }


    /// <summary>
    /// 登录配置
    /// </summary>
    public static LoginConfig LoginConfig { get; set; }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public static DBConfig DBConfig { get; set; }

    /// <summary>
    /// Neo4j配置
    /// </summary>
    public static Neo4jConfig Neo4jConfig { get; set; }

    /// <summary>
    /// Redis配置
    /// </summary>
    public static RedisConfig RedisConfig { get; set; }

    /// <summary>
    /// Oss相关配置
    /// </summary>
    public static OssConfig OssConfig { get; set; }

    /// <summary>
    /// MongoDB相关配置
    /// </summary>
    public static MongoDBConfig MongoDBConfig { get; set; }

    /// <summary>
    /// Rabbitmq相关配置
    /// </summary>
    public static RabbitMQConfig RabbitMQConfig { get; set; }

    /// <summary>
    /// RestSharp配置
    /// </summary>
    public static RestSharpConfig RestSharpConfig { get; set; }

    /// <summary>
    /// Cron 配置
    /// </summary>
    public static CronConfig CronConfig { get; set; }

    /// <summary>
    /// 企业微信通用配置信息
    /// </summary>
    public static EWeChatConfig EWeChatConfig { get; set; }
}