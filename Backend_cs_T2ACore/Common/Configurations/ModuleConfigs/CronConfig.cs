namespace T2ACore;

/// <summary>
/// CronConfig配置
/// </summary>
public class CronConfig
{
    /// <summary>
    /// 接口请求超过多少秒记录警告日志
    /// </summary>
    public int WarnTime { get; set; }

    /// <summary>
    /// 最多保存日志数量
    /// </summary>
    public int MaxLogCount { get; set; }
}