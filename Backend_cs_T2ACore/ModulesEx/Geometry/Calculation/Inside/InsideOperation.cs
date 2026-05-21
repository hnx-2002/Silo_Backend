using System;

namespace TPGeometryPro;

/// <summary>
/// 判断是否在范围内
/// </summary>
public static class InsideOperation
{
    /// <summary>
    /// 判断点在线段内
    /// 点在线段的起点和终点同样认为是在线段上 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool IsPointOnLine(this XYZ point, Line line)
    {
        var v0 = line.EndPoint - line.StartPoint;
        var v1 = point - line.StartPoint;
        var v2 = point - line.EndPoint;
        var parallelTest = v0.IsParallel(v1) &&
                           v0.IsParallel(v2) &&
                           v1.IsParallel(v2);

        var d0 = line.EndPoint.DistanceTo(line.StartPoint);
        var d1 = point.DistanceTo(line.StartPoint);
        var d2 = point.DistanceTo(line.EndPoint);

        var lenTest = Math.Abs(d0 - d1 - d2) < Config.TOLERANCE;

        return parallelTest && lenTest;
    }

    /// <summary>
    /// 判断点是否在射线内 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="ray"></param>
    /// <returns></returns>
    public static bool IsPointOnRay(this XYZ point, Ray ray)
    {
        XYZ dirToStart = (ray.Origin - point).Normalize();
        if (dirToStart.IsZeroLength())//如果点在线段的起点
        {
            return true;
        }
        if (dirToStart.IsAlmostEqualTo(ray.Direction.Negate()))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判断交点是否在圆弧上 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="arc"></param>
    /// <returns></returns>
    public static bool IsPointOnArc(this XYZ point, Arc arc)
    {
        Plane plane = Plane.CreateByNormalAndOrigin(arc.Normal, arc.Center);
        XYZ proj = point.GetProjectedPointOnPlane(plane);
        double distance_Proj = proj.DistanceTo(point);

        // 检查点是否在圆弧的平面上
        if (!distance_Proj.IsAlmostEqualTo(0))
        {
            return false;
        }

        // 检查点是否在圆弧所在的圆上（距离圆心等于半径）
        double distanceToCenter = point.DistanceTo(arc.Center);
        if (!distanceToCenter.IsAlmostEqualTo(arc.Radius))
        {
            return false;
        }

        // 如果是完整圆（未绑定），直接返回true
        if (!arc.IsBound)
        {
            return true;
        }

        // 否则，检查点是否在起点和终点之间的弧段上
        XYZ startPoint = arc.GetEndPoint(0);
        XYZ endPoint = arc.GetEndPoint(1);

        XYZ v1 = point - startPoint;
        XYZ v2 = endPoint - startPoint;

        // 计算叉乘方向
        XYZ kross = v1.CrossProduct(v2);
        if (kross.IsAlmostEqualTo(XYZ.Zero))
        {
            // 处理零向量情况（点与起点或终点重合，或弧为半圆）
            return true;
        }

        // 判断方向是否与法线方向一致
        return kross.Normalize().IsAlmostEqualTo(arc.Normal.Normalize());
    }


    /// <summary>
    /// 使用重心坐标法判断点是否在三角形内
    /// </summary>
    /// <param name="point"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsPointInsideTriangle(this XYZ point, XYZ a, XYZ b, XYZ c)
    {
        XYZ v0 = c - a;
        XYZ v1 = b - a;
        XYZ v2 = point - a;

        double dot00 = v0.DotProduct(v0);
        double dot01 = v0.DotProduct(v1);
        double dot02 = v0.DotProduct(v2);
        double dot11 = v1.DotProduct(v1);
        double dot12 = v1.DotProduct(v2);

        double invDenominator = 1 / (dot00 * dot11 - dot01 * dot01);
        double u = (dot11 * dot02 - dot01 * dot12) * invDenominator;
        double v = (dot00 * dot12 - dot01 * dot02) * invDenominator;

        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }
}
