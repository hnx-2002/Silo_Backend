using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 带翻页内容返回值
/// </summary>
public class PagedResponse
{
    /// <summary>
    /// 构造函数（静态方法）
    /// </summary>
    /// <param name="total"></param>
    /// <param name="datas"></param>
    /// <returns></returns>
    public static PagedResponse<T> New<T>(int total, List<T> datas)
    {
        var res = new PagedResponse<T>();
        res.TotalCount = total;
        res.Datas = datas;
        return res;
    }
}

/// <summary>
/// 带翻页内容返回值
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// 条目总数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 结果
    /// </summary>
    public List<T> Datas { get; set; }
}
