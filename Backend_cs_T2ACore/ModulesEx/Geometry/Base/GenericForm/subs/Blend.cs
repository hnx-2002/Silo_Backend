using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 融合
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </summary>
public class Blend : GenericForm
{
    #region 构造函数，静态方法

    #endregion 构造函数，静态方法

    #region 属性

    #endregion 属性

    #region 重写属性
    /// <summary>
    /// 实体类型
    /// </summary>
    public override GenericFormType GenericFormType
    {
        get
        {
            return GenericFormType.Blend;
        }
    }
    #endregion 重写属性
}
