using System;
using System.Collections.Generic;

namespace TPGeometryPro;

/// <summary>
/// 线相交相关方法 
/// </summary>
public static class CurveIntersectOperation
{
    /// <summary>
    /// 直线与直线相交，返回交点集合
    /// </summary>
    /// <param name="source"></param>
    /// <param name="line"></param>
    /// <param name="intersectPoints"></param>
    /// <returns></returns>
    public static SetComparisonResult LineIntersectLine(this Line source, Line line, out List<XYZ> intersectPoints)
    {
        intersectPoints = new List<XYZ>();
        //构造线段参数方程所需要的参数
        //线段1： p0+s*d0
        //线段2： p1+t*d1
        //p0 p1为线段端点，d0 d1为起点指向终点的方向向量，对于线段，s和t的取值范围为[0,1]

        XYZ p0 = source.StartPoint;
        XYZ d0 = source.EndPoint - source.StartPoint;
        XYZ p1 = line.StartPoint;
        XYZ d1 = line.EndPoint - line.StartPoint;

        XYZ e = p1 - p0;
        //e的长度接近零向量，将其置为零向量
        if (e.IsZeroLength())
        {
            e = XYZ.Zero;
        }
        double kross = d0.CrossProduct(d1).GetLength();
        double sqrKross = kross * kross;
        double sqrLen0 = Math.Pow(d0.X, 2) + Math.Pow(d0.Y, 2) + Math.Pow(d0.Z, 2);
        double sqrLen1 = Math.Pow(d1.X, 2) + Math.Pow(d1.Y, 2) + Math.Pow(d1.Z, 2);
        double sqrEpsilon = Config.TOLERANCE * Config.TOLERANCE;

        //线段不平行
        if (sqrKross > sqrEpsilon * sqrLen0 * sqrLen1)
        {
            XYZ kross_Norm = d0.CrossProduct(d1).Normalize();
            XYZ ed1_Norm = e.CrossProduct(d1).Normalize();
            XYZ ed0_Norm = e.CrossProduct(d0).Normalize();

            double s;
            double t;
            if (kross_Norm.IsAlmostEqualTo(ed1_Norm))
            {
                s = e.CrossProduct(d1).GetLength() / kross;
            }
            else
            {
                s = -e.CrossProduct(d1).GetLength() / kross;
            }

            if (s > 1 + Config.TOLERANCE || s < 0 - Config.TOLERANCE)
            {
                //交点不在线段1上
                return SetComparisonResult.Disjoint;
            }

            if (kross_Norm.IsAlmostEqualTo(ed0_Norm))
            {
                t = e.CrossProduct(d0).GetLength() / kross;
            }
            else
            {
                t = -e.CrossProduct(d0).GetLength() / kross;
            }

            if (t > 1 + Config.TOLERANCE || t < 0 - Config.TOLERANCE)
            {
                //交点不在线段2上
                return SetComparisonResult.Disjoint;
            }
            XYZ p = p0 + s * d0;

            if (p.IsPointOnLine(source) &&
                p.IsPointOnLine(line))
            {
                intersectPoints.Add(p);
                return SetComparisonResult.Overlap;
            }
            else
            {
                return SetComparisonResult.Disjoint;
            }
        }
        //线段平行，需要判断是否重合
        else
        {
            double sqrLenE = Math.Pow(e.X, 2) + Math.Pow(e.Y, 2) + Math.Pow(e.Z, 2);
            kross = e.CrossProduct(d0).GetLength();
            if (kross.IsAlmostEqualTo(0))
            {
                kross = 0;
            }

            sqrKross = kross * kross;
            if (sqrKross > sqrEpsilon * sqrLen0 * sqrLenE)
            {
                return SetComparisonResult.Disjoint;
            }

            double s0 = d0.DotProduct(e) / sqrLen0;
            double s1 = s0 + d0.DotProduct(d1) / sqrLen0;

            double sMin = Math.Min(s0, s1);
            double sMax = Math.Max(s0, s1);

            int result = FindIntersectionByInterval(0, 1, sMin, sMax, out double[] valueArr);

            for (int i = 0; i < result; i++)
            {
                intersectPoints.Add(p0 + valueArr[i] * d0);
            }
            if (result == 1)
            {
                return SetComparisonResult.Subset;
            }
            if (result == 2 &&
                ((!line.IsBound && source.IsBound) ||
                 (line.IsBound && !source.IsBound)))
            {
                return SetComparisonResult.Subset;
            }
            return (SetComparisonResult)result;
        }
    }

    /// <summary>
    /// 曲线与直线相交，返回交点集合
    /// </summary>
    /// <param name="source"></param>
    /// <param name="line"></param>
    /// <param name="intersectPoints"></param>
    /// <returns></returns>
    public static SetComparisonResult ArcIntersectLine(this Arc source, Line line, out List<XYZ> intersectPoints)
    {
        intersectPoints = new List<XYZ>();

        //判断圆心到直线的距离，如果距离大于半径，肯定无交点
        var cOnLine = source.Center.ProjectToLine(line, out double distance);

        if (distance > source.Radius + Config.TOLERANCE)
        {
            return SetComparisonResult.Disjoint;
        }

        //line的方程 p+t*d
        XYZ p = line.StartPoint;
        XYZ d = line.EndPoint - line.StartPoint;

        //定义Δ为p-Center
        XYZ delta_Upper = p - source.Center;

        //求解圆方程和直线相交联立的二次方程
        //二次方程根的数量
        //定义δ，判断δ是否大于0
        double delta_Lower = Math.Pow(d.DotProduct(delta_Upper), 2) -
            d.GetLength() * d.GetLength() *
            (delta_Upper.GetLength() * delta_Upper.GetLength() - source.Radius * source.Radius);

        if (delta_Lower < 0)  //方程无根，无交点
        {
            return SetComparisonResult.Disjoint;
        }

        //将方程的根求出，判断得到的点是否在圆弧上
        List<XYZ> results_Equation = new List<XYZ>();

        if (Math.Abs(delta_Lower) < Config.TOLERANCE)  //有一根
        {
            double t = (-d.DotProduct(delta_Upper)) / Math.Pow(d.GetLength(), 2);

            //需要判断t的取值范围是否在0~1范围内
            if (t < -Config.TOLERANCE || t > 1)
            {
                return SetComparisonResult.Disjoint;
            }

            XYZ result = p + t * d;
            results_Equation.Add(result);
        }
        else  //有2个根
        {
            double t1 = (-d.DotProduct(delta_Upper) + Math.Sqrt(delta_Lower)) / Math.Pow(d.GetLength(), 2);
            double t2 = (-d.DotProduct(delta_Upper) - Math.Sqrt(delta_Lower)) / Math.Pow(d.GetLength(), 2);

            if (t1 > -Config.TOLERANCE && t1 < 1 + Config.TOLERANCE)
            {
                XYZ result1 = p + t1 * d;
                results_Equation.Add(result1);
            }
            if (t2 > -Config.TOLERANCE && t2 < 1 + Config.TOLERANCE)
            {
                XYZ result2 = p + t2 * d;
                results_Equation.Add(result2);
            }
        }

        //判断一下方程的根是否都在弧线上
        foreach (XYZ result in results_Equation)
        {
            if (result.IsPointOnArc(source))
            {
                intersectPoints.Add(result);
            }
        }

        if (intersectPoints.Count > 0)
        {
            return SetComparisonResult.Overlap;
        }
        else
        {
            return SetComparisonResult.Disjoint;
        }
    }

    /// <summary>
    /// 曲线与曲线相交，返回交点集合
    /// </summary>
    /// <param name="source"></param>
    /// <param name="arc"></param>
    /// <param name="intersectPoints"></param>
    /// <returns></returns>
    public static SetComparisonResult ArcIntersectArc(this Arc source, Arc arc, out List<XYZ> intersectPoints)
    {
        intersectPoints = new List<XYZ>();

        //首先判断两个圆是否完全一样（半径，圆心）
        XYZ ct1 = source.Center;
        XYZ ct2 = arc.Center;
        double r1 = source.Radius;
        double r2 = arc.Radius;

        if (ct1.IsAlmostEqualTo(ct2) &&
            r1.IsAlmostEqualTo(r2))
        {
            if (!source.StartPoint.IsPointOnArc(arc) &&
                !source.EndPoint.IsPointOnArc(arc))
            {
                return SetComparisonResult.Disjoint;
            }
            else
            {
                return SetComparisonResult.Equal;  //圆弧有重叠，等于交点无数个，不返回交点值
            }
        }

        double a1 = ct1.X;
        double b1 = ct1.Y;
        double c1 = ct1.Z;

        double a2 = ct2.X;
        double b2 = ct2.Y;
        double c2 = ct2.Z;

        double dx = a2 - a1;
        double dy = b2 - b1;
        double dz = c2 - c1;

        double dis_Ct = dx * dx + dy * dy + dz * dz;

        if (dis_Ct > Math.Pow((r1 + r2), 2) || dis_Ct < Math.Pow((r1 - r2), 2))
        {
            return SetComparisonResult.Disjoint;
        }

        double t = Math.Atan2(dy, dx);
        double a = Math.Acos((r1 * r1 - r2 * r2 + dis_Ct) / (2 * r1 * Math.Sqrt(dis_Ct)));

        double x3 = a1 + r1 * Math.Cos(t + a);
        double y3 = b1 + r1 * Math.Sin(t + a);
        double z3_Squr = r1 * r1 - (x3 - a1) * (x3 - a1) - (y3 - b1) * (y3 - b1);

        double z3, z4;
        if (Math.Abs(z3_Squr).IsAlmostEqualTo(0))
        {
            z3 = -c1;
        }
        else
        {
            z3 = Math.Sqrt(z3_Squr) - c1;
        }

        double x4 = a1 + r1 * Math.Cos(t - a);
        double y4 = b1 + r1 * Math.Sin(t - a);
        double z4_Squr = r2 * r2 - (x4 - a2) * (x4 - a2) - (y4 - b2) * (y4 - b2);

        if (Math.Abs(z4_Squr).IsAlmostEqualTo(0))
        {
            z4 = -c2;
        }
        else
        {
            z4 = Math.Sqrt(z4_Squr) - c2;
        }

        if (Math.Abs(a) < Config.TOLERANCE)
        {
            XYZ point_Prob = XYZ.New(x3, y3, z3);
            if (point_Prob.IsPointOnArc(source) &&
                point_Prob.IsPointOnArc(arc))
            {
                intersectPoints.Add(point_Prob);
            }
            if (intersectPoints.Count == 0)
            {
                return SetComparisonResult.Disjoint;
            }
            else
            {
                return SetComparisonResult.Overlap;
            }
        }
        else
        {
            XYZ point1_Prob = XYZ.New(x3, y3, z3);
            XYZ point2_Prob = XYZ.New(x4, y4, z4);

            if (point1_Prob.IsPointOnArc(source) &&
                point1_Prob.IsPointOnArc(arc))
            {
                intersectPoints.Add(point1_Prob);
            }

            if (point2_Prob.IsPointOnArc(source) &&
                point2_Prob.IsPointOnArc(arc))
            {
                intersectPoints.Add(point2_Prob);
            }

            if (intersectPoints.Count > 1)
            {
                if (intersectPoints[0].IsAlmostEqualTo(intersectPoints[1]))
                {
                    intersectPoints.RemoveAt(1);
                }
            }

            if (intersectPoints.Count == 0)
            {
                return SetComparisonResult.Disjoint;
            }
            else
            {
                return SetComparisonResult.Overlap;
            }
        }
    }


    /// <summary>
    /// 计算两个区间[u0,u1]和[v0,v1]相交 
    /// GKX实现
    /// </summary>
    /// <param name="u0"></param>
    /// <param name="u1"></param>
    /// <param name="v0"></param>
    /// <param name="v1"></param>
    /// <param name="valueArr"></param>
    /// <returns>不相交返回值0，相交于一个交点，返回值1，如果相交于一个区间，返回值为2，保存区间的端点</returns> 
    internal static int FindIntersectionByInterval(double u0, double u1, double v0, double v1, out double[] valueArr)
    {
        valueArr = new double[2];
        if (u1 < v0 || u0 > v1)
        {
            return 0;
        }
        if (u1 > v0)
        {
            if (u0 < v1)
            {
                if (u0 < v0)
                {
                    valueArr[0] = v0;
                }
                else
                {
                    valueArr[0] = u0;
                }
                if (u1 > v1)
                {
                    valueArr[1] = v1;
                }
                else
                {
                    valueArr[1] = u1;
                }
                return 2;
            }
            else
            {
                valueArr[0] = u0;
                return 1;
            }
        }
        else
        {
            valueArr[0] = u1;
            return 1;
        }
    }

    /// <summary>
    /// 寻找两条射线的交点
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="direction1"></param>
    /// <param name="point2"></param>
    /// <param name="direction2"></param>
    /// <returns></returns>
    public static XYZ GetRayIntersection(
        XYZ point1, XYZ direction1,
        XYZ point2, XYZ direction2)
    {
        var end1 = point1.Move(direction1, 100); //假设100
        var end2 = point2.Move(direction2, 100); //假设100

        var line1 = Line.Create(point1, end1);
        var line2 = Line.Create(point2, end2);

        var interType = line1.LineIntersectLine(line2, out List<XYZ> resArr);

        if (interType == SetComparisonResult.Overlap)
        {
            var interPoint = resArr[0];
            return interPoint;
        }
        return null;
    }

    /// <summary>
    /// 寻找两条线段的交点
    /// </summary>
    /// <param name="line1point1"></param>
    /// <param name="line1point2"></param>
    /// <param name="line2point1"></param>
    /// <param name="line2point2"></param>
    /// <returns></returns>
    public static XYZ GetLineIntersection(
        XYZ line1point1, XYZ line1point2,
        XYZ line2point1, XYZ line2point2)
    {
        try
        {
            var line1 = Line.Create(line1point1, line1point2);
            var line2 = Line.Create(line2point1, line2point2);

            var interType = line1.LineIntersectLine(line2, out List<XYZ> resArr);

            if (interType == SetComparisonResult.Overlap)
            {
                var interPoint = resArr[0];
                return interPoint;
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

}
