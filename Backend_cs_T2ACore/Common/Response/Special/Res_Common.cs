namespace T2ACore;

/// <summary>
/// 三参数返回值
/// </summary>
public class Res_Common<T>
{
    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 实体
    /// </summary>
    public T Entity { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>  
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Res_Common<T> New(T entity)
    {
        var res = new Res_Common<T>();
        res.Status = true;
        res.Message = "OK";
        res.Entity = entity;
        return res;
    }

    /// <summary>
    /// 构造函数
    /// </summary>  
    /// <param name="msg"></param> 
    /// <returns></returns>
    public static Res_Common<T> Bad(string msg)
    {
        var res = new Res_Common<T>();
        res.Status = false;
        res.Message = msg;
        res.Entity = default;
        return res;
    }
}
