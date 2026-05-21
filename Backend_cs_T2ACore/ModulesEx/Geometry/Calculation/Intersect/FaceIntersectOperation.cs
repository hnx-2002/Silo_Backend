using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 面相关求交点
/// </summary>
public class FaceIntersectOperation
{
    /// <summary>
    /// 计算圆锥面与空间射线交点
    /// </summary>
    /// <param name="inOrigin">圆锥面顶点</param>
    /// <param name="inAxisDirection">圆锥轴向量</param>
    /// <param name="inApexAngle">圆锥母线与轴线的夹角</param>
    /// <param name="inOtherPoint">空间射线原点</param>
    /// <param name="inOtherDirection">空间射线方向向量</param>
    /// <returns>交点列表</returns>
    public static XYZ GetIntersectionsConeWithLine(
        XYZ inOrigin, XYZ inAxisDirection, double inApexAngle,
        XYZ inOtherPoint, XYZ inOtherDirection)
    {
        //lxl路径优化(业务) 排除圆锥变圆
        //夹角90°，形成圆，获得90度弯头
        if (Math.Abs(inApexAngle - 0.5 * Math.PI) < 0.1.ToRadian())
        {
            return null;
        }
        double tanTheta = Math.Tan(inApexAngle);
        double tanThetaSquared = tanTheta * tanTheta;

        // 归一化向量（标准化方向向量）
        // 空间射线方向向量标准化
        XYZ d = inOtherDirection.Normalize();
        // 圆锥方向向量标准化
        XYZ v = inAxisDirection.Normalize();

        XYZ co = inOtherPoint - inOrigin;

        double dDotV = d.DotProduct(v);
        double coDotV = co.DotProduct(v);

        XYZ dPerpendicular = d - dDotV * v;
        XYZ coPerpendicular = co - coDotV * v;

        // 建立二次方程参数
        double a = dPerpendicular.DotProduct(dPerpendicular) - (double)tanThetaSquared * dDotV * dDotV;
        double b = 2 * (dPerpendicular.DotProduct(coPerpendicular) - (double)tanThetaSquared * dDotV * coDotV);
        double c = coPerpendicular.DotProduct(coPerpendicular) - (double)tanThetaSquared * coDotV * coDotV;

        // 求解二次方程
        double discriminant = b * b - 4 * a * c;
        List<XYZ> intersections = [];

        if (discriminant < 0)
        {
            // 无实数解，即无交点
            return null; // 无交点
        }
        else
        {
            double sqrtDiscriminant = (double)Math.Sqrt(discriminant);
            // 计算两个可能的交点
            double t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

            // 检查交点是否有效（非负）
            if (t1 >= 0)
            {
                XYZ resPoint1 = inOtherPoint + d * t1;
                intersections.Add(resPoint1);
            }

            if (t2 >= 0)
            {
                XYZ resPoint2 = inOtherPoint + d * t2;
                intersections.Add(resPoint2);
            }

            if (intersections.Count == 0)
            {
                return null;
            }
            else if (intersections.Count == 1)
            {
                return intersections[0];
            }
            else if (intersections.Count == 2)
            {
                var len1 = intersections[0].DistanceTo(inOtherPoint);
                var len2 = intersections[1].DistanceTo(inOtherPoint);

                if (len1 <= len2)
                {
                    return intersections[0];
                }
                else
                {
                    return intersections[1];
                }
            }
            else
            {
                return null;
            }
        }
    }


    /// <summary>
    /// 计算射线与包围盒的交点
    /// </summary>
    /// <param name="startPoint">射线起点</param>
    /// <param name="direction">射线方向</param>
    /// <param name="minPoint">包围盒表示的最小点</param>
    /// <param name="maxPoint">包围盒表示的最大点</param>
    /// <param name="maxLength">交点的长度</param>
    /// <returns></returns>
    public static bool RayIntersectsCube(
        XYZ startPoint, XYZ direction,
        XYZ minPoint, XYZ maxPoint, out double maxLength)
    {
        // 立方体的边界值
        double xMin = minPoint.X, xMax = maxPoint.X;
        double yMin = minPoint.Y, yMax = maxPoint.Y;
        double zMin = minPoint.Z, zMax = maxPoint.Z;

        // X方向为0
        if (direction.X == 0)
        {
            // x == 0, y = 0, z != 0
            if (direction.Y == 0 && direction.Z != 0)
            {
                maxLength = Math.Abs(zMax - zMin);
                return true;
            }
            // x == 0, y != 0, z == 0
            else if (direction.Y != 0 && direction.Z == 0)
            {
                maxLength = Math.Abs(yMax - yMin);
                return true;
            }
            // x == 0, y != 0, z != 0
            else
            {
                // Y,Z不为0
                double y = Math.Abs(yMax - yMin);
                double z = Math.Abs(zMax - zMin);
                maxLength = Math.Sqrt(y * y + z * z);
                return true;
            }
        }
        else
        {
            // x != 0, y = 0, z = 0
            if (direction.Y == 0 && direction.Z == 0)
            {
                maxLength = Math.Abs(xMax - xMin);
                return true;
            }

            // x != 0, y != 0, z == 0
            else if (direction.Y != 0 && direction.Z == 0)
            {
                double x = Math.Abs(xMax - xMin);
                double y = Math.Abs(yMax - yMin);
                maxLength = Math.Sqrt(x * x + y * y);
                return true;
            }

            // x != 0, y == 0, z != 0
            else if (direction.Y == 0 && direction.Z != 0)
            {
                // Y,Z不为0
                double x = Math.Abs(xMax - xMin);
                double z = Math.Abs(zMax - zMin);
                maxLength = Math.Sqrt(x * x + z * z);
                return true;
            }
            else
            {
                // X,Y,Z不为0，那就是找交点

                //cyl note:这里还是可能有点问题,tMin的交点需要不需要判断

                // 计算射线与立方体在每个轴上的交点参数 t
                double tXMin = (xMin - startPoint.X) / direction.X;
                double tXMax = (xMax - startPoint.X) / direction.X;

                double tYMin = (yMin - startPoint.Y) / direction.Y;
                double tYMax = (yMax - startPoint.Y) / direction.Y;

                double tZMin = (zMin - startPoint.Z) / direction.Z;
                double tZMax = (zMax - startPoint.Z) / direction.Z;

                // 判断最小和最大交点 t
                double tMin = Math.Max(Math.Max(Math.Min(tXMin, tXMax), Math.Min(tYMin, tYMax)), Math.Min(tZMin, tZMax));
                double tMax = Math.Min(Math.Min(Math.Max(tXMin, tXMax), Math.Max(tYMin, tYMax)), Math.Max(tZMin, tZMax));

                // 计算射线与立方体的交点坐标
                double intersectX = tMax * direction.X;  // 用 tMax 计算交点
                double intersectY = tMax * direction.Y;
                double intersectZ = tMax * direction.Z;

                // 计算起点与交点之间的距离
                maxLength = Math.Sqrt(intersectX * intersectX + intersectY * intersectY + intersectZ * intersectZ);
                return true;
            }
        }
    }

    /// <summary>
    /// 使用重心法判断一条直线是否与一个三角面片相交
    /// 如果相交则返回交点坐标，若不相交，返回null
    /// </summary>
    /// <param name="lineStartPoint">直线起点</param>
    /// <param name="lineEndPoint">直线终点</param>
    /// <param name="vertexA">三角面片顶点A</param>
    /// <param name="vertexB">三角面片顶点B</param>
    /// <param name="vertexC">三角面片顶点C</param>
    /// <returns></returns>
    public static XYZ LineIsIntersectTriangle(
        XYZ lineStartPoint, XYZ lineEndPoint,
        XYZ vertexA, XYZ vertexB, XYZ vertexC)
    {
        XYZ edge1 = vertexB - vertexA;
        XYZ edge2 = vertexC - vertexA;
        XYZ lineDirection = lineEndPoint - lineStartPoint;
        XYZ normal = edge1.CrossProduct(edge2);

        double denominator = normal.DotProduct(lineDirection);

        // 判断直线是否与平面平行
        if (Math.Abs(denominator) < 1e-6)
            return null;

        // 计算交点参数 t
        XYZ vectorAStart = vertexA - lineStartPoint;
        double numerator = normal.DotProduct(vectorAStart);
        double t = numerator / denominator;

        // 判断交点是否在线段范围内
        if (t < 0 || t > 1)
            return null;

        // 计算交点坐标
        XYZ intersection = lineStartPoint + lineDirection * t;

        // 检查交点是否在三角形内部
        if (intersection.IsPointInsideTriangle(vertexA, vertexB, vertexC))
            return intersection;

        return null;
    }
}
