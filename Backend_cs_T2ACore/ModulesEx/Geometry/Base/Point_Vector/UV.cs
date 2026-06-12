using System;

namespace TPGeometryPro;

/// <summary>
/// 二维空间坐标系
/// </summary>
/// <remarks>
/// 通常这意味着曲面上的参数。在实际使用中，
/// 它可以被解释为二维空间中的点或向量。
/// </remarks>
public class UV
{
    #region 静态构造方法

    /// <summary>
    /// 构造函数
    /// 根据提供的坐标系，创建UV
    /// </summary>
    /// <param name="u">第一坐标</param>
    /// <param name="v">第二坐标</param>
    /// <exception cref="ArgumentException">
    /// 当为U或V特性设置无穷大的数时抛出。
    /// </exception>
    public static UV New(double u, double v)
    {
        var result = new UV();
        result.U = u;
        result.V = v;
        return result;
    }


    #endregion 静态构造方法

    #region 属性

    /// <summary>
    /// 第idx个坐标值
    /// </summary> 
    /// <returns>返回第idx个坐标值</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// 索引只能为0,1
    /// </exception>
    public double this[int idx]
    {
        get
        {
            if (idx == 0)
            {
                return U;
            }
            else if (idx == 1)
            {
                return V;
            }
            else
            {
                throw new IndexOutOfRangeException("索引只能为0,1");
            }
        }
    }

    /// <summary>
    /// 坐标系第一值
    /// </summary>
    public double U { get; set; }

    /// <summary>
    /// 坐标系第二值
    /// </summary>
    public double V { get; set; }

    #endregion 属性



    #region 方法

    /// <summary>
    /// 将指定的二维矢量添加到此二维矢量并返回结果
    /// </summary>
    /// <param name="source">要添加到此矢量的矢量</param>
    /// <returns>二维矢量等于两个矢量之和</returns>
    /// <exception cref="ArgumentNullException">
    /// 当源为null时抛出
    /// </exception>
    /// <remarks>
    /// 相加的矢量是通过将指定矢量的每个坐标与该矢量的对应坐标相加而获得的
    /// </remarks>
    public UV Add(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不能为null。");
        }

        return New(U + source.U, V + source.V);
    }

    /// <summary>
    /// 返回此矢量与指定矢量之间的角度
    /// </summary>
    /// <param name="source">指定的矢量</param>
    /// <returns>0和2之间的实数*PI等于两个矢量之间的角度（以弧度为单位）。</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception>
    /// <exception cref="InvalidOperationException">其中一个向量的长度为零</exception>
    /// <remarks>
    /// 角度是逆时针测量的。
    /// </remarks>
    public double AngleTo(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不能为null。");
        }

        // 假设 UV 是一个向量，且具有 U,V 作为其分量的属性。
        // 这个计算使用点积和向量的大小（范数）来找到角度。
        double dotProduct = U * source.U + V * source.V;
        double magnitudeA = Math.Sqrt(U * U + V * V);
        double magnitudeB = Math.Sqrt(source.U * source.U + source.V * source.V);

        // 防止除以零
        if (magnitudeA == 0 || magnitudeB == 0)
        {
            throw new InvalidOperationException("其中一个向量的长度为零。");
        }

        double cosAngle = dotProduct / (magnitudeA * magnitudeB);
        // 确保余弦值在 [-1, 1] 范围内，避免 NaN 结果
        //cosAngle = cosAngle.Clamp(-1.0, 1.0);  //Math.Clamp

        if (cosAngle.CompareTo(-1.0) < 0) cosAngle = -1.0;
        if (cosAngle.CompareTo(1.0) > 0) cosAngle = 1.0;

        // 以弧度计算角度
        double angle = Math.Acos(cosAngle);

        return angle;
    }

    /// <summary>
    /// 此二维矢量与指定二维矢量的叉积
    /// </summary>
    /// <param name="source">要与此向量相乘的向量</param>
    /// <returns>实数等于叉积</returns>
    /// <exception cref="ArgumentNullException">当源为null时抛出</exception> 
    /// <remarks>二维空间中两个矢量的叉积等于它们所跨越的平行四边形的面积</remarks>
    public double CrossProduct(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "源向量不能为null");
        }
        return U * source.V - V * source.U;
    }

    /// <summary>
    /// 返回从该二维点到指定二维点的距离
    /// </summary>
    /// <param name="source">指定的点</param>
    /// <returns>实数,等于两点之间的距离</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception> 
    /// <remarks>
    /// 两点之间的距离等于连接两点的向量的长度
    /// </remarks>
    public double DistanceTo(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return Math.Sqrt(
            Math.Pow(U - source.U, 2) +
            Math.Pow(V - source.V, 2));
    }

    /// <summary>
    /// 将此二维矢量除以指定值并返回结果
    /// </summary>
    /// <param name="value">将此矢量除以的值</param>
    /// <returns>分割后的二维矢量</returns> 
    /// <exception cref="ArgumentException">当指定的值为零时抛出</exception> 
    /// <remarks>
    /// 分割后的矢量是通过将该矢量的每个坐标除以指定值而获得的
    /// </remarks>
    public UV Divide(double value)
    {
        if (Math.Abs(value) < Config.TOLERANCE) // 使用一个非常小的值来判断value是否接近于0
        {
            throw new DivideByZeroException("value不能为零。");
        }

        return New(U / value, V / value);

    }

    /// <summary>
    /// 此二维矢量与指定二维矢量的点积
    /// </summary>
    /// <param name="source">要与此向量相乘的向量</param>
    /// <returns>等于点积的实数</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception>  
    /// <remarks>
    /// 点积是两个矢量Pu*Ru+Pv*Rv各自坐标的总和
    /// </remarks>
    public double DotProduct(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return U * source.U + V * source.V;
    }

    /// <summary>
    /// 2D向量的长度
    /// </summary>
    /// <returns>
    /// 在二维欧几里得空间中，向量的长度是两个坐标之和的平方根。
    /// </returns>
    public double GetLength()
    {
        return Math.Sqrt(U * U + V * V);
    }

    /// <summary>
    /// 确定此二维矢量和指定的二维矢量在公差（Config.TOLERANCE）内是否相同
    /// </summary>
    /// <param name="source">要与此矢量进行比较的矢量</param>
    /// <returns>如果矢量相同，则为True；否则为false</returns>
    /// <exception cref="ArgumentNullException">当left为null时抛出</exception> 
    public bool IsAlmostEqualTo(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return Math.Abs(U - source.U) < Config.TOLERANCE &&
               Math.Abs(V - source.V) < Config.TOLERANCE;

    }

    /// <summary>
    /// 确定此二维矢量和指定的二维矢量在指定公差内是否相同
    /// </summary>
    /// <param name="source">要与此矢量进行比较的矢量</param>
    /// <param name="tolerance">相等检查的公差</param>
    /// <returns>如果矢量相同，则为True；否则为false</returns>
    /// <exception cref="ArgumentNullException">当源为null时抛出</exception>
    /// <exception cref="ArgumentException">当公差小于0时抛出</exception>
    public bool IsAlmostEqualTo(UV source, double tolerance)
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

        return Math.Abs(U - source.U) < tolerance &&
               Math.Abs(V - source.V) < tolerance;
    }

    /// <summary>
    /// 布尔值指示此二维矢量是否具有单位长度。
    /// </summary>
    /// <returns>
    /// 单位长度向量的长度为1，并且被认为是归一化的。
    /// </returns>
    public bool IsUnitLength()
    {
        return Math.Abs(GetLength() - 1) < Config.TOLERANCE;
    }

    /// <summary>
    /// 布尔值指示此二维矢量是否为零矢量。
    /// </summary>
    /// <returns>
    /// 零矢量的每个分量在公差（Config.TOLERANCE）内为零。
    /// </returns>
    public bool IsZeroLength()
    {
        return GetLength() < Config.TOLERANCE;
    }


    /// <summary>
    /// 将此二维矢量乘以指定值并返回结果
    /// </summary>
    /// <param name="value">要与此向量相乘的值</param>
    /// <returns>相乘后的二维矢量</returns>
    /// <remarks>
    /// 相乘的矢量是通过将该矢量的每个坐标乘以指定值而获得的
    /// </remarks> 
    public UV Multiply(double value)
    {
        return New(U * value, V * value);
    }

    /// <summary>
    /// 否定此二维矢量
    /// </summary>
    /// <returns>与此矢量相反的二维矢量</returns>
    /// <remarks>
    /// 求反的矢量是通过改变该矢量的每个坐标的符号而获得的
    /// </remarks>
    public UV Negate()
    {
        return New(-U, -V);
    }

    /// <summary>
    /// 返回一个新UV，该UV的坐标是该向量的归一化值。
    /// </summary>
    /// <returns>
    /// 归一化UV或零（如果矢量几乎为零）。
    /// </returns>
    /// <remarks>
    /// Normalized表示此向量的长度等于1（单位向量）。
    /// </remarks>
    public UV Normalize()
    {
        double length = Math.Sqrt(U * U + V * V);
        if (length < Config.TOLERANCE) // 防止除以0的情况
        {
            return new UV(); // 返回零向量
        }
        return New(U / length, V / length); // 返回归一化的向量

    }

    /// <summary>
    /// 从该二维矢量中减去指定的二维矢量并返回结果。
    /// </summary>
    /// <param name="source">要从该向量中减去的向量。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出。</exception> 
    /// <remarks>
    /// 减去的矢量是通过从该矢量的对应坐标减去指定矢量的每个坐标而获得的。
    /// </remarks>
    public UV Subtract(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }
        return New(U - source.U, V - source.V);
    }

    /// <summary>
    /// 获取显示（U，V）的格式化字符串，其中值的格式为小数点后9位。
    /// </summary>
    /// <returns>格式化字符串</returns>
    public override string ToString()
    {
        return "(" +
             U.ToString("F9") + ", " +
             V.ToString("F9") + ")";
    }


    #endregion 方法

    #region 操作符
    /// <summary>
    /// 将两个指定的二维矢量相加并返回结果。
    /// </summary>
    /// <param name="left">第一个向量</param>
    /// <param name="right">第二个向量</param>
    /// <returns>
    /// 二维矢量等于两个源矢量之和。
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// 当left或right为null时抛出。
    /// </exception>
    /// <remarks>
    /// 通过将右向量的每个坐标与左向量的对应坐标相加来获得相加的向量。
    /// </remarks>
    public static UV operator +(UV left, UV right)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不能为null。");
        }

        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不能为null。");
        }

        return left.Add(right);
    }

    /// <summary>
    /// 将指定的二维矢量除以指定的值
    /// </summary>
    /// <param name="left">向量除以的值</param>
    /// <param name="value">要除以值的向量</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">当left为null时抛出</exception>
    /// <exception cref="ArgumentException">当指定的值是无穷大的数或零时抛出</exception>
    /// <remarks>
    /// 通过将指定向量的每个坐标除以指定值来获得分割向量
    /// </remarks>
    public static UV operator /(UV left, double value)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不能为null。");
        }

        if (Math.Abs(value) <= Config.TOLERANCE)
        {
            throw new ArgumentException(
                nameof(value),
                "value不可等于0");
        }

        return left.Divide(value);
    }


    /// <summary>
    /// 指定的数字和指定的二维矢量的乘积
    /// </summary>
    /// <param name="value">要与指定向量相乘的值</param>
    /// <param name="right">要与值相乘的向量</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">当right为空时抛出</exception> 
    /// <remarks>
    /// 通过将指定向量的每个坐标乘以指定值来获得相乘的向量。
    /// </remarks>
    public static UV operator *(double value, UV right)
    {
        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不能为null。");
        }

        return right.Multiply(value);
    }

    /// <summary>
    /// 指定的数字和指定的二维矢量的乘积。
    /// </summary>
    /// <param name="left">要与值相乘的向量</param>
    /// <param name="value">要与指定向量相乘的值</param>
    /// <returns></returns>
    /// <exception cref=" ArgumentNullException">当left为null时抛出</exception> 
    /// <remarks>
    /// 通过将指定向量的每个坐标乘以指定值来获得相乘的向量。
    /// </remarks>
    public static UV operator *(UV left, double value)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不能为null。");
        }

        return left.Multiply(value);
    }


    /// <summary>
    /// 减去两个指定的二维矢量并返回结果。
    /// </summary>
    /// <param name="left">第一个矢量</param>
    /// <param name="right">第二个矢量</param>
    /// <returns>二维矢量等于两个源矢量之间的差</returns>
    /// <exception cref="ArgumentNullException">当left或right为null时抛出。</exception>
    /// <remarks>
    /// 减去的矢量是通过从左矢量的对应坐标减去右矢量的每个坐标而获得的。
    /// </remarks>
    public static UV operator -(UV left, UV right)
    {
        if (left == null)
        {
            throw new ArgumentNullException(
                nameof(left),
                "left不能为null。");
        }

        if (right == null)
        {
            throw new ArgumentNullException(
                nameof(right),
                "right不能为null。");
        }

        return left.Subtract(right);
    }

    /// <summary>
    /// 求反此二维矢量并返回结果。
    /// </summary>
    /// <param name="source"></param>
    /// <returns>
    /// 与此矢量相反的二维矢量。
    /// </returns>
    /// <exception cref="ArgumentNullException">当源为null时抛出。</exception> 
    /// <remarks>
    /// 求反的矢量是通过改变的每个坐标的符号来获得的
    /// </remarks>
    public static UV operator -(UV source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source),
                "source不可为null。");
        }

        return source.Negate();
    }


    #endregion 操作符

    #region 静态属性

    /// <summary>
    /// U轴单位向量
    /// </summary>
    /// <remarks>
    /// U轴的基础是向量（1,0），即U轴上的单位向量。
    /// </remarks>  
    public static UV BasisU
    {
        get
        {
            return New(1.0, 0.0);
        }
    }

    /// <summary>
    /// V轴单位向量
    /// </summary>
    /// <remarks>
    /// V轴的基础是向量（0,1），即V轴上的单位向量。
    /// </remarks> 
    public static UV BasisV
    {
        get
        {
            return New(0.0, 1.0);
        }
    }

    /// <summary>
    /// 二维原点，或者二维0向量
    /// </summary>
    /// <remarks>
    /// 0向量为(0,0)
    /// </remarks> 
    public static UV Zero
    {
        get
        {
            return New(0.0, 0.0);
        }
    }
    #endregion 静态属性


}

