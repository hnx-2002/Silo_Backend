using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 使用Dify问问题
/// </summary>
public class Req_DifyAsk
{
    /// <summary>
    /// 输入
    /// </summary>
    public JObject inputs { get; set; }

    /// <summary>
    /// 问题
    /// </summary>
    public string query { get; set; }

    /// <summary>
    /// 返回模式
    /// </summary>
    public string response_mode { get; set; } = "streaming";

    /// <summary>
    /// 对话Id
    /// </summary>
    public string conversation_id { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public string user { get; set; }

    /// <summary>
    /// 文件
    /// </summary>
    public List<Req_DifyImgFile> files { get; set; }

    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="askInput"></param>
    /// <param name="question"></param>
    /// <param name="account"></param>
    /// <returns></returns>
    public static Req_DifyAsk Cast(JObject askInput,
        string question, string account)
    {
        var newModel = new Req_DifyAsk();
        newModel.inputs = askInput;
        newModel.query = question;
        newModel.response_mode = "streaming";
        newModel.conversation_id = "";
        newModel.user = string.IsNullOrEmpty(account) ? "www" : account;
        newModel.files = [];
        return newModel;
    }
    
    /// <summary>
    /// 转化
    /// </summary> 
    /// <param name="account"></param>
    /// <returns></returns>
    public static Req_DifyAsk UploadCast(string account)
    {
        var newModel = new Req_DifyAsk();
        newModel.inputs = new JObject();
        newModel.query = "";
        newModel.response_mode = "streaming";
        newModel.conversation_id = "";
        newModel.user = string.IsNullOrEmpty(account) ? "www" : account;
        newModel.files = [];
        return newModel;
    }
}


