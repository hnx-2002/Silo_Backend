using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 返回值
/// </summary>
public class Res_TextInCreate
{
    /// <summary>
    /// 200 请求成功
    /// 400 坏的请求
    /// 401 未授权
    /// 403 禁止访问
    /// 404 资源未找到
    /// 406 参数错误
    /// 500 内部错误
    /// 10702 任务处理中
    /// 10703 任务失败
    /// 40429 额度不足
    /// 50207 解析任务部分任务失败
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TextInCreateData Data { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Msg { get; set; }


}

/// <summary>
/// TextIn数据
/// </summary>
public class TextInCreateData
{
    /// <summary>
    /// 任务集合
    /// </summary>
    public List<string> Task_ids { get; set; }

}