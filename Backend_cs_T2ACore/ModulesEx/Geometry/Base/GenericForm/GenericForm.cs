using System;

namespace TPGeometryPro;

/// <summary>
/// 建模实体
/// </summary>
public class GenericForm
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否为实心
    /// </summary>
    public bool IsSolid { get; set; }

    /// <summary>
    /// 建模实体类型
    /// </summary>
    public virtual GenericFormType GenericFormType
    {
        get
        {
            throw new NotImplementedException();
        }
    }

}
