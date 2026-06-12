using System;
using System.Collections.Generic;

namespace TPGeometryPro;

/// <summary>
/// 单位换算
/// </summary>
public static class FunUnit
{
    /// <summary>
    /// 转换为公制
    /// 英尺转为毫米
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static double ToMetric(this double data)
    {
        return data * 304.8;
    }

    /// <summary>
    /// 转换为英制
    /// 毫米转为英尺
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static double ToBritish(this double data)
    {
        return data / 304.8;
    }

    /// <summary>
    /// 转换为公制
    /// 英尺转为毫米
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static double ToMetric(this float data)
    {
        return data * 304.8;
    }

    /// <summary>
    /// 转换为英制
    /// 毫米转为英尺
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static double ToBritish(this float data)
    {
        return data / 304.8;
    }

    /// <summary>
    /// 转为弧度制
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static double ToRadian(this double degree)
    {
        const double radiansPerDegree = Math.PI / 180.0;
        return degree * radiansPerDegree;
    }

    /// <summary>
    /// 转为角度制
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static double ToDegree(this double radian)
    {
        const double degreesPerRadian = 180.0 / Math.PI;
        return radian * degreesPerRadian;
    }

    /// <summary>
    /// 转换为公制
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static XYZ ToMetric(this XYZ point)
    {
        return XYZ.New(
            point.X.ToMetric(),
            point.Y.ToMetric(),
            point.Z.ToMetric());
    }

    /// <summary>
    /// 转换为英制
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static XYZ ToBritish(this XYZ point)
    {
        return XYZ.New(
            point.X.ToBritish(),
            point.Y.ToBritish(),
            point.Z.ToBritish());
    }

    /// <summary>
    /// 转换为公制
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static List<XYZ> ToMetric(this List<XYZ> points)
    {
        var res = new List<XYZ>();
        foreach (var point in points)
        {
            res.Add(point.ToMetric());
        }
        return res;
    }

    /// <summary>
    /// 转换为英制
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static List<XYZ> ToBritish(this List<XYZ> points)
    {
        var res = new List<XYZ>();
        foreach (var point in points)
        {
            res.Add(point.ToBritish());
        }
        return res;
    }
}
