using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 数据库配置
/// </summary>
public class DBConfig
{ 
    /// <summary>
    /// 连接集合
    /// </summary>
    public List<DBConn> DBConns { get; set; }

    /// <summary>
    /// 是否将Sql语句输出至Console
    /// </summary>
    public bool OutputToConsole { get; set; }

    /// <summary>
    /// 是否记录数据库变化
    /// </summary>
    public bool LogDiff { get; set; }
}

/// <summary>
/// 数据库连接配置
/// </summary>
public class DBConn
{
    /// <summary>
    /// 租户
    /// </summary>
    public string Tenant { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DatabaseType { get; set; }

    /// <summary>
    /// 数据库连接串
    /// </summary>
    public string LinkString { get; set; }

    /// <summary>
    /// 数据库超时时间
    /// </summary>
    public int CommandTimeOut { get; set; }

}