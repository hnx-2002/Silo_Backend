using System; 
using TPGeometryPro;

namespace PTools_PSilo;

/// <summary>
/// 放置操作
/// </summary> 
public class SiloPlacement
{
    /// <summary>
    /// Id{MenuBase:任务结果}
    /// </summary> 
    public Guid Id { get; set; }

    /// <summary>
    /// 排序{Frontend:Number}
    /// </summary> 
    public int Sort { get; set; }

    /// <summary>
    /// 族id{Search}
    /// </summary>
    public Guid Rfa_resource_id { get; set; }

    /// <summary>
    /// 布置标题{Search}
    /// </summary> 
    public string Layout_title { get; set; }

    /// <summary>
    /// 族地址
    /// </summary>
    public string Rfa_path { get; set; }

    /// <summary>
    /// 族模板的基点
    /// </summary>
    public XYZ TemplatePoint { get; set; }

    /// <summary>
    /// 布置类型{Frontend:Select(放置:放置,旋转:旋转,镜像:镜像)}
    /// </summary> 
    public string Layout_type { get; set; }

    /// <summary>
    /// 计算的点
    /// </summary>
    public XYZ Location { get; set; }

    /// <summary>
    /// 计算的法向量
    /// </summary>
    public XYZ Normal { get; set; }

    /// <summary>
    /// 旋转角度 
    /// </summary> 
    public decimal Rotate_angle { get; set; }


}
