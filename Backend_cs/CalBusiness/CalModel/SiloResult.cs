using MemoryPack;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using TPGeometryPro;

namespace PTools_PSilo;

/// <summary>
/// Silo计算结果
/// </summary> 
public partial class SiloResult
{
    /// <summary>
    /// Id{MenuBase:建模任务}{Option:task_title,id}
    /// </summary> 
    public Guid Id { get; set; }

    /// <summary>
    /// 任务标题{Search}
    /// </summary> 
    public string Task_title { get; set; }

    /// <summary>
    /// 库型名称{Search}{Validate:NotEmpty}
    /// </summary>
    public string Silo_name { get; set; }

    /// <summary>
    /// 储库直径{Frontend:Number}
    /// </summary> 
    public decimal Silo_diameter { get; set; }

    /// <summary>
    /// 库底板高度{Frontend:Number}
    /// </summary> 
    public decimal Silo_height { get; set; }

    /// <summary>
    /// 项目基点
    /// </summary>
    public XYZ TaskBasePoint { get; set; }

    /// <summary>
    /// 旋转角度{Frontend:Number}
    /// </summary> 
    public decimal Rotation_angle { get; set; }

    /// <summary>
    /// 状态{Frontend:Select(10:新建,11:计算中,12:计算成功,13:计算失败,20:错误)}
    /// </summary> 
    public int Status { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary> 
    public string Error_msg { get; set; }

    /// <summary>
    /// 放置操作
    /// </summary>
    public List<SiloPlacement> Placements { get; set; }
}

