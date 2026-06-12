using System;
using System.Collections.Generic;
using System.Linq;

namespace TPGeometryPro;

/// <summary>
/// 样条曲线
/// </summary> 
public class HermiteSpline : Curve
{
    #region 静态方法创建

    /// <summary>
    /// 创建样条曲线的新实例
    /// </summary>
    /// <param name="fitPoints">拟合点集合</param>
    /// <param name="isClosed">是否闭合</param>
    /// <returns>新的有界直线</returns>
    /// <exception cref="ArgumentNullException">拟合点数量小于2/两顶点形成的多段线不能闭合</exception>
    /// <exception cref="ArgumentException">两点间距过小</exception>
    public static HermiteSpline Create(List<XYZ> fitPoints, bool isClosed = false)
    {
        if (fitPoints.Count < 2)
        {
            throw new ArgumentNullException(
                "拟合点数量小于2。");
        }

        if (fitPoints.Count == 2 && isClosed)
        {
            throw new ArgumentNullException(
                "两拟合点形成的样条曲线不能闭合。");
        }

        if (fitPoints.Count == 2 && fitPoints.First().IsAlmostEqualTo(fitPoints.Last()))
        {
            throw new ArgumentException(
                "拟合点数量为2时，起始点不可与末尾点重合。");
        }

        var sermiteSpline = new HermiteSpline();
        sermiteSpline.FitPoints = fitPoints;
        sermiteSpline.IsClosed = isClosed;
        sermiteSpline.StartPoint = fitPoints.First();
        sermiteSpline.EndPoint = fitPoints.Last();
        return sermiteSpline;
    }

    #endregion 静态方法创建

    #region 构造属性

    /// <summary>
    /// 拟合点列表
    /// </summary>
    public List<XYZ> FitPoints { get; set; }

    /// <summary>
    /// 是否闭合(末尾点连接起始点)
    /// </summary>
    public bool IsClosed { get; set; }

    #endregion 构造属性

    #region 重写属性

    /// <summary>
    /// 曲线类型
    /// </summary>
    public override string CurveType
    {
        get
        {
            return "HermiteSpline";
        }
    }

    /// <summary>
    /// 是否有界
    /// </summary>
    public override bool IsBound
    {
        get
        {
            return true;
        }
    }

    /// <summary>
    /// 曲线的近似长度
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 当曲线未绑定且不是周期性的时抛出
    /// </exception>
    /// <remarks>
    /// 快速估计曲线的长度，在某些情况下可能会偏离2倍。直线和圆弧的计算是精确的
    /// </remarks>
    public override double ApproximateLength
    {
        get
        {
            return Length;
        }
    }

    /// <summary>
    /// 指示此曲线是否为循环曲线的布尔值
    /// </summary>
    /// <returns>
    /// 如果此曲线是循环的，则为True；否则为false。
    /// </returns>
    public override bool IsCyclic
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 长度(有序排列的点)
    /// </summary>
    public override double Length
    {
        get
        {
            return 0;
        }
    }

    /// <summary>
    /// 直线圆心角为0
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 当曲线不循环时抛出
    /// </exception>
    public override double Period
    {
        get
        {
            return 0;
        }
    }

    #endregion 重写属性
}