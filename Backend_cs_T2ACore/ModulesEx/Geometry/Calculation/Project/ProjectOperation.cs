using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 投影操作
/// </summary>
public static class ProjectOperation
{
    /// <summary>
    /// 点向直线做投影
    /// </summary>
    /// <param name="point"></param>
    /// <param name="line"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static XYZ ProjectToLine(this XYZ point, Line line, out double distance)
    {
        distance = 0;

        //var onlintTest = point.IsPointOnLine(line);
        var ray1 = Ray.Create(line.Origin, line.Direction);
        var onRay1Test = point.IsPointOnRay(ray1);
        var ray2 = Ray.Create(line.Origin, -line.Direction);
        var onRay2Test = point.IsPointOnRay(ray2);
        if (onRay1Test || onRay2Test)
        {
            return point;
        }

        XYZ endPoint = line.StartPoint;
        double val = (point - endPoint).DotProduct(line.Direction);
        var pOnLine = endPoint + line.Direction * val;

        distance = point.DistanceTo(pOnLine);

        return pOnLine;
    }
     

    /// <summary>
    /// 已知空间三维点P和三个不共线的空间三维点ABC
    /// 求由P到ABC三个点形成的平面的投影点
    /// </summary>
    /// <param name="pointP"></param>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <param name="pointC"></param>
    /// <returns></returns>
    public static XYZ ProjectToPlane(this XYZ pointP,
        XYZ pointA, XYZ pointB, XYZ pointC)
    {
        // 计算向量AB和AC
        XYZ vectorAB = pointB - pointA;
        XYZ vectorAC = pointC - pointA;

        // 计算平面的法向量N（向量AB和向量AC的叉积）
        XYZ normal = vectorAB.CrossProduct(vectorAC);

        // 计算点P到平面的距离d（点A到点P的向量与法向量的点积，然后除以法向量的模）
        XYZ vectorAP = pointP - pointA;
        var distance = vectorAP.DotProduct(normal) / normal.DotProduct(normal);

        // 计算点P在平面上的正交投影点
        XYZ projection = pointP - normal * distance;

        return projection;
    }

    /// <summary>
    /// 获取点在平面上的投影点 
    /// </summary>
    /// <param name="point">待投影的点</param>
    /// <param name="plane">平面</param>
    /// <returns></returns>
    public static XYZ GetProjectedPointOnPlane(this XYZ point, Plane plane)
    {
        XYZ xVector = plane.XVec;
        XYZ yVector = plane.YVec;
        XYZ origin = plane.Origin;
        double u = (point - origin).DotProduct(xVector);
        double v = (point - origin).DotProduct(yVector);
        return origin.Add(xVector * u).Add(yVector * v);
    }

}
