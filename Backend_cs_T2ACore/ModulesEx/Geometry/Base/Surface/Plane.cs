using System;

namespace TPGeometryPro;

/// <summary>
/// 平面
/// </summary>
/// <remarks>
/// 平面的参数方程为S(u, v) = origin + u*xVec + v*yVec.
/// </remarks>
public class Plane //: Surface
{
    #region 静态方法创建

    /// <summary>
    /// 创建一个平面对象，该对象通过作为参数提供的三个点 
    /// </summary>
    /// <param name="point1">定义唯一平面的三个点中的第一个。创建的平面对象将穿过这些点。</param>
    /// <param name="point2">定义唯一平面的三个点中的第二个。</param>
    /// <param name="point3">定义唯一平面的三个点中的第三个。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">非可选参数为NULL</exception>
    /// <exception cref="ArgumentException">输入点位于Revit设计限制之外</exception>
    /// <exception cref="ArgumentException">
    /// 如果输入点未定义唯一平面，则引发。
    /// 这通常是由于点彼此太近，或者所有三个点都在一条直线上或靠近一条直线造成的
    /// </exception>
    /// <remarks>
    /// 作为自变量提供的点必须完全定义一个平面：它们不能位于一条直线上，也不能彼此太近。
    /// 这些点必须位于Revit设计限制范围内。
    /// 此函数不保证对创建的平面进行特定的参数化。
    /// 使用Plane.Create(Frame)可强制对已创建的Plane对象进行特定的参数化。
    /// 所有三个点都应位于Revit设计限制Autodesk的范围内。 
    /// </remarks> 
    public static Plane CreateByThreePoints(XYZ point1, XYZ point2, XYZ point3)
    {
        if (point1 == null ||
            point2 == null ||
            point3 == null)
        {
            throw new ArgumentNullException("三点不可为null");
        }

        if (point1.IsAlmostEqualTo(point2) ||
            point2.IsAlmostEqualTo(point3) ||
            point3.IsAlmostEqualTo(point1))
        { 
            throw new ArgumentException("三点中有两点过于接近");
        }

        if (FunXYZ.AreCollinear(point1, point2, point3))
        { 
            throw new ArgumentException("三点共线");
        }

        try
        {
            double nX = (point2.Y - point1.Y) * (point3.Z - point1.Z) - (point2.Z - point1.Z) * (point3.Y - point1.Y);
            double nY = (point2.Z - point1.Z) * (point3.X - point1.X) - (point2.X - point1.X) * (point3.Z - point1.Z);
            double nZ = (point2.X - point1.X) * (point3.Y - point1.Y) - (point2.Y - point1.Y) * (point3.X - point1.X);

            XYZ normal = XYZ.New(nX, nY, nZ).Normalize();

            if (normal.IsZeroLength())
            {
                throw new ArgumentException("平面法向量为0向量，无法创建平面");
            }

            return CreateByNormalAndOrigin(normal, point1);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// 从法线和表示为XYZ对象的原点构造平面对象
    /// 遵循平面曲面的标准约定。构造的平面对象将穿过原点并垂直于法线。
    /// 平面的X轴和Y轴将被任意定义。
    /// </summary>
    /// <param name="normal">平面法线。应为有效的非零长度矢量。不需要是单位向量。</param>
    /// <param name="origin">平面原点。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">非可选参数为NULL</exception>
    /// <exception cref="ArgumentException">输入点位于Revit设计限制之外</exception>
    /// <exception cref="ArgumentOutOfRangeException">法线的长度为零</exception>
    /// <remarks>
    /// 此函数不保证对创建的平面进行特定的参数化。
    /// 使用平面。Create(Frame)可强制对已创建的Plane对象进行特定的参数化。
    /// </remarks> 
    public static Plane CreateByNormalAndOrigin(XYZ normal, XYZ origin)
    {
        if (normal == null)
        {
            throw new ArgumentNullException("normal不可为空");
        }

        if (origin == null)
        {
            throw new ArgumentNullException("origin不可为空");
        }

        if (normal.IsZeroLength())
        {
            throw new ArgumentOutOfRangeException("平面法向量为0向量，无法创建平面");
        }

        XYZ unitNormal = normal.Normalize();

        // 选择与法线不共线的参考向量
        XYZ reference = XYZ.BasisX;
        if (FunXYZ.IsParallel(unitNormal, reference))
        {
            reference = XYZ.BasisY;
            if (FunXYZ.IsParallel(unitNormal, reference))
            {
                reference = XYZ.BasisZ;
            }
        }

        // 生成正交基向量
        XYZ xAxis = reference.CrossProduct(unitNormal).Normalize();
        XYZ yAxis = unitNormal.CrossProduct(xAxis).Normalize();

        var plane = new Plane();
        plane.Origin = origin;
        plane.XVec = xAxis;
        plane.YVec = yAxis;
        plane.Normal = unitNormal;
        return plane;
    }

    /// <summary>
    /// 创建一个平面对象，该对象由两个正交的单位向量定义，并通过作为参数提供的原点。
    /// </summary>
    /// <param name="origin">平面原点。</param>
    /// <param name="basisX">定义平面的两个单位向量中的第一个。必须与第二个正交。</param>
    /// <param name="basisY">定义平面的两个单位向量中的第二个。必须与第一个正交。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">非可选参数为NULL</exception>
    /// <exception cref="ArgumentException">输入点位于Revit设计限制之外</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// basisX的长度不是1.0
    /// -或者-bassisY的长度不是1.0。
    /// </exception>
    /// <exception cref="ArgumentException">矢量basisX和basisY不垂直</exception>
    /// <remarks>
    /// 平面的参数方程为S(u, v) = origin + u*basisX + v*basisY.
    /// 平面的法线定义为basisX.Cross(basisY)
    /// </remarks>
    public static Plane CreateByOriginAndBasis(XYZ origin, XYZ basisX, XYZ basisY)
    {
        if (origin == null ||
            basisX == null ||
            basisY == null)
        {
            throw new ArgumentNullException("原点和向量不可为null");
        }

        if (Math.Abs(basisX.GetLength() - 1) > Config.TOLERANCE)
        {
            throw new ArgumentOutOfRangeException("basisX的长度不是1.0");
        }

        if (Math.Abs(basisY.GetLength() - 1) > Config.TOLERANCE)
        {
            throw new ArgumentOutOfRangeException("basisY的长度不是1.0");
        }

        if (Math.Abs(basisX.DotProduct(basisY)) > Config.TOLERANCE)
        {
            throw new ArgumentOutOfRangeException("平面上两单位向量不垂直");
        }

        var plane = new Plane();
        plane.Origin = origin;
        plane.XVec = basisX;
        plane.YVec = basisY;
        plane.Normal = basisX.CrossProduct(basisY);

        return plane;
    }

    #endregion 静态方法创建

    #region 属性

    /// <summary>
    /// 定义平面的第一个参数化方向的轴
    /// </summary>
    public XYZ XVec { get; set; }
    /// <summary>
    /// 定义平面的第二个参数化方向的轴
    /// </summary>
    public XYZ YVec { get; set; }

    /// <summary>
    /// 平面原点
    /// </summary>
    public XYZ Origin { get; set; }

    /// <summary>
    /// 平面法向量
    /// </summary>
    public XYZ Normal { get; set; }

    #endregion 属性

    #region 方法

    /// <summary>
    /// 将三维点正交投影到面上（以找到最近的点）。
    /// 如果投影失败，则抛出InvalidOperationException。
    /// 待测试
    /// </summary>
    /// <param name="point">三维投影点</param>
    /// <param name="uv">面上的UV坐标</param>
    /// <param name="distance">三维点到面上点的距离</param>
    /// <exception cref="ArgumentNullException">point为null是抛出异常</exception>
    public bool Project(XYZ point, out UV uv, out double distance)
    {
        uv = null;
        distance = 0.0;

        try
        {
            // 计算点P到平面的距离d（点A到点P的向量与法向量的点积，然后除以法向量的模）
            XYZ vectorAP = point - Origin;
            distance = vectorAP.DotProduct(Normal) / Normal.DotProduct(Normal);

            // 计算点P在平面上的正交投影点
            XYZ projection = point - Normal * distance;
            uv = projection.GetUV(Origin, XVec, YVec);
            return true;
        }
        catch (Exception)
        {
            return false;
            //throw new InvalidOperationException();
        }

    }




    #endregion 方法

    #region 需要进一步实现的方法

    // Dispose 继承自Surface 
    // Project  继承自Surface

    // ProjectWithGuessPoint  继承自Surface

    // Equals 继承自Object
    // GetHashCode  继承自Object
    // GetType 继承自Object 
    // ToString  继承自Object
    #endregion 需要进一步实现的方法

    #region 需要进一步实现的属性
    // IsValidObject 继承自Surface 
    // OrientationMatchesParametricOrientation  继承自Surface 
    #endregion 需要进一步实现的属性

}
