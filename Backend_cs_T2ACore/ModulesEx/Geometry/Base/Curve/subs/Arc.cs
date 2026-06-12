using System;

namespace TPGeometryPro;

/// <summary>
/// 圆弧
/// </summary>
/// <remarks>
/// 圆弧位于由中心和法线定义的平面内
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </remarks>
public class Arc : Curve
{
    #region 静态方法创建

    /// <summary>
    /// 基于三个点创建新的几何圆弧对象
    /// </summary>
    /// <param name="end0">圆弧的起点</param>
    /// <param name="end1">圆弧的终点</param>
    /// <param name="pointOnArc">弧上的一个点</param>
    /// <returns>新的弧线</returns>
    /// <exception cref="ArgumentNullException">
    /// 入参三个点为null时抛出
    ///  </exception>
    /// <exception cref="ArgumentException">
    /// 矢量end0和end1重合
    /// -或-矢量end0和pointOnArc重合
    /// -或-矢量end1和pointOnArc重合
    /// -或-无法创建圆弧。
    /// -或-曲线长度太小。
    /// </exception>
    public static Arc Create(XYZ end0, XYZ end1, XYZ pointOnArc)
    {
        if (end0 == null)
        {
            throw new ArgumentNullException(
                nameof(end0),
                "end0不可为null。");
        }

        if (end1 == null)
        {
            throw new ArgumentNullException(
                nameof(end1),
                "end1不可为null。");
        }

        if (pointOnArc == null)
        {
            throw new ArgumentNullException(
                nameof(pointOnArc),
                "pointOnArc不可为null。");
        }

        if (end0.IsAlmostEqualTo(end1))
        {
            throw new ArgumentException(
                "end0不可与end1重合");
        }

        if (end0.IsAlmostEqualTo(pointOnArc))
        {
            throw new ArgumentException(
                "pointOnArc不可与end0重合");
        }

        if (end1.IsAlmostEqualTo(pointOnArc))
        {
            throw new ArgumentException(
                "pointOnArc不可与end1重合");
        }

        var arc = new Arc();
        arc.StartPoint = end0;
        arc.EndPoint = end1;
        var (r, center) = FunXYZ.CalArcRadius(end0, end1, pointOnArc);
        arc.Radius = r;
        arc.Center = center;
        Plane plane = Plane.CreateByThreePoints(end0, pointOnArc, end1);
        arc.Normal = plane.Normal;

        return arc;
    }

    /// <summary>
    /// 基于平面、半径和角度创建新的几何圆弧对象。
    /// </summary>
    /// <param name="plane">圆弧所在的平面。平面的原点是圆弧的中心</param>
    /// <param name="radius">圆弧的半径</param>
    /// <param name="startAngle">起始角度：圆弧的起始角度（以弧度为单位）</param>
    /// <param name="endAngle">结束角度：圆弧的结束角度（以弧度为单位）</param>
    /// <returns>新的弧线</returns>
    /// <exception cref="ArgumentNullException">
    /// 任意参数为null时抛异常
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 给定的半径值必须大于0且不超过9144000mm
    /// </exception>
    /// <exception cref="ArgumentException">
    /// 开始角度必须小于结束角度
    /// -或-曲线长度太小
    /// </exception>
    /// <remarks>
    /// 如果角度范围等于或大于2* PI，则曲线将自动转换为无边界圆。
    /// </remarks>
    public static Arc Create(Plane plane, double radius, double startAngle, double endAngle)
    {
        if (plane == null)
        {
            throw new ArgumentNullException(
                nameof(plane),
                "plane不可为null。");
        }

        if (radius <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius), "给定的半径值必须大于0");
        }

        if (radius > 9144000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius), "给定的半径值必须不超过9144000mm");
        }

        if (endAngle <= startAngle)
        {
            throw new ArgumentException("开始角度必须小于结束角度");
        }

        //TODO 曲线长度太小的异常

        var arc = new Arc();
        arc.Radius = radius;
        arc.Center = plane.Origin;
        arc.Normal = plane.Normal;
        XYZ vector_Start = plane.XVec.Rotate(plane.Normal, startAngle).Normalize();
        XYZ vector_End = plane.XVec.Rotate(plane.Normal, endAngle).Normalize();
        arc.StartPoint = plane.Origin + vector_Start * radius;
        arc.EndPoint = plane.Origin + vector_End * radius;
        return arc;
    }

    /// <summary>
    /// 基于中心、半径、单位矢量和角度创建新的几何圆弧对象。
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="radius">圆弧的半径</param>
    /// <param name="startAngle">起始角度：圆弧的起始角度（以弧度为单位）</param>
    /// <param name="endAngle">结束角度：圆弧的结束角度（以弧度为单位）</param>
    /// <param name="xAxis">x轴：定义圆弧平面的x轴。必须单位化</param>
    /// <param name="yAxis">Y轴：定义圆弧平面的y轴。必须单位化。</param>
    /// <returns>新的弧线</returns>
    /// <exception cref="ArgumentNullException">
    /// 任意参数为null时抛异常
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// xAxis的长度不是1.0。
    /// -或-yAxis的长度不是1.0。
    /// -或-给定的半径值必须大于0且不超过9144000mm
    /// </exception>
    /// <exception cref="ArgumentException">
    /// 矢量xAxis和yAAxis不垂直。
    /// -或-起始角度必须小于结束角度。
    /// -或-曲线长度太小。
    /// </exception>
    /// <remarks>
    /// 如果角度范围等于或大于2*PI，则曲线将自动转换为无边界圆。
    /// </remarks>
    public static Arc Create(XYZ center, double radius, double startAngle, double endAngle, XYZ xAxis, XYZ yAxis)
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

        if (!xAxis.IsUnitLength())
        {
            throw new ArgumentOutOfRangeException(
                nameof(xAxis),
                "xAxis的长度不是1.0");
        }

        if (!yAxis.IsUnitLength())
        {
            throw new ArgumentOutOfRangeException(
                nameof(yAxis),
                "yAxis的长度不是1.0");
        }

        if (radius <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius), "给定的半径值必须大于0");
        }

        if (radius > 9144000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radius), "给定的半径值必须不超过9144000mm");
        }

        if (Math.Abs(xAxis.DotProduct(yAxis)) > Config.TOLERANCE)
        {
            throw new ArgumentOutOfRangeException("矢量xAxis和yAAxis不垂直");
        }

        if (endAngle <= startAngle)
        {
            throw new ArgumentException("开始角度必须小于结束角度");
        }

        //TODO 曲线长度太小的异常

        var arc = new Arc();
        arc.Radius = radius;
        arc.Center = center;
        arc.Normal = xAxis.CrossProduct(yAxis).Normalize();
        XYZ vector_Start = xAxis.Rotate(arc.Normal, startAngle).Normalize();
        XYZ vector_End = xAxis.Rotate(arc.Normal, endAngle).Normalize();
        arc.StartPoint = center + vector_Start * radius;
        arc.EndPoint = center + vector_End * radius;
        return arc;
    }

    #endregion 静态方法创建

    #region 构造属性

    /// <summary>
    /// 弧线中心
    /// </summary>
    public XYZ Center { get; set; }

    /// <summary>
    /// 弧线半径
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// 返回定义圆弧所在平面的法线。
    /// </summary>
    public XYZ Normal { get; set; }

    #endregion 构造属性

    #region 属性

    /// <summary>
    /// X 方向单位向量 
    /// </summary>
    public XYZ XDirection
    {
        get
        {
            Plane plane = Plane.CreateByNormalAndOrigin(Normal, Center);
            return plane.XVec;
        }
    }

    /// <summary>
    /// Y 方向单位向量 
    /// </summary>
    public XYZ YDirection
    {
        get
        {
            Plane plane = Plane.CreateByNormalAndOrigin(Normal, Center);
            return plane.YVec;
        }
    }

    /// <summary>
    /// 中点
    /// </summary>
    public XYZ MiddlePoint
    {
        get
        {
            var st = StartPoint;
            var mid = Evaluate(0.5, true);
            var ed = EndPoint;
            return mid;
        }
    }


    #endregion 属性

    #region 重写属性

    /// <summary>
    /// 曲线类型
    /// </summary>
    public override string CurveType
    {
        get
        {
            return "Arc";
        }
    }

    /// <summary>
    /// 是否有界
    /// 起点终点一致，IsBound为false，认为是无界的
    /// </summary>
    public override bool IsBound
    {
        get
        {
            if (StartPoint.IsAlmostEqualTo(EndPoint)) //起点终点一致，IsBound为false
            {
                return false;
            }
            else
            {
                return true;
            }
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
            return !IsBound;
        }
    }

    /// <summary>
    /// 弧线长度
    /// </summary>
    public override double Length
    {
        get
        {
            XYZ oa = (StartPoint - Center).Normalize();
            XYZ ob = (EndPoint - Center).Normalize();
            double angle = Math.PI * 2;
            if (!oa.IsAlmostEqualTo(ob))
            {
                angle = oa.AngleTo(ob);
            }

            return angle * Radius;
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
            if (StartPoint.IsAlmostEqualTo(EndPoint))
            {
                return Math.PI * 2;
            }

            //判断优弧和劣弧
            XYZ oa = (StartPoint - Center).Normalize();
            XYZ ob = (EndPoint - Center).Normalize();
            XYZ cross = oa.CrossProduct(ob).Normalize();

            //优弧
            if (cross.IsAlmostEqualTo(Normal.Negate()))
            {
                return Math.PI * 2 - oa.AngleTo(ob);
            }
            //劣弧
            else
            {
                return oa.AngleTo(ob);
            }
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
        var arc = new Arc();
        arc.Center = Center;
        arc.Radius = Radius;
        arc.StartPoint = StartPoint;
        arc.EndPoint = EndPoint;
        arc.Normal = Normal;
        return arc;
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
        double angle = 0;
        XYZ oa = (StartPoint - Center).Normalize();

        if (parameter < -Config.TOLERANCE)
        {
            throw new ArgumentOutOfRangeException("输入的比例非法，请重新输入！");
        }

        if (normalized)
        {
            if ((parameter - 1) > Config.TOLERANCE)
            {
                throw new ArgumentOutOfRangeException("输入的比例超出线段范围，请重新输入！");
            }

            if (parameter > Config.TOLERANCE)
            {
                angle = Period * parameter;
            }
            else
            {
                angle = 0;
            }
        }
        else
        {
            angle = parameter;
        }

        try
        {
            oa = oa.Rotate(Normal, angle);
            XYZ start_V2 = Center + (oa * Radius);
            return start_V2;
        }
        catch (Exception err)
        {
            throw new ArgumentException(err.ToString());
        }
    }

    /// <summary>
    /// 创建与现有曲线方向相反的新曲线。
    /// 注意，起点终点取反之后，法向量也取反
    /// </summary>
    /// <returns>新曲线</returns>
    public override Curve CreateReversed()
    {
        var arc = new Arc();
        arc.Center = Center;
        arc.Radius = Radius;
        arc.StartPoint = EndPoint;
        arc.EndPoint = StartPoint;
        arc.Normal = Normal.Negate();
        return arc;
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
        if (index == 0)
        {
            return StartPoint;
        }
        else if (index == 1)
        {
            return EndPoint;
        }
        else
        {
            throw new ArgumentOutOfRangeException("index索引只能为0或者1");
        }
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


}