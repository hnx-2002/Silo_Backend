using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 返回值
/// </summary>
public class Res_TextInList
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
    public TextInListData Data { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Msg { get; set; }


}

/// <summary>
/// TextIn数据
/// </summary>
public class TextInListData
{
    /// <summary>
    /// 总数
    /// </summary>
    public long Sum { get; set; }
    /// <summary>
    /// 任务集合
    /// </summary>
    public List<TextInTaskData> List { get; set; }

}

/// <summary>
/// TextIn单个任务
/// </summary>
public class TextInTaskData
{    /// <summary>
     /// 创建时间
     /// </summary>
    public long Create_time { get; set; }

    /// <summary>
    /// 文档id
    /// </summary>
    public long Doc_id { get; set; }

    /// <summary>
    /// 文档名
    /// </summary>
    public string Doc_name { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string File_type { get; set; }

    /// <summary>
    /// 最大页码
    /// </summary>
    public long Max_page { get; set; }

    /// <summary>
    /// 转化Id
    /// </summary>
    public long Parse_id { get; set; }

    /// <summary>
    /// 转化类别
    /// </summary>
    public string Parse_type { get; set; }
 
    /// <summary>
    /// 失败原因
    /// </summary>
    public string Reasons_for_failure { get; set; }

    /// <summary>
    /// 状态
    /// 0 等待处理中
    /// 1 解析中
    /// 2 解析成功
    /// 3 余额不足
    /// -1 解析失败
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 任务Id
    /// </summary>
    public string Task_id { get; set; } 
  
}