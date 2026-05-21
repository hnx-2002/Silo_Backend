using System;

namespace T2ACore;

/// <summary>
/// API 异常
/// </summary>
public class RagFlowException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常消息</param>
    public RagFlowException(string message) : base(message)
    {}
}
