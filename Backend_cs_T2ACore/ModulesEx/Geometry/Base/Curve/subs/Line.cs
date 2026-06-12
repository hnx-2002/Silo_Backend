using System;

namespace TPGeometryPro;

/// <summary>
/// 直线
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </summary>
public class Line : Curve
{
    #region 静态方法创建 
    /// <summary>
    /// 创建直线的新实例
    /// </summary>
    /// <param name="endpoint1">第一个线端点</param>
    /// <param name="endpoint2">第二个线端点</param>
    /// <returns>新的有界直线</returns>
    /// <exception cref="ArgumentNullException">当endpoint1或者endpoint2为null时抛出</exception>
    /// <exception cref="ArgumentException">两点间距过小</exception>
    public static Line Create(XYZ endpoint1, XYZ endpoint2)
    {
        if (endpoint1 == null)
        {
            throw new ArgumentNullException(
                nameof(endpoint1),
                "endpoint1不能为null。");
        }

        if (endpoint2 == null)
        {
            throw new ArgumentNullException(
                nameof(endpoint2),
                "endpoint2不能为null。");
        }

        if (endpoint1.IsAlmostEqualTo(endpoint2))
        {
            throw new ArgumentException(
                "两点间距过小");
        }

        var line = new Line();
        line.StartPoint = endpoint1;
        line.EndPoint = endpoint2;
        return line;
    }

    #endregion 静态方法创建

    #region 属性

    /// <summary>
    /// 起点
    /// </summary>
    public XYZ Origin
    {
        get
        {
            return StartPoint;
        }
    }

    /// <summary>
    /// 方向
    /// </summary>
    public XYZ Direction
    {
        get
        {
            return (EndPoint - StartPoint).Normalize();
        }
    }
    #endregion  属性


    #region 重写属性

    /// <summary>
    /// 曲线类型
    /// </summary>
    public override string CurveType
    {
        get
        {
            return "Line";
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
    /// 长度
    /// </summary>
    public override double Length
    {
        get
        {
            return EndPoint.DistanceTo(StartPoint);
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


    #region 重写方法

    /// <summary>
    /// 返回此曲线的副本,继承自Curve
    /// </summary>
    /// <returns>此曲线的副本</returns>
    public override Curve Clone()
    {
        return Create(StartPoint, EndPoint);
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
        double length = 0;
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
                length = Length * parameter;
            }
            else
            {
                length = 0;
            }
        }
        else
        {
            length = parameter;
        }

        try
        {
            XYZ start_V2 = StartPoint + (Direction * length);
            return start_V2;
        }
        catch (Exception err)
        {
            throw new ArgumentException(err.ToString());
        }
    }

    /// <summary>
    /// 创建与现有曲线方向相反的新曲线。 
    /// </summary>
    /// <returns>新曲线</returns>
    public override Curve CreateReversed()
    {
        return Create(EndPoint, StartPoint);
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
    /// </remarks>
    public override Curve CreateOffset(double offsetDist, XYZ referenceVector)
    {
        //确定直线偏移方向：线方向和referenceVector的叉乘方向
        XYZ offsetVector = Direction.CrossProduct(referenceVector);

        if (offsetVector.IsZeroLength())
        {
            throw new ArgumentException("参考向量与线方向平行，无法确定平移方向，不能创建平移！");
        }
        offsetVector = offsetVector.Normalize();

        XYZ sp_Offset = GetEndPoint(0).Add(offsetVector * offsetDist);
        XYZ ep_Offset = GetEndPoint(1).Add(offsetVector * offsetDist);
        return Create(sp_Offset, ep_Offset);

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



    #region 需进一步实现 方法 

    // 【Done】 Clone 继承自Curve
    // 【Done】 Evaluate 继承自Curve
    // 【Done】 CreateReversed 继承自Curve
    // 【Done】 CreateOffset 继承自Curve 
    // 【Done】 GetEndPoint 继承自Curve

    // ComputeClosestPoints 继承自Curve
    // ComputeDerivatives 继承自Curve
    // ComputeNormalizedParameter 继承自Curve
    // ComputeRawParameter 继承自Curve 
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

    // Intersect(Curve, List<XYZ>) 新加方法

    #endregion 需进一步实现 方法

    #region 需进一步实现 属性

    //  【Done】 ApproximateLength 继承自Curve
    //  【Done】 IsBound 继承自Curve
    //  【Done】 IsCyclic 继承自Curve
    //  【Done】 Length 继承自Curve
    //  【Done】 Period 继承自Curve
    // Reference 继承自Curve

    // IsReadOnly 继承自APIObject

    // GraphicsStyleId 继承自GeometryObject
    // IsElementGeometry 继承自GeometryObject
    // Visibility 继承自GeometryObject

    #endregion 需进一步实现 属性
}