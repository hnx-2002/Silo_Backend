using System;

namespace TPGeometryPro;

/// <summary>
/// 表示三维空间中坐标的对象
/// </summary>
/// <remarks>
/// 通常这意味着三维空间中的点或矢量，具体取决于实际使用情况
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </remarks>
public class XYZ
{
    #region 静态构造方法

    /// <summary>使用提供的坐标创建XYZ。</summary>
    /// <param name="x">第一个坐标。</param>
    /// <param name="y">第二个坐标。</param>
    /// <param name="z">第三个坐标。</param>
    /// <exception cref="ArgumentException">
    /// 将无穷大的数字设置为X、Y或Z属性时抛出。
    /// </exception> 
    public static XYZ New(double x, double y, double z)
    {
        XYZ result = new XYZ();
        result.X = x;
        result.Y = y;
        result.Z = z;
        return result;
    }

    #endregion 静态构造方法

    #region 属性

    /// <summary>
    /// 第idx个坐标值
    /// </summary>
    /// <param name="idx">索引</param>
    /// <returns>返回第idx个坐标值</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// 索引只能为0,1,2
    /// </exception>
    public double this[int idx]
    {
        get
        {
            if (idx == 0)
            {
                return X;
            }
            else if (idx == 1)
            {
                return Y;
            }
            else if (idx == 2)
            {
                return Z;
            }
            else
            {
                throw new IndexOutOfRangeException("索引只能为0,1,2");

            }
        }
    }

    /// <summary>
    /// 获取第一个坐标。
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// 获取第二个坐标。
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// 获取第三个坐标。
    /// </summary>
    public double Z { get; set; }

    #endregion 属性


    #region 方法

    /// <summary>
    /// 将指定的向量添加到此向量并返回结果
    /// </summary>
    /// <param name="source">要添加到此向量的向量</param>
    /// <returns>等于两个向量之和的向量</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception>
    /// <remarks>
    /// 通过将指定向量的每个坐标添加到该向量的相应坐标来获得添加的向量。
    /// </remarks>
    public XYZ Add(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不能为null。");
        }

        return New(X + source.X, Y + source.Y, Z + source.Z);
    }

    /// <summary>
    /// 返回此矢量与投影到指定平面的指定矢量之间的角度
    /// </summary>
    /// <param name="right">指定的向量</param>
    /// <param name="normal">定义平面的法向量</param>
    /// <returns>0和2*PI之间的实数,两个向量之间的投影角度。</returns>
    /// <exception cref="ArgumentNullException">当right或normal为null时抛出</exception>
    /// <remarks>
    /// 该角度投影到与指定法向量正交的平面上，法向量逆时针向上。
    /// </remarks>
    public double AngleOnPlaneTo(XYZ right, XYZ normal)
    {
        //TODO 未测试
        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不可为null。");
        }

        if (normal == null)
        {
            throw new ArgumentNullException(
                nameof(normal),
                "normal不可为null。");
        }

        // 计算向量在平面上的投影
        XYZ thisProjection = this - DotProduct(normal) / normal.DotProduct(normal) * normal;
        XYZ rightProjection = right - right.DotProduct(normal) / normal.DotProduct(normal) * normal;

        // 计算投影向量的单位向量
        thisProjection = thisProjection.Normalize();
        rightProjection = rightProjection.Normalize();

        // 使用点积计算角度的余弦值
        double dot = thisProjection.DotProduct(rightProjection);

        // 确保余弦值在[-1,1]范围内，以避免由于浮点误差导致的异常
        dot = Math.Max(-1.0, Math.Min(1.0, dot));

        // 计算角度
        double angle = Math.Acos(dot);

        // 使用叉积确定角度的方向（顺时针或逆时针）
        XYZ cross = thisProjection.CrossProduct(rightProjection);
        if (cross.DotProduct(normal) < 0)
            angle = 2 * Math.PI - angle;

        return angle;
    }

    /// <summary>
    /// 返回此矢量与指定矢量之间的角度。
    /// </summary>
    /// <param name="source">指定的矢量。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"> 当source为null时抛出 </exception>
    /// <exception cref="InvalidOperationException">其中一个向量的长度为零。</exception>
    /// <returns>两个矢量之间的角度（以弧度为单位,0和PI之间的实数）。</returns>
    /// <remarks>两个矢量之间的角度是在它们所跨越的平面内测量的。</remarks>
    public double AngleTo(XYZ source)
    {
        //TODO 未测试
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不能为null。");
        }

        // 假设 XYZ 是一个向量，且具有 X, Y, Z 作为其分量的属性。
        // 这个计算使用点积和向量的大小（范数）来找到角度。
        double dotProduct = X * source.X + Y * source.Y + Z * source.Z;
        double magnitudeA = Math.Sqrt(X * X + Y * Y + Z * Z);
        double magnitudeB = Math.Sqrt(source.X * source.X + source.Y * source.Y + source.Z * source.Z);

        // 防止除以零
        if (magnitudeA == 0 || magnitudeB == 0)
        {
            throw new InvalidOperationException("其中一个向量的长度为零。");
        }

        double cosAngle = dotProduct / (magnitudeA * magnitudeB);
        // 确保余弦值在 [-1, 1] 范围内，避免 NaN 结果
        //cosAngle = cosAngle.Clamp(-1.0, 1.0);//Math.Clamp

        if (cosAngle.CompareTo(-1.0) < 0) cosAngle = -1.0;
        if (cosAngle.CompareTo(1.0) > 0) cosAngle = 1.0;

        // 以弧度计算角度
        double angle = Math.Acos(cosAngle);

        return angle;
    }

    /// <summary>
    /// 此向量与指定向量的叉积。
    /// </summary>
    /// <param name="source">要与此向量相乘的向量。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出 </exception>
    /// <returns>等于叉积的向量。</returns>
    /// <remarks>
    /// 叉积定义为垂直于两个向量的向量，其大小等于它们跨越的平行四边形的面积。
    /// 也称为向量积或外积。
    /// </remarks>
    public XYZ CrossProduct(XYZ source)
    {
        //TODO 未测试
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "Source vector cannot be null.");
        }

        double x = Y * source.Z - Z * source.Y;
        double y = Z * source.X - X * source.Z;
        double z = X * source.Y - Y * source.X;

        return New(x, y, z);
    }

    /// <summary>返回从该点到指定点的距离。</summary>
    /// <param name="source">指定的点。</param>
    /// <returns>两点之间的距离。</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception>        
    /// <remarks>
    /// 两点之间的距离等于连接两点的向量的长度。
    /// </remarks>
    public double DistanceTo(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return Math.Sqrt(
            Math.Pow(X - source.X, 2) +
            Math.Pow(Y - source.Y, 2) +
            Math.Pow(Z - source.Z, 2));

    }

    /// <summary>将此矢量除以指定的值并返回结果。</summary>
    /// <param name="value">将此矢量除以的值。</param>
    /// <returns>除法操作之后得到的向量</returns>
    /// <exception cref="ArgumentException">
    /// 当指定的值是零时抛出。
    /// </exception>
    /// <remarks>
    /// 结果矢量是通过将该矢量的每个坐标除以指定值而获得的。
    /// </remarks>
    public XYZ Divide(double value)
    {
        if (Math.Abs(value) < Config.TOLERANCE) // 使用一个非常小的值来判断value是否接近于0
        {
            throw new DivideByZeroException("value不能为零。");
        }

        return New(X / value, Y / value, Z / value);
    }

    /// <summary>此向量与指定向量的点积。</summary>
    /// <param name="source">要与此向量相乘的向量。</param>
    /// <returns>等于点积的实数。</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception>
    /// <remarks>
    /// 点积是两个向量的各自坐标之和：Vx*Wx+Vy*Wy+Vz*Wz。
    /// 也称为标量积或内积。
    /// </remarks>
    public double DotProduct(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return X * source.X + Y * source.Y + Z * source.Z;
    }

    /// <summary>
    /// 获取此矢量的长度。
    /// </summary>
    /// <remarks>
    /// 在三维欧几里得空间中，向量的长度是三个坐标之和的平方根。
    /// </remarks>
    public double GetLength()
    {
        return DistanceTo(Zero);
    }

    /// <summary>
    /// 确定此矢量和指定矢量在公差（Config.TOLERANCE）内是否相同。
    /// </summary>
    /// <returns>如果矢量相同，则为True；否则为false。</returns> 
    /// <param name="source">要与此矢量进行比较的矢量。</param>
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception> 
    /// <remarks>
    /// 此处使用默认公差来比较两个矢量，以查看它们是否几乎相等。 
    /// 因为公差足够小，所以也可以用于比较两点。
    /// </remarks>
    public bool IsAlmostEqualTo(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return Math.Abs(X - source.X) < Config.TOLERANCE &&
               Math.Abs(Y - source.Y) < Config.TOLERANCE &&
               Math.Abs(Z - source.Z) < Config.TOLERANCE;
    }

    /// <summary>
    /// 确定两个矢量在给定公差内是否相同。
    /// </summary> 
    /// <param name="source">要与此矢量进行比较的矢量。</param>
    /// <param name="tolerance">相等检查的公差。</param>
    /// <returns>如果矢量相同，则为True；否则为false。</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception>
    /// <exception cref="ArgumentException">
    /// 当tolerance小于0时抛出 
    /// </exception> 
    /// <remarks>此处使用输入容差来比较两个矢量，以查看它们是否几乎相等。
    /// 因为它比较两个矢量，所以公差值不是以长度为单位，而是表示矢量之间方向的变化。
    /// 对于非常小的公差值，也应该可以使用此方法比较两点。
    /// 若要计算两点之间的距离以进行允许差异较大的比较，请使用DistanceTo
    /// </remarks>
    public bool IsAlmostEqualTo(XYZ source, double tolerance)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        if (tolerance <= 0)
        {
            throw new ArgumentException(
                nameof(tolerance),
                "tolerance不可小于等于0");
        }

        return Math.Abs(X - source.X) < tolerance &&
               Math.Abs(Y - source.Y) < tolerance &&
               Math.Abs(Z - source.Z) < tolerance;
    }

    /// <summary>
    /// 指示此向量是否具有单位长度。
    /// </summary>
    /// <remarks>
    /// 单位长度向量的长度为1，并且被认为是归一化的。
    /// </remarks> 
    public bool IsUnitLength()
    {
        var temp = Normalize();
        return Math.Abs(temp.GetLength() - 1) < Config.TOLERANCE &&
               IsAlmostEqualTo(temp);
    }

    /// <summary>
    /// 验证输入点是否在Revit设计限制范围内。
    /// </summary>
    /// <param name="point">要检测的点</param>
    /// <returns>
    /// 如果输入点在Revit设计限制内，则为True，否则为false。
    /// </returns>
    /// <remarks>
    /// 用于验证几何图形构造方法的输入。
    /// </remarks> 
    public static bool IsWithinLengthLimits(XYZ point)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// 指示此矢量是否为零矢量。
    /// </summary>
    /// <remarks>
    /// 零矢量的每个分量在公差（Config.TOLERANCE）内为零。
    /// </remarks> 
    public bool IsZeroLength()
    {
        return IsAlmostEqualTo(Zero);
    }

    /// <summary>
    /// 将此矢量乘以指定的值并返回结果。
    /// </summary>
    /// <param name="value">要与此向量相乘的值。</param>
    /// <returns>相乘后的矢量。</returns> 
    /// <remarks>
    /// 相乘的矢量是通过将该矢量的每个坐标乘以指定值而获得的。
    /// </remarks>
    public XYZ Multiply(double value)
    {
        return New(X * value, Y * value, Z * value);
    }

    /// <summary>对该向量求反。</summary>
    /// <returns>与此向量相对的向量。</returns>
    /// <remarks>
    /// 通过改变该向量的每个坐标的符号，得到求反的向量。
    /// </remarks>
    public XYZ Negate()
    {
        return New(-X, -Y, -Z);
    }

    /// <summary>
    /// 返回一个新的XYZ，其坐标是此矢量的标准化值。
    /// </summary>
    /// <returns>
    /// 归一化的XYZ或零向量（如果矢量几乎为零）。
    /// </returns>
    /// <remarks>
    /// Normalized表示此向量的长度等于1（单位向量）。
    /// </remarks> 
    public XYZ Normalize()
    {
        double length = Math.Sqrt(X * X + Y * Y + Z * Z);
        if (length < Config.TOLERANCE) // 防止除以0的情况
        {
            return New(0, 0, 0); // 返回零向量
        }
        return New(X / length, Y / length, Z / length); // 返回归一化的向量
    }

    /// <summary>
    /// 从该矢量中减去指定的矢量并返回结果。
    /// </summary>
    /// <param name="source">要从该向量中减去的向量。</param>
    /// <returns>两个矢量之差的矢量。</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception>
    /// <remarks>
    /// 减去的矢量是通过从该矢量的对应坐标减去指定矢量的每个坐标而获得的。
    /// </remarks>
    public XYZ Subtract(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return New(X - source.X, Y - source.Y, Z - source.Z);
    }

    /// <summary>
    /// 获取显示（X、Y、Z）的格式化字符串，其中的值格式化为9位小数。
    /// </summary>
    public override string ToString()
    {
        return "(" +
            X.ToString("F9") + ", " +
            Y.ToString("F9") + ", " +
            Z.ToString("F9") + ")";
    }

    /// <summary>这个向量和两个指定向量的三乘积。</summary>
    /// <param name="middle">第二个矢量。</param>
    /// <param name="right">第三个矢量。</param>
    /// <returns>三乘积实数结果</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当middle或right为null时抛出
    /// </exception>
    /// <remarks>
    /// 标量三乘积被定义为其中一个向量与另两个向量的叉积的点积。
    /// 在几何上，这个乘积是由给定的三个向量形成的平行六面体的（有符号）体积。
    /// </remarks>
    public double TripleProduct(XYZ middle, XYZ right)
    {
        if (middle == null)
        {
            throw new ArgumentNullException(
                nameof(middle),
                "middle不可为null。");
        }

        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不可为null。");
        }

        // 计算叉积
        XYZ crossProduct = CrossProduct(middle);
        // 计算点积
        double dotProduct = crossProduct.DotProduct(right);

        return dotProduct;
    }

    #endregion 方法

    #region 操作符号

    /// <summary>
    /// 将两个指定的矢量相加并返回结果。
    /// </summary>
    /// <param name="left">第一个向量。</param>
    /// <param name="right">第二个向量。</param>
    /// <returns>等于两个源向量之和的向量。</returns> 
    /// <exception cref="ArgumentNullException">
    /// 当left或right为null时抛出
    /// </exception>
    /// <remarks>
    /// 通过将右向量的每个坐标添加到左向量的相应坐标来获得添加的向量。
    /// </remarks>
    public static XYZ operator +(XYZ left, XYZ right)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不可为null。");
        }

        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不可为null。");
        }

        return left.Add(right);
    }

    /// <summary>
    /// 将指定向量除以指定值。
    /// </summary>
    /// <param name="left">要被向量除的值。</param>
    /// <param name="value">要除以值的向量。</param>
    /// <returns>除法操作后的向量</returns>
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出 
    /// </exception>
    /// <exception cref="ArgumentException">
    /// 值为0抛出
    /// </exception>
    /// <remarks>
    /// 通过将指定向量的每个坐标除以指定值来获得分割向量。
    /// </remarks>
    public static XYZ operator /(XYZ left, double value)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不可为null。");
        }
        if (Math.Abs(value) < Config.TOLERANCE)
        {
            throw new ArgumentException(
                nameof(value),
                "value不可为0。");
        }

        return left.Divide(value);
    }

    /// <summary>
    /// 将指定的数字与指定的向量相乘。
    /// </summary>
    /// <param name="value">要与指定向量相乘的值。</param>
    /// <param name="right">要与值相乘的向量。</param>
    /// <returns>乘以的向量。</returns>
    /// <exception cref="ArgumentNullException">
    /// 当right为null时抛出 
    /// </exception>
    /// <remarks>
    /// 将指定向量的每个坐标乘以指定值，即可获得相乘向量。
    /// </remarks>
    public static XYZ operator *(double value, XYZ right)
    {
        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不可为null。");
        }
        return right.Multiply(value);
    }

    /// <summary>
    /// 将指定的向量与指定的数字相乘。
    /// </summary>
    /// <param name="left">要与值相乘的向量。</param>
    /// <param name="value">要与指定向量相乘的值。</param>
    /// <returns>乘以的向量。</returns>
    /// <exception cref="ArgumentNullException">
    /// 当source为null时抛出
    /// </exception> 
    /// <remarks>
    /// 将指定向量的每个坐标乘以指定值，即可获得相乘向量。
    /// </remarks>
    public static XYZ operator *(XYZ left, double value)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不可为null。");
        }
        return left.Multiply(value);
    }

    /// <summary>
    /// 两个指定的向量相减并返回结果。
    /// </summary>
    /// <param name="left">第一个向量</param>
    /// <param name="right">第二个向量</param>
    /// <returns>
    /// 等于两个源向量之差的向量。
    /// </returns> 
    /// <exception cref="ArgumentNullException">
    /// 当left或right为null时抛出
    /// </exception>
    /// <remarks>
    /// 减去的向量通过从左向量的对应坐标减去右向量的每个坐标获得。
    /// </remarks>
    public static XYZ operator -(XYZ left, XYZ right)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不可为null。");
        }

        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不可为null。");
        }
        return left.Subtract(right);
    }

    /// <summary>
    /// 对指定的向量求反并返回结果。
    /// </summary>
    /// <returns>
    /// 与指定向量相反的向量。
    /// </returns> 
    /// <exception cref="ArgumentNullException">
    /// 当left或right为null时抛出
    /// </exception>
    /// <remarks>
    /// 通过改变指定向量的每个坐标的符号来获得求反向量。
    /// </remarks>
    public static XYZ operator -(XYZ source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return source.Negate();
    }

    #endregion 操作符号

    #region 静态属性

    /// <summary>
    /// X轴的单位向量。
    /// </summary>
    /// <remarks>
    /// X轴的单位向量是（1,0,0），即X轴上的单位向量。
    /// </remarks>
    public static XYZ BasisX
    {
        get
        {
            return New(1.0, 0.0, 0.0);
        }
    }

    /// <summary>
    /// Y轴的单位向量。
    /// </summary>
    /// <remarks>
    /// Y轴的单位向量是（0,1,0），即Y轴上的单位向量。
    /// </remarks>
    public static XYZ BasisY
    {
        get
        {
            return New(0.0, 1.0, 0.0);
        }
    }

    /// <summary>
    /// Z轴的单位向量。
    /// </summary>
    /// <remarks>
    /// Z轴的单位向量是（0,0,1），即Z轴上的单位向量。
    /// </remarks>
    public static XYZ BasisZ
    {
        get
        {
            return New(0.0, 0.0, 1.0);
        }
    }

    /// <summary>
    /// 坐标原点或零矢量。
    /// </summary>
    /// <remarks>
    /// 零矢量为（0,0,0）。
    /// </remarks>
    public static XYZ Zero
    {
        get
        {
            return New(0.0, 0.0, 0.0);
        }
    }

    #endregion 静态属性


}
