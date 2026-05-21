using System;

namespace TPGeometryPro;

/// <summary>
/// XYZ相关方法
/// </summary>
public static class FunXYZ
{
    /// <summary>
    /// 判断两个向量是否平行
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool IsParallel(this XYZ v1, XYZ v2)
    {
        var crossProduct = v1.CrossProduct(v2);
        return crossProduct.IsAlmostEqualTo(XYZ.Zero);
    }

    /// <summary>
    /// 判断两个向量是否平行
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="tolerance">用于内方法的判等</param>
    /// <returns></returns>
    public static bool IsParallel(this XYZ v1, XYZ v2, double tolerance)
    {
        var crossProduct = v1.CrossProduct(v2);
        return crossProduct.IsAlmostEqualTo(XYZ.Zero, tolerance);
    }

    /// <summary>
    /// 判断三点是否共线
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <param name="point3"></param>
    /// <returns></returns>
    public static bool AreCollinear(XYZ point1, XYZ point2, XYZ point3)
    {
        XYZ v1 = point2 - point1;
        XYZ v2 = point3 - point2;

        // 检查向量是否为零向量（这里假设零向量与任何向量共线）  
        if (v1.X == 0 && v1.Y == 0 && v1.Z == 0) return true;
        if (v2.X == 0 && v2.Y == 0 && v2.Z == 0) return true;

        // 计算叉积（在三维空间中，两个向量叉积的模长为零当且仅当这两个向量共线）  
        double crossX = v1.Y * v2.Z - v1.Z * v2.Y;
        double crossY = v1.Z * v2.X - v1.X * v2.Z;
        double crossZ = v1.X * v2.Y - v1.Y * v2.X;

        // 如果叉积的模长（长度平方）非常接近于零，则认为两个向量共线  
        double crossLengthSquared = crossX * crossX + crossY * crossY + crossZ * crossZ;

        return crossLengthSquared < Config.TOLERANCE;
    }

    /// <summary>
    /// 获取平面上XYZ点所对应的UV坐标
    /// </summary>
    /// <param name="point">平面外一点</param>
    /// <param name="planeOrigin">平面原点</param>
    /// <param name="planeBasis1">平面基础向量1</param>
    /// <param name="planeBasis2">平面基础向量2</param>
    /// <returns></returns>
    public static UV GetUV(this XYZ point, XYZ planeOrigin, XYZ planeBasis1, XYZ planeBasis2)
    {
        var vec = point - planeOrigin;
        double ulen = vec.DotProduct(planeBasis1);
        double vlen = vec.DotProduct(planeBasis2);
        return UV.New(ulen, vlen);
    }

    /// <summary>
    /// 获取平面上UV点所对应的XYZ坐标
    /// </summary>
    /// <param name="point"></param>
    /// <param name="planeOrigin"></param>
    /// <param name="planeBasis1"></param>
    /// <param name="planeBasis2"></param>
    /// <returns></returns>
    public static XYZ GetXYZ(this UV point, XYZ planeOrigin, XYZ planeBasis1, XYZ planeBasis2)
    {
        var newVec1 = planeBasis1 * point.U;
        var newVec2 = planeBasis2 * point.V;
        var newPoint = newVec1 + newVec2;

        return XYZ.New(
            planeOrigin.X + newPoint.X,
            planeOrigin.Y + newPoint.Y,
            planeOrigin.Z + newPoint.Z
         );

    }

    /// <summary>
    /// 已知共圆三点，求半径
    /// </summary>
    /// <param name="startPoint">起点</param>
    /// <param name="endPoint">终点</param>
    /// <param name="pointOnArc">弧上一点</param>
    /// <returns></returns>
    public static (double Radius, XYZ Center) CalArcRadius(XYZ startPoint, XYZ endPoint, XYZ pointOnArc)
    {
        try
        {
            double a1, b1, c1, d1;
            double a2, b2, c2, d2;
            double a3, b3, c3, d3;

            double x1 = startPoint.X;
            double y1 = startPoint.Y;
            double z1 = startPoint.Z;
            double x2 = pointOnArc.X;
            double y2 = pointOnArc.Y;
            double z2 = pointOnArc.Z;
            double x3 = endPoint.X;
            double y3 = endPoint.Y;
            double z3 = endPoint.Z;

            a1 = (y1 * z2 - y2 * z1 - y1 * z3 + y3 * z1 + y2 * z3 - y3 * z2);
            b1 = -(x1 * z2 - x2 * z1 - x1 * z3 + x3 * z1 + x2 * z3 - x3 * z2);
            c1 = (x1 * y2 - x2 * y1 - x1 * y3 + x3 * y1 + x2 * y3 - x3 * y2);
            d1 = -(x1 * y2 * z3 - x1 * y3 * z2 - x2 * y1 * z3 + x2 * y3 * z1 +
                x3 * y1 * z2 - x3 * y2 * z1);

            a2 = 2 * (x2 - x1);
            b2 = 2 * (y2 - y1);
            c2 = 2 * (z2 - z1);
            d2 = x1 * x1 + y1 * y1 + z1 * z1 - x2 * x2 - y2 * y2 - z2 * z2;

            a3 = 2 * (x3 - x1);
            b3 = 2 * (y3 - y1);
            c3 = 2 * (z3 - z1);
            d3 = x1 * x1 + y1 * y1 + z1 * z1 - x3 * x3 - y3 * y3 - z3 * z3;

            double x, y, z;
            x = -(b1 * c2 * d3 - b1 * c3 * d2 - b2 * c1 * d3 + b2 * c3 * d1 + b3 * c1 * d2 - b3 * c2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);
            y = (a1 * c2 * d3 - a1 * c3 * d2 - a2 * c1 * d3 + a2 * c3 * d1 + a3 * c1 * d2 - a3 * c2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);
            z = -(a1 * b2 * d3 - a1 * b3 * d2 - a2 * b1 * d3 + a2 * b3 * d1 + a3 * b1 * d2 - a3 * b2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);

            double r = Math.Sqrt(
                (x1 - x) * (x1 - x) +
                (y1 - y) * (y1 - y) +
                (z1 - z) * (z1 - z));

            return (r, XYZ.New(x, y, z));

        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 判断v0向量与，p1到p2的向量，是否同向
    /// </summary>
    /// <param name="v0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static bool IsSameDirection(this XYZ v0, XYZ p1, XYZ p2)
    {
        var v1 = (p2 - p1).Normalize();
        var v2 = v0.Normalize();
        var res = v1.IsAlmostEqualTo(v2);
        return res;
    }

    /// <summary>
    /// 判断v1向量与v2向量，是否同向
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool IsSameDirection(this XYZ v1, XYZ v2)
    {
        var v1Normal = v1.Normalize();
        var v2Normal = v2.Normalize();
        return v1Normal.IsAlmostEqualTo(v2Normal);
    }

    /// <summary>
    /// 判断是否共线
    /// </summary>
    /// <param name="PA">点A</param>
    /// <param name="VA">点A出发的向量</param>
    /// <param name="PB">点B</param>
    /// <param name="VB">点B出发的向量</param>
    /// <returns></returns>
    public static bool IsCollinear(XYZ PA, XYZ VA, XYZ PB, XYZ VB)
    {
        var VAB = PB - PA;
        return IsParallel(VA, VB) &&
               IsParallel(VAB, VA) &&
               IsParallel(VAB, VB);
    }

    /// <summary>
    /// 判断是否共面
    /// </summary>
    /// <param name="origin1">点1</param>
    /// <param name="normal1">法向量1</param>
    /// <param name="origin2">点2</param>
    /// <param name="normal2">法向量2</param>
    /// <returns></returns>
    public static bool IsCoplanar(
        XYZ origin1, XYZ normal1,
        XYZ origin2, XYZ normal2)
    {
        // 向量 u = direction1
        double uX = normal1.X;
        double uY = normal1.Y;
        double uZ = normal1.Z;

        // 向量 v = direction2
        double vX = normal2.X;
        double vY = normal2.Y;
        double vZ = normal2.Z;

        // 向量 w = point2 - point1
        double wX = origin2.X - origin1.X;
        double wY = origin2.Y - origin1.Y;
        double wZ = origin2.Z - origin1.Z;

        // 计算混合积（标量三重积）
        double mixedProduct =
            uX * (vY * wZ - vZ * wY) -
            uY * (vX * wZ - vZ * wX) +
            uZ * (vX * wY - vY * wX);

        // 如果混合积为零，则三个向量共面
        return Math.Abs(mixedProduct) < Config.TOLERANCE;
    }
}
