using System;

namespace PTools_PSilo;

/// <summary>
/// 筒仓模板族放置计算结果
/// </summary>
public class SiloTemplatePlacementResult
{
    /// <summary>
    /// 库型模板Id
    /// </summary>
    public Guid Template_silo_id { get; set; }

    /// <summary>
    /// 族类型名
    /// </summary>
    public string Symbol_name { get; set; }

    /// <summary>
    /// 族文件地址
    /// </summary>
    public string Rfa_path { get; set; }

    /// <summary>
    /// Revit内部X坐标
    /// </summary>
    public double Location_x { get; set; }

    /// <summary>
    /// Revit内部Y坐标
    /// </summary>
    public double Location_y { get; set; }

    /// <summary>
    /// Revit内部Z坐标
    /// </summary>
    public double Location_z { get; set; }

    /// <summary>
    /// Revit内部旋转角度
    /// </summary>
    public double Rotate_angle { get; set; }
}
