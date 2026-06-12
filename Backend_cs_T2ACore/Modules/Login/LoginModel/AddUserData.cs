using System;

namespace T2ACore;

/// <summary>
/// 添加用户返回
/// </summary>
public class AddUserData
{
    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// 信息
    /// </summary>
    public string Msg { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }
    /// <summary>
    /// Id
    /// </summary>
    public Guid UserId { get; set; }
}
