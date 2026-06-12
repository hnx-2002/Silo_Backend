namespace TPGeometryPro;

/// <summary>
/// 曲线类型
/// </summary>
public enum CurveType
{
    /// <summary>
    /// 直线
    /// </summary>
    Line = 0,

    /// <summary>
    /// 射线
    /// </summary>
    Ray = 1,

    /// <summary>
    /// 弧线
    /// </summary>
    Arc = 2,

    /// <summary>
    /// 椭圆线
    /// </summary>
    Ellipse = 3,

    /// <summary>
    /// 螺旋线
    /// </summary>
    CylindricalHelix = 4,

    /// <summary>
    /// 样条曲线
    /// </summary>
    HermiteSpline = 5,

    /// <summary>
    /// 非均匀有理B样条
    /// </summary>
    NurbSpline = 6
}
