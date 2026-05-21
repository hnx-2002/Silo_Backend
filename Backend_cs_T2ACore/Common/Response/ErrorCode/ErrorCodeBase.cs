// yg added 20230302
// 灵感来源：
// 在API程序设计开发中错误码如何规划设计？ - 知乎用户的回答 - 知乎
// https://www.zhihu.com/question/24091286/answer/1090883831

namespace T2ACore;

/// <summary>
/// 错误码基类
/// </summary>
public abstract class ErrorCodeBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ErrorCodeBase() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="System.Exception"></exception>
    public ErrorCodeBase(int code)
    {
        if (code > 999 || code < 0)
        {
            throw new System.Exception("错误码应当位于 0-999 之间");
        }
        this.Code = code;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <exception cref="System.Exception"></exception>
    public ErrorCodeBase(int code, string message)
    {
        if (code > 999 || code < 0)
        {
            throw new System.Exception("错误码应当位于 0-999 之间");
        }
        this.Code = code;
        this.Message = message;
    }

    /// <summary>
    /// 默认错误返回类型
    /// </summary>
    public ErrorResponseType DefaultErrorResponseType { get; set; } = ErrorResponseType.warning;

    /// <summary>
    /// 错误码
    /// </summary>
    private int Code;

    /// <summary>
    /// 错误消息
    /// </summary>

    private string Message;

    /// <summary>
    /// 获取错误码消息
    /// </summary>
    /// <returns></returns>
    public string GetMessage() { return Message; }

    /// <summary>
    /// 获取错误码的值
    /// </summary>
    /// <returns></returns>
    public int GetCode()
    {
        System.Type type = this.GetType();
        int prefix = 0;
        var codePrefixAttributes = type.GetCustomAttributes(typeof(ErrorCodePrefixAttribute), true);
        if (codePrefixAttributes != null && codePrefixAttributes.Length > 0)
        {
            prefix = ((ErrorCodePrefixAttribute)codePrefixAttributes[0]).Prefix;
        }
        return prefix * 1000 + Code;
    }
}