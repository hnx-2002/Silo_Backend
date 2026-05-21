// yg added 20230302

using System;

namespace T2ACore;

/// <summary>
/// 错误码前缀标签
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ErrorCodePrefixAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="prefix"></param>
    public ErrorCodePrefixAttribute(int prefix)
    {
        Prefix = prefix;
    }

    /// <summary>
    /// 前缀
    /// </summary>
    public int Prefix { get; private set; }

}