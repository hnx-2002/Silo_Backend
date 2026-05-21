using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace T2ACore;

/// <summary>
/// sqlSugar静态方法
/// </summary>
public static class FunSugar
{
    /// <summary>
    /// 构造实体类表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expressionable<T> GetEntityExpression<T>() where T : class, new()
    {
        return Expressionable.Create<T>();
    }

    /// <summary>
    /// 由文本获取Join类型
    /// </summary>
    /// <param name="joinType"></param>
    /// <returns></returns>
    public static JoinType GetJoinType(string joinType)
    {
        return joinType switch
        {
            "Full" => JoinType.Full,
            "Inner" => JoinType.Inner,
            "Left" => JoinType.Left,
            "Right" => JoinType.Right,
            _ => JoinType.Left,
        };
    }

    /// <summary>
    /// 直接返回 SqlSugar 官方结构类
    /// </summary>
    /// <param name="db">SqlSugarClient 实例</param>
    /// <returns>所有用户表结构（含列、主键、描述、类型、长度）</returns>
    public static List<(DbTableInfo table, List<DbColumnInfo> columns)> GetDbStruct(SqlSugarClient db)
    {
        return db.DbMaintenance
            .GetTableInfoList(false)                 // 1. 所有用户表
            .Select(t => (table: t, columns: db.DbMaintenance.GetColumnInfosByTableName(t.Name, false)))
            .ToList();
    }

    /// <summary>
    /// 从当前程序集所有打了 SugarTable 的实体类中，
    /// 返回与“数据库侧”相同的返回值 
    /// </summary>
    /// <param name="db"></param> 
    /// <param name="asm"></param> 
    public static List<TableDifferenceInfo> GetDbDiffInfos(
        SqlSugarScopeProvider db, Assembly asm)
    {
        asm ??= Assembly.GetExecutingAssembly();
        var allTypes = asm.GetTypes();
        var types = new List<Type>();
        foreach (var type in allTypes)
        {
            var tblAttr = type
                .GetCustomAttribute<SugarTable>();
            if (tblAttr != null &&
                type.BaseType == typeof(object))
            {
                types.Add(type);
            }
        }

        if (string.IsNullOrEmpty(db.CurrentConnectionConfig.ConnectionString))
        {
            return null;
        }

        var diffs = db.CodeFirst
            .GetDifferenceTables(types.ToArray())
            .ToDiffList();
        return diffs;
    }


}
