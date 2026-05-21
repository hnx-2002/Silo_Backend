namespace T2ACore;

/// <summary>
/// 接口返回值包装类
/// </summary>
public class TPResponse
{
    /// <summary>
    /// 构造函数(静态方法)
    /// </summary>
    /// <param name="message">返回信息</param>
    /// <param name="result">结果</param>
    /// <param name="statusCode">自定义的状态码，默认值1000</param>
    public static TPResponse<T> New<T>(string message, T result = default, int statusCode = 1000)
    {
        var res = new TPResponse<T>();
        res.StatusCode = statusCode;
        res.Message = message;
        res.IsError = false;
        res.Result = result;
        return res;
    }

    /// <summary>
    /// 构造函数(静态方法)
    /// </summary> 
    /// <param name="result">结果</param>
    /// <param name="statusCode">自定义的状态码，默认值1000</param>
    public static TPResponse<T> New<T>(T result = default, int statusCode = 1000)
    {
        var res = new TPResponse<T>();
        res.StatusCode = statusCode;
        res.Message = "";
        res.IsError = false;
        res.Result = result;
        return res;
    }

    /// <summary>
    /// 构造函数(静态方法)
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="message">返回信息</param>
    /// <param name="result">返回值</param>
    /// <param name="statusCode">自定义的状态码，默认值1000</param>
    /// <returns></returns>
    public static TPResponse<T> Bad<T>(string message, T result = default, int statusCode = 1005)
    {
        var res = new TPResponse<T>();
        res.StatusCode = statusCode;
        res.Message = message;
        res.IsError = true;
        res.Result = result;
        return res;
    }
}

/// <summary>
/// 接口返回值包装类
/// 需要指明包装类包装的对象类型 
/// </summary>
/// <typeparam name="T"></typeparam>
public class TPResponse<T>
{
    /// <summary>
    /// 状态码 默认值1000
    /// </summary>
    public int StatusCode { get; set; } = 1000;

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 是否错误
    /// </summary>
    public bool? IsError { get; set; }

    /// <summary>
    /// 结果
    /// </summary>
    public T Result { get; set; }
}

