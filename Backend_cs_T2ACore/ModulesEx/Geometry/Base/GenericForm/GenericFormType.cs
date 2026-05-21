namespace TPGeometryPro;

/// <summary>
/// 建模实体类型
/// </summary>
public enum GenericFormType
{
    /// <summary>
    /// 拉伸
    /// </summary>
    Extrusion = 0,

    /// <summary>
    /// 旋转
    /// </summary>
    Revolution = 1,

    /// <summary>
    /// 融合
    /// </summary>
    Blend = 2,

    /// <summary>
    /// 放样
    /// </summary>
    Sweep = 3,

    /// <summary>
    /// 放样融合
    /// </summary>
    SweptBlend = 4,

}
