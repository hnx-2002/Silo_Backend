namespace T2ACore;

/// <summary>
/// 
/// </summary>
public class Res_ChatResHub
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    public static Res_ChatResHub New(string type, string msg)
    {
        var res = new Res_ChatResHub();
        res.MsgType = type;
        res.Msg = msg;
        return res;
    }

    /// <summary>
    /// 消息类型，text，audio
    /// </summary>
    public string MsgType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Msg { get; set; }


}
