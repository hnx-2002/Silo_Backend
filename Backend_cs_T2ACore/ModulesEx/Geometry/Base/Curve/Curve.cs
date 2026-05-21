using System;

namespace TPGeometryPro;

/// <summary>
/// 参数曲线
/// </summary>
/// <remarks>
/// 具体的曲线由参数方程定义。
/// 如果曲线有界，则它仅在参数化区间内定义。
/// 否则，它对参数的所有值都有定义。
/// </remarks>
public class Curve
{
    #region 属性

    /// <summary>
    /// 起点
    /// </summary>
    public virtual XYZ StartPoint { get; set; }

    /// <summary>
    /// 终点
    /// </summary>
    public virtual XYZ EndPoint { get; set; }

    /// <summary>
    /// 曲线类型
    /// </summary>
    public virtual string CurveType
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 描述曲线的参数是否被限制为特定的间隔。
    /// </summary>
    public virtual bool IsBound
    {
        get
        {
            throw new NotImplementedException();
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
    public virtual double ApproximateLength
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 指示此曲线是否为循环曲线的布尔值
    /// </summary>
    /// <returns>
    /// 如果此曲线是循环的，则为True；否则为false。
    /// </returns>
    public virtual bool IsCyclic
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 曲线的确切长度
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 当曲线无界且不是周期性的时抛出
    /// </exception>
    /// <remarks>
    /// 使用分析积分或数值积分计算曲线的长度。
    /// 直线和圆弧没有性能命中
    /// </remarks>
    public virtual double Length
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 圆心角
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 当曲线不循环时抛出
    /// </exception>
    public virtual double Period
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    #endregion 属性

    #region 方法
    /// <summary>
    /// 返回此曲线的副本
    /// </summary>
    /// <returns>此曲线的副本</returns>
    public virtual Curve Clone()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 计算并返回与曲线上的参数匹配的点
    /// </summary>
    /// <param name="parameter">要评估的参数</param>
    /// <param name="normalized">
    /// 如果为false，param将被解释为曲线的自然参数化。
    /// 如果为true，则param应为映射到曲线边界的[0,1]区间。
    /// 只有当曲线有界时，设置为true才有效。
    /// </param>
    /// <returns>沿曲线计算的点</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 由于曲线无界，因此无法将其计算为标准化曲线-或-参数不是规范化评估的有效值。
    /// </exception>
    public virtual XYZ Evaluate(double parameter, bool normalized)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建与现有曲线方向相反的新曲线。
    /// </summary>
    /// <returns>新曲线</returns>
    public virtual Curve CreateReversed()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建一条新曲线，该曲线是现有曲线的偏移。
    /// </summary>
    /// <param name="offsetDist">控制偏移的有符号距离</param>
    /// <param name="referenceVector">用于定义偏移方向的参考矢量</param>
    /// <returns>新曲线</returns>
    /// <exception cref="InvalidOperationException">无法创建曲线的偏移量</exception>
    /// <remarks>
    /// 偏移曲线是通过将给定曲线的点相对于参考向量向右移动一定距离（不一定是输入距离）来获得的。
    /// 如果距离为负，则偏移实际上将在左侧。参考向量所起的确切作用以不一致的方式取决于曲线类型，如下所述。
    /// 我们计划更新此函数，使其行为更加一致和易于理解。
    /// 对于“Line”、“HermiteSpline”、“NurbSpline”和“Cylindrical Helix”，
    /// 右方向是沿着该点的切线与参考向量的叉积。
    /// 换言之，曲线上给定点处的曲线“右侧”被定义为参考向量被视为向上方向，曲线切线被视为向前方向，
    /// 就好像你在沿着曲线行走时身体与参考向量对齐一样。
    /// 对于“圆弧”和“椭圆”，右侧方向是相对于圆弧或椭圆的轴以及参考向量定义的。
    /// 如果参考向量与轴的点积为正，则向右的方向是沿着与轴相切的曲线的叉积。
    /// 如果点积是负的，那么它是相反的。
    /// 如果点积为零，则这是一个输入错误。
    /// 行为的更多细节取决于曲线的类型：
    ///
    /// 直线：对于直线，向量Perp计算为直线方向和参考向量的叉积。
    /// 偏移是通过将直线沿vecPerp的方向移动给定距离来获得的。
    /// 注意：如果参考矢量平行于直线方向，vecPerp可以是零矢量。
    /// 在这种情况下，偏移只是给定行的副本。未报告任何错误。
    ///
    /// 弧：对于一条弧，偏移是在该弧的平面内完成的。
    /// 产生的偏移是与给定弧在同一平面内且具有相同中心的另一条弧。
    /// 半径会增加或减少偏移距离，具体取决于向右或向左的偏移。
    /// 参考矢量用于根据参考矢量与弧轴（法向量）的点积来确定偏移是在弧的右侧还是左侧。
    /// 如果点积为正，则偏移是相对于弧轴的右侧；如果不是，它在左边。
    /// 如果点积为零，即参考矢量垂直于轴，则函数将返回NULL并报告错误。
    ///
    /// 椭圆：椭圆也是像圆弧一样的平面曲线。偏移是在椭圆的平面内完成的。
    /// 请注意，椭圆的偏移通常不是椭圆。它将由埃尔米特样条曲线近似。
    /// 偏移曲线是通过将椭圆的点相对于椭圆的轴（法向量）向右或向左偏移给定的偏移距离来获得的。
    /// 参考矢量用于根据参考矢量与椭圆轴的点积来确定是向椭圆的右侧还是左侧偏移。
    /// 如果点积为正，则偏移相对于椭圆的轴向右；如果不是，它在左边。
    /// 如果点积为零，即参考矢量垂直于轴，则函数将返回NULL并报告错误。
    ///
    /// Hermite样条曲线、NurbSpline曲线和Cylindrical Helix：
    /// Hermite或Nurbs样条曲线可以是平面的，也可以是非平面的；圆柱形螺旋线是非平面的。
    /// 对于这三种类型的曲线，偏移量的计算方法如下：
    /// 设P为给定曲线上的一个点，设T为P处的单位切向量，假设曲线在P处的导数不为零。
    /// P处的偏移矢量是T和参考矢量的叉积。然后，对应于P的偏移点Q被计算为：Q＝P+（偏移距离）*（在P处的偏移矢量）。
    /// 偏移取决于参考矢量的大小和方向。如果参考矢量的大小不是一，则实际移动的距离将不是给定的偏移距离。
    /// 注：如果曲线是平面Hermite或Nurbs样条曲线，则偏移曲线可能不在同一平面上，这与圆弧和椭圆的情况不同。
    /// </remarks>
    public virtual Curve CreateOffset(double offsetDist, XYZ referenceVector)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 返回此曲线起点或终点处的三维点
    /// </summary>
    /// <param name="index">0表示曲线的起点，1表示曲线的终点</param>
    /// <returns>曲线端点</returns>
    /// <exception cref="ArgumentException">
    /// 曲线是无界的，并没有起点和终点-或-index对于曲线的起点必须为0，对于曲线的终点必须为1。</exception>
    /// <exception cref="ArgumentNullException">非可选参数为NULL</exception>
    public virtual XYZ GetEndPoint(int index)
    {
        throw new NotImplementedException();
    }
    #endregion  方法


}