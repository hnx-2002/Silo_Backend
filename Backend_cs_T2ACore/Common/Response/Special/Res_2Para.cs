namespace T2ACore;

/// <summary>
/// 两参数返回值
/// </summary>
public class Res_2Para
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
    /// 构造函数
    /// </summary>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static Res_2Para New(bool status, string msg)
    {
        var res = new Res_2Para();
        res.Status = status;
        res.Message = msg;
        return res;
    }
}

