namespace T2ACore;

/// <summary>
/// 错误码定义
/// </summary>

[ErrorCodePrefix(1)]
public class ErrorCodeGeneric : ErrorCodeBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public ErrorCodeGeneric(int code, string message) : base(code, message) { }

    /// <summary>
    /// 内部错误
    /// </summary>
    public static ErrorCodeGeneric InternalError = new(0, "系统内部错误");

    /// <summary>
    /// 登录过期
    /// </summary>
    public static ErrorCodeGeneric AuthExpired = new(1, "用户登录过期，请尝试重新登陆");

    /// <summary>
    /// 无权限
    /// </summary>
    public static ErrorCodeGeneric UnAuthorized = new(2, "当前用户禁止进行操作");

}