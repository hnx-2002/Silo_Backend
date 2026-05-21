using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 旋转
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </summary>
public class Revolution : GenericForm
{
    #region 构造函数，静态方法
    /// <summary>
    /// 使用曲线元素的轮廓及旋转轴将新的旋转形式添加到族文档中。
    /// </summary>
    /// <param name="isSolid">指示旋转是实心旋转还是空心旋转。</param>
    /// <param name="profile">
    /// 边界线(多组闭合轮廓)，应该是2D，其中所有输入曲线都位于一个平面中
    ///【IMPORTANT!!!】原方法类型为CurveArrArray,现改为List List Curve
    /// </param>
    /// <param name="profilePlane">
    ///【IMPORTANT!!!】原方法类型为SketchPlane,现改为Plane
    /// 轮廓所在平面。创建现有平面上的二维轮廓 。
    /// </param>
    /// <param name="axis"> 
    /// 新创建的旋转的轮廓。这可能包含多个曲线回路或轮廓族。
    /// 轮廓必须位于XY平面中，它将自动转换为轮廓平面。
    /// 每个循环必须是一个完全闭合的曲线循环，并且循环不得相交。
    /// 回路可以是未绑定的圆或椭圆，但其几何图形将一分为二，以满足旋转中使用的草图的要求。
    /// </param>
    /// <param name="startAngle">
    /// 起始弧度
    /// </param>
    /// <param name="endAngle">
    /// 终止弧度
    /// </param>
    /// <returns>如果创建成功，则返回新的Revolution，否则将引发包含失败信息的异常。</returns>
    /// <exception cref="ArgumentException">当输入参数路径为null或为空时抛出。</exception>
    /// <exception cref="ArgumentNullException">当输入参数配置文件为null时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当输入参数profileLocationCurveIndex超出索引界限时抛出。
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当输入参数profilePlaneLocation在profilePlaneLocation枚举中不存在时抛出。
    /// </exception>
    /// <exception cref="InvalidOperationException">在无法创建旋转的概念体量、二维或其他族中尝试创建时抛出。</exception>
    /// <exception cref="InvalidOperationException">创建失败时抛出。</exception>
    /// <remarks>
    /// 此方法在族文档中创建旋转。旋转将沿着路径跟踪轮廓。
    /// </remarks>
    public static Revolution New(bool isSolid, List<List<Curve>> profile,
        Plane profilePlane, Line axis, double startAngle, double endAngle)
    {
        if (profile == null)
        {
            throw new ArgumentNullException("profile不可为null");
        }

        if (profilePlane == null)
        {
            throw new ArgumentNullException("profilePlane不可为null");
        }

        if (axis == null)
        {
            throw new ArgumentNullException("axis不可为null");
        }

        if (startAngle == double.NaN)
        {
            throw new ArgumentNullException("startAngle不可为null");
        }

        if (endAngle == double.NaN)
        {
            throw new ArgumentNullException("endAngle不可为null");
        }

        if (startAngle == endAngle)
        {
            throw new ArgumentOutOfRangeException("profileLocationCurveIndex索引不可小于0");
        }

        var result = new Revolution();
        result.IsSolid = isSolid;
        result.Profile = profile;
        result.ProfilePlane = profilePlane;
        result.Axis = axis;
        result.StartAngle = startAngle;
        result.EndAngle = endAngle;

        return result;
    }
    #endregion 构造函数，静态方法

    #region 属性 

    /// <summary>
    /// 边界线：多组轮廓的集合
    /// </summary>
    public List<List<Curve>> Profile { get; set; }

    /// <summary>
    /// 轮廓所在平面
    /// </summary>
    public Plane ProfilePlane { get; set; }

    /// <summary>
    /// 旋转轴线
    /// </summary>
    public Curve Axis { get; set; }

    /// <summary>
    /// 起始弧度
    /// </summary>
    public double StartAngle { get; set; }

    /// <summary>
    /// 终止弧度
    /// </summary>
    public double EndAngle { get; set; }

    #endregion 属性

    #region 重写属性
    /// <summary>
    /// 实体类型
    /// </summary>
    public override GenericFormType GenericFormType
    {
        get
        {
            return GenericFormType.Revolution;
        }
    }
    #endregion 重写属性
}
