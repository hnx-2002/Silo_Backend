using System;

namespace TPGeometryPro;

/// <summary>
/// 判等方法
/// </summary>
public static class FunEqual
{
    /// <summary>
    /// 两个double判等
    /// </summary>
    /// <param name="val1"></param>
    /// <param name="val2"></param>
    /// <returns></returns>
    public static bool IsAlmostEqualTo(this double val1, double val2)
    {
        return Math.Abs(val1 - val2) < Config.TOLERANCE;
    }

    /// <summary>
    /// 确定此矢量和指定矢量在公差（1.0e-09）内是否相同。
    /// </summary>
    /// <returns>如果矢量相同，则为True；否则为false。</returns> 
    /// <param name="source">要与此矢量进行比较的矢量。</param>
    /// <param name="point2">要与此矢量进行比较的矢量。</param>
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception> 
    /// <remarks>
    /// 此处使用默认公差来比较两个矢量，以查看它们是否几乎相等。 
    /// 因为公差足够小，所以也可以用于比较两点。
    /// </remarks>
    public static bool IsAlmostEqualTo(this XYZ source, XYZ point2)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        if (point2 == null)
        {
            throw new ArgumentNullException(
                nameof(point2),
                "point2不可为null。");
        }

        return point2.X.IsAlmostEqualTo(source.X) &&
               point2.Y.IsAlmostEqualTo(source.Y) &&
               point2.Z.IsAlmostEqualTo(source.Z);
    }
}
