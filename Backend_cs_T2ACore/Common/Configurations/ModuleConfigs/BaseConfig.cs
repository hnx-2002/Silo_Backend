using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 系统的基本通用配置信息
/// </summary>
public class BaseConfig
{
    /// <summary>
    /// 处理跨域 来源
    /// </summary>
    public string WebAPI_Origins { get; set; }

    /// <summary>
    /// 处理跨域 方法
    /// </summary>
    public string WebAPI_Methods { get; set; }

    /// <summary>
    /// 是否使用本地登录鉴权
    /// 若为true则下方 AuthPath,AuthDomain无效, 初始密码起效
    /// </summary>
    public bool UseLoginLoginAuth { get; set; }

    /// <summary>
    /// 初始密码
    /// </summary>
    public string InitPassword { get; set; }

    /// <summary>
    /// 鉴权地址
    /// </summary>
    public string AuthPath { get; set; }

    /// <summary>
    /// 鉴权域
    /// </summary>
    public string AuthDomain { get; set; }

    /// <summary>
    /// 是否使用鉴权
    /// </summary>
    public bool UsingAuth { get; set; }

    /// <summary>
    /// 管理员、开发人员、测试人员...
    /// </summary>
    public List<string> AdminList { get; set; } = new List<string>();

    /// <summary>
    /// Token字符串
    /// </summary>
    public string TokenString { get; set; }

    /// <summary>
    /// 系统code
    /// </summary>
    public string SystemCode { get; set; }

    /// <summary>
    /// 缓存形式Core，Redis
    /// </summary>
    public string CacheType { get; set; }

    /// <summary>
    /// 是否使用Swagger
    /// </summary>
    public bool UseSwagger { get; set; }
}