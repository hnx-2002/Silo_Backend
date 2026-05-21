using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

internal class DBDiffLog
{
    /// <summary>
    /// 主键
    /// </summary> 
    public long Id { get; set; }

    /// <summary>
    /// 差异数据
    /// </summary> 
    public string DiffData { get; set; }

    /// <summary>
    /// Sql
    /// </summary> 
    public string Sql { get; set; }

    /// <summary>
    /// 参数（手动传入的参数）
    /// </summary> 
    public string Parameters { get; set; }

    /// <summary>
    /// 业务对象
    /// </summary> 
    public string BusinessData { get; set; }

    /// <summary>
    /// 差异操作
    /// </summary> 
    public string DiffType { get; set; }

    /// <summary>
    /// 耗时
    /// </summary> 
    public long? Elapsed { get; set; }
}
