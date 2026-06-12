using SqlSugar;
using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 角色树
/// </summary>
[SugarTable("auth_assets")]
public class RoleList
{ 
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(
        ColumnName = "Section",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255
    )]
    public string Section { get; set; }


    /// <summary>
    /// 角色维度
    /// </summary> 
    [SugarColumn(
        ColumnName = "Type",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255
    )]

    public string Region { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary> 
    [SugarColumn(
        ColumnName = "Value1",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255
    )]
    public string RoleCode { get; set; }

    /// <summary>
    /// 被拥有的角色
    /// </summary>
    [SugarColumn(
        IsTreeKey = true,
        ColumnName = "Value2",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255)]
    public string UpRole { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    [SugarColumn(
        ColumnName = "Value3",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255)]
    public string Tenant { get; set; }

    /// <summary>
    /// 是个人还是角色
    /// </summary>
    [SugarColumn(
        ColumnName = "Value4",
        ColumnDataType = "varchar",
        ColumnDescription = "",
        Length = 255)]
    public string IsPerson { get; set; }

    /// <summary>
    /// 创建人姓名
    /// </summary>
    [SugarColumn(
        ColumnName = "create_username",
        ColumnDataType = "varchar",
        ColumnDescription = "创建人姓名",
        Length = 255,
        IsNullable = true 
    )]
    public string Create_username { get; set; }

    /// <summary>
    /// 创建时间{Default:Now}
    /// </summary>
    [SugarColumn(
        ColumnName = "create_time",
        ColumnDataType = "datetime",
        ColumnDescription = "创建时间{Default:Now}",
        Length = 0,
        IsNullable = true 
    )]
    public DateTime Create_time { get; set; } = DateTime.Now; //感谢yf关于默认值的建议！20220324


    /// <summary>
    /// 转化方法
    /// </summary> 
    /// <returns></returns>
    public RoleTree CastToTree()
    {
        var newModel = new RoleTree();
        newModel.Section = Section;
        newModel.Region = Region;
        newModel.RoleCode = RoleCode;
        newModel.UpRole = UpRole;
        newModel.Tenant = Tenant;
        newModel.IsPerson = IsPerson;
        newModel.Havings = new List<RoleTree>();
        newModel.Create_time = Create_time;
        newModel.Create_username = Create_username;
        return newModel;
    }
}
