namespace T2ACore;

/// <summary>
/// 三参数返回值
/// </summary>
public class Res_3Para
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
    /// 信息2
    /// </summary>
    public string Message2 { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    /// <param name="msg2"></param>
    /// <returns></returns>
    public static Res_3Para New(bool status, string msg, string msg2)
    {
        var res = new Res_3Para();
        res.Status = status;
        res.Message = msg;
        res.Message2 = msg2;
        return res;
    }
}
