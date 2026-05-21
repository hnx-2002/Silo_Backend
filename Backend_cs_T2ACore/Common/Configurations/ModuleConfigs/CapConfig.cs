namespace T2ACore;

/// <summary>
/// Cap配置
/// </summary>
public class CapConfig
{
    /// <summary>
    /// Subscriber default group name. kafka-->group name. rabbitmq --> queue name.
    /// </summary>
    public string DefaultGroupName { get; set; }

    /// <summary>
    ///  Subscriber group prefix.
    /// </summary>
    public string GroupNamePrefix { get; set; }

    /// <summary>
    /// Topic prefix.
    /// </summary>
    public string TopicNamePrefix { get; set; }
}