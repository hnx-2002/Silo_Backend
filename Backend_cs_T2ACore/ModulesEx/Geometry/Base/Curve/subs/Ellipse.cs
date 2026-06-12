using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace TPGeometryPro;

/// <summary>
/// 椭圆
/// </summary>
/// <remarks>
/// 椭圆
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </remarks>
public class Ellipse : Curve
{
    #region 静态方法创建

    /// <summary>
    /// 创建一个新的几何椭圆或椭圆弧对象。
    /// </summary>
    /// <param name="center">椭圆的中心点。</param>
    /// <param name="xRadius">椭圆在 X 轴方向上的向量半径长度。</param>
    /// <param name="yRadius">椭圆在 Y 轴方向上的向量半径长度。</param>
    /// <param name="xAxis">定义椭圆所在平面的 X 轴向量。必须为单位向量（长度=1.0）。</param>
    /// <param name="yAxis">定义椭圆所在平面的 Y 轴向量。必须为单位向量（长度=1.0）。</param>
    /// <param name="startAngle">椭圆弧起点的原始参数值。</param>
    /// <param name="endAngle">椭圆弧终点的原始参数值。</param>
    /// <returns>
    /// 新的椭圆或椭圆弧曲线对象。
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///  某个不可为空的参数为 NULL。
    ///  </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// xRadius 的值必须大于 0 且不超过 30000 英尺。
    /// -或-yRadius 的值必须大于 0 且不超过 30000 英尺。
    /// -或-xAxis 的长度不是 1.0。
    /// -或-yAxis 的长度不是 1.0。 
    /// -或-起始参数必须小于结束参数。
    /// </exception>
    /// <exception cref="ArgumentException">
    /// xAxis 和 yAxis 向量不垂直。 
    /// -或-曲线长度小于 Revit 的容差值（由 Application.ShortCurveTolerance 确定）。 
    /// </exception>
    /// <remarks>
    /// 如果参数角度范围大于或等于 2 * PI，该曲线将自动转换为一个无界的椭圆。
    /// 如果 xRadius 和 yRadius 几乎相等，该曲线将作为一个圆弧返回。
    /// </remarks>
    public static Ellipse CreateCurve(XYZ center,
        double xRadius, double yRadius, XYZ xAxis, XYZ yAxis,
        double startAngle, double endAngle)
    {
        if (center == null)
        {
            throw new ArgumentNullException(
                nameof(center),
                "center不可为null。");
        }

        if (xAxis == null)
        {
            throw new ArgumentNullException(
                nameof(xAxis),
                "xAxis不可为null。");
        }
        if (yAxis == null)
        {
            throw new ArgumentNullException(
                nameof(yAxis),
                "yAxis不可为null。");
        }


        if (xRadius <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(xRadius),
                "xRadius不可小于等于0");
        }
        if (yRadius <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(yRadius),
                "yRadius不可小于等于0");
        }

        if (!xAxis.IsUnitLength())
        {
            throw new ArgumentOutOfRangeException(
                nameof(xAxis),
                "xAxis应为单位向量");
        }
        if (!yAxis.IsUnitLength())
        {
            throw new ArgumentOutOfRangeException(
                nameof(yAxis),
                "yAxis应为单位向量");
        }

        if (endAngle <= startAngle)
        {
            throw new ArgumentOutOfRangeException(
                nameof(endAngle),
                "endAngle应比startAngle大");
        }

        if (Math.Abs(xAxis.DotProduct(yAxis)) > Config.TOLERANCE)
        {
            throw new ArgumentException(
                "xAxis与yAxis不垂直");
        }


        var ellipse = new Ellipse();

        ellipse.Center = center;
        ellipse.XRadius = xRadius;
        ellipse.YRadius = yRadius;
        ellipse.XDirection = xAxis;
        ellipse.YDirection = yAxis;
        ellipse.StartAngle = startAngle;
        ellipse.EndAngle = endAngle;

        return ellipse;

    }

    #endregion 静态方法创建

    #region 构造属性

    /// <summary>
    /// 椭圆中心
    /// </summary>
    public XYZ Center { get; set; }

    /// <summary>
    /// 弧线x轴半径
    /// </summary>
    public double XRadius { get; set; }

    /// <summary>
    /// 弧线y轴半径
    /// </summary>
    public double YRadius { get; set; }

    /// <summary>
    /// X轴基础向量
    /// </summary>
    public XYZ XDirection { get; set; }

    /// <summary>
    /// Y轴基础向量
    /// </summary>
    public XYZ YDirection { get; set; }

    /// <summary>
    /// 起始角度
    /// </summary>
    public double StartAngle { get; set; }

    /// <summary>
    /// 终止角度
    /// </summary>
    public double EndAngle { get; set; }


    #endregion 构造属性

    #region 重写属性

    /// <summary>
    /// 曲线类型
    /// </summary>
    public override string CurveType
    {
        get
        {
            return "Ellipse";
        }
    }

    /// <summary>
    /// 是否有界 
    /// </summary>
    public override bool IsBound
    {
        get
        {
            double sweep = Math.Abs(EndAngle - StartAngle);
            double remainder = sweep % (Math.PI * 2);
            bool isFullEllipse =
                remainder < Config.TOLERANCE ||
                Math.Abs(remainder - Math.PI * 2) < Config.TOLERANCE;
            return !isFullEllipse; // 整圆返回false，弧线返回true
        }
    }

    /// <summary>
    /// 弧线长度
    /// </summary>
    public override double Length
    {
        get
        {
            return double.NaN;
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
            if (IsBound)
            {
                return EllipseArcLength();
            }
            else
            {
                return EllipsePerimeter();
            }
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
            return !IsBound;
        }
    }

    /// <summary>
    /// 圆心角
    /// 区分优劣弧
    /// </summary>
    public override double Period
    {
        get
        {
            return double.NaN;
        }
    }

    #endregion 重写属性

    #region 重写方法

    /// <summary>
    /// 返回此曲线的副本,继承自Curve
    /// </summary>
    /// <returns>此曲线的副本</returns>
    public override Curve Clone()
    {
        var ellipse = new Ellipse();
        ellipse.Center = Center;
        ellipse.XRadius = XRadius;
        ellipse.XRadius = XRadius;
        ellipse.StartAngle = StartAngle;
        ellipse.EndAngle = EndAngle;
        ellipse.XDirection = XDirection;
        ellipse.YDirection = YDirection;
        return ellipse;
    }

    /// <summary>
    /// 计算并返回与曲线上的参数匹配的点 
    /// </summary>
    /// <param name="parameter">要评估的参数</param>
    /// <param name="normalized">
    /// 如果为false，param将被解释为曲线的自然参数化。
    /// 如果为true，则param应为映射到曲线边界的[0,1]区间。
    /// 只有当曲线被绑定时，设置为true才有效。
    /// </param>
    /// <returns>沿曲线计算的点</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 由于曲线无界，因此无法将其计算为标准化曲线-或-参数不是规范化评估的有效值。
    /// </exception>
    public override XYZ Evaluate(double parameter, bool normalized)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建与现有曲线方向相反的新曲线。
    /// 注意，起点终点取反之后，法向量也取反
    /// </summary>
    /// <returns>新曲线</returns>
    public override Curve CreateReversed()
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
    /// 弧：对于一条弧，偏移是在该弧的平面内完成的。
    /// 产生的偏移是与给定弧在同一平面内且具有相同中心的另一条弧。
    /// 半径会增加或减少偏移距离，具体取决于向右或向左的偏移。
    /// 参考矢量用于根据参考矢量与弧轴（法向量）的点积来确定偏移是在弧的右侧还是左侧。
    /// 如果点积为正，则偏移是相对于弧轴的右侧；如果不是，它在左边。
    /// 如果点积为零，即参考矢量垂直于轴，则函数将返回NULL并报告错误。
    /// </remarks>
    public override Curve CreateOffset(double offsetDist, XYZ referenceVector)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// 返回此曲线起点或终点处的三维点
    /// </summary>
    /// <param name="index">0表示曲线的起点，1表示曲线的终点</param>
    /// <returns>曲线端点</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 曲线是无界的，并没有起点和终点-或-index对于曲线的起点必须为0，对于曲线的终点必须为1。</exception>
    /// <exception cref="ArgumentNullException">非可选参数为NULL</exception>
    public override XYZ GetEndPoint(int index)
    {
        throw new NotImplementedException();
    }

    #endregion 重写方法

    #region 需要进一步实现 属性

    // 【Done】ApproximateLength 继承自Curve
    // 【Done】IsBound 继承自Curve
    // 【Done】IsCyclic 继承自Curve
    // 【Done】Length 继承自Curve
    // 【Done】Period 继承自Curve

    // Reference 继承自Curve 
    // IsReadOnly 继承自APIObject

    // GraphicsStyleId 继承自GeometryObject
    // IsElementGeometry 继承自GeometryObject
    // Visibility 继承自GeometryObject

    #endregion 需要进一步实现 属性

    #region 需进一步实现 方法

    // 【Done】Clone 继承自Curve
    // 【Done】Evaluate 继承自Curve
    // 【Done】CreateReversed 继承自Curve
    // 【Done】GetEndPoint 继承自Curve

    // ComputeClosestPoints 继承自Curve
    // ComputeDerivatives 继承自Curve
    // ComputeNormalizedParameter 继承自Curve
    // ComputeRawParameter 继承自Curve
    // CreateOffset 继承自Curve 
    // CreateTransformed 继承自Curve
    // Distance 继承自Curve 
    // GetEndParameter 继承自Curve 
    // GetEndPointReference 继承自Curve
    // Intersect(Curve)  继承自Curve
    // Intersect(Curve, IntersectionResultArray) 继承自Curve
    // IsInside(Double) 继承自Curve
    // IsInside(Double, Int32)  继承自Curve
    // MakeBound 继承自Curve
    // MakeUnbound 继承自Curve
    // Project  继承自Curve
    // SetGraphicsStyleId 继承自Curve
    // Tessellate 继承自Curve 
    // GetHashCode 继承自GeometryObject
    // Equals 继承自GeometryObject 
    // Dispose 继承自APIObject 
    // GetType 继承自Object
    // ToString 继承自Object

    #endregion 需进一步实现 方法

    #region 私有方法

    /// <summary>
    /// 计算整个椭圆的周长（Ramanujan 第二近似公式，误差小于0.04%）。
    /// </summary>
    private double EllipsePerimeter()
    {
        double h = Math.Pow(XRadius - YRadius, 2) / Math.Pow(XRadius + YRadius, 2);
        return Math.PI * (XRadius + YRadius) * (1 + (3 * h) / (10 + Math.Sqrt(4 - 3 * h)));
    }

    /// <summary>
    /// 计算椭圆弧的弧长（Gauss–Kummer 数值积分，精度可配置）。
    /// </summary>
    private double EllipseArcLength(int n = 32)          // 积分分段数，可再调大
    {
        // 椭圆离心率平方 e²
        double e2 = 1.0 - (YRadius * YRadius) / (XRadius * XRadius);
        if (Math.Abs(e2) < Config.TOLERANCE)        // 圆弧退化
            return XRadius * (EndAngle - StartAngle);

        // 参数方程 x = a cosθ, y = b sinθ
        // 弧长公式 S = ∫√(a²sin²θ + b²cos²θ)dθ
        Func<double, double> integrand = theta =>
        {
            double s = Math.Sin(theta);
            double c = Math.Cos(theta);
            return Math.Sqrt(XRadius * XRadius * s * s + YRadius * YRadius * c * c);
        };

        // 用 Simpson 1/3 法做数值积分
        double h = (EndAngle - StartAngle) / n;
        double sum = integrand(StartAngle) + integrand(EndAngle);
        for (int i = 1; i < n; i++)
        {
            double y = integrand(StartAngle + i * h);
            sum += (i % 2 == 0 ? 2 : 4) * y;
        }
        return sum * h / 3.0;
    }
    #endregion 私有方法
}
