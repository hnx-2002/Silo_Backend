namespace T2ACore;

/// <summary>
/// 自定义错误返回类型
/// </summary>
public enum ErrorResponseType
{
    /// <summary>
    /// 错误
    /// </summary>
    error,

    /// <summary>
    /// 警告
    /// </summary>
    warning
}

/// <summary>
/// 自定义错误返回类型
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="type"></param>
    public ErrorResponse(string message, ErrorResponseType type = ErrorResponseType.warning)
    {
        ErrorCode = 1005;
        ExceptionMessage = message;
        ExceptionType = type.ToString();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorCode"></param>
    public ErrorResponse(ErrorCodeBase errorCode)
    {
        ExceptionType = errorCode.DefaultErrorResponseType.ToString();
        ExceptionMessage = errorCode.GetMessage();
        ErrorCode = errorCode.GetCode();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public ErrorResponse() { }

    /// <summary>
    /// 异常消息
    /// </summary>
    public string ExceptionMessage { get; }

    /// <summary>
    /// 异常类型（warning，error）
    /// </summary>
    public string ExceptionType { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public int ErrorCode { get; set; }

}