namespace TPGeometryPro;

/// <summary>
/// UV方法
/// </summary>
public class FunUV
{
    /// <summary>
    /// 判断平面三点是否共线
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <param name="point3"></param>
    /// <returns></returns>
    public static bool AreCollinear(UV point1, UV point2, UV point3)
    {
        XYZ p1 = XYZ.New(point1.U, point1.V, 0);
        XYZ p2 = XYZ.New(point2.U, point2.V, 0);
        XYZ p3 = XYZ.New(point3.U, point3.V, 0);

        return FunXYZ.AreCollinear(p1, p2, p3);
    }
}
