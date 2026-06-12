using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 点的矩阵变换操作
/// </summary>
public static class PointTransformOperation
{
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="origin">点</param>
    /// <param name="vector">方向</param>
    /// <param name="length">距离</param>
    /// <returns></returns>
    public static XYZ Move(this XYZ origin, XYZ vector, double length)
    {
        double newX = origin.X + vector.X * length;
        double newY = origin.Y + vector.Y * length;
        double newZ = origin.Z + vector.Z * length;

        return XYZ.New(newX, newY, newZ);
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="origins">点集合</param>
    /// <param name="vector">方向</param>
    /// <param name="length">距离</param>
    /// <returns></returns>
    public static List<XYZ> Move(this List<XYZ> origins, XYZ vector, double length)
    {
        var resPoints = new List<XYZ>();
        foreach (var origin in origins)
        {
            double newX = origin.X + vector.X * length;
            double newY = origin.Y + vector.Y * length;
            double newZ = origin.Z + vector.Z * length;
            resPoints.Add(XYZ.New(newX, newY, newZ));
        }
        return resPoints;
    }

    /// <summary>
    /// 点绕旋转轴，得到新的点 
    /// </summary>
    /// <param name="point">要操作的点</param>
    /// <param name="rotationAxis">旋转轴</param>
    /// <param name="angle">弧度制角度</param>
    /// <returns></returns>
    public static XYZ Rotate(this XYZ point, XYZ rotationAxis, double angle)
    {
        return point * Math.Cos(angle) +
            rotationAxis.CrossProduct(point) * Math.Sin(angle) +
            rotationAxis.DotProduct(point) * rotationAxis * (1 - Math.Cos(angle));
    }

    /// <summary>
    /// 旋转向量v绕axis轴angle弧度
    /// </summary>
    /// <param name="v"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static XYZ RotateAroundAxis(this XYZ v, XYZ axis, double angle)
    {
        // 确保axis是单位向量
        XYZ axisNorm = axis.Normalize();

        // 计算旋转矩阵的组成部分
        double cosTheta = Math.Cos(angle);
        double sinTheta = Math.Sin(angle);
        double oneMinusCosTheta = 1.0 - cosTheta;

        // 旋转矩阵的元素
        double xx = axisNorm.X * axisNorm.X * oneMinusCosTheta + cosTheta;
        double yy = axisNorm.Y * axisNorm.Y * oneMinusCosTheta + cosTheta;
        double zz = axisNorm.Z * axisNorm.Z * oneMinusCosTheta + cosTheta;
        double xy = axisNorm.X * axisNorm.Y * oneMinusCosTheta - axisNorm.Z * sinTheta;
        double xz = axisNorm.X * axisNorm.Z * oneMinusCosTheta + axisNorm.Y * sinTheta;
        double yz = axisNorm.Y * axisNorm.Z * oneMinusCosTheta - axisNorm.X * sinTheta;

        // 应用旋转矩阵到向量v
        XYZ rotatedV = XYZ.New(
            v.X * xx + v.Y * xy + v.Z * xz,
            v.X * xy + v.Y * yy + v.Z * yz,
            v.X * xz + v.Y * yz + v.Z * zz
        ).Normalize();

        return rotatedV;
    }


    /// <summary>
    /// 点关于平面的镜像
    /// </summary>
    /// <param name="p">点</param>
    /// <param name="n">平面的单位法向量</param>
    /// <param name="o">平面上一点</param>
    /// <returns></returns>
    public static XYZ MirrorPoint(this XYZ p, XYZ n, XYZ o)
    {
        double d = (p - o).DotProduct(n);   // 有符号距离
        return p - 2 * d * n;
    }


    /// <summary>
    /// 点集合关于平面的镜像
    /// </summary>
    /// <param name="ps">点集合</param>
    /// <param name="n">平面的单位法向量</param>
    /// <param name="o">平面上一点</param>
    /// <returns></returns>
    public static List<XYZ> MirrorPoint(this List<XYZ> ps, XYZ n, XYZ o)
    {
        var resPoints = new List<XYZ>();
        foreach (var p in ps)
        {
            double d = (p - o).DotProduct(n);   // 有符号距离
            var rewPoint = p - 2 * d * n;
            resPoints.Add(rewPoint);
        }
        return resPoints;
    }
}
