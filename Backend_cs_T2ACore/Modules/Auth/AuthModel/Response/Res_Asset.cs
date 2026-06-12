using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace T2ACore;

/// <summary>
/// 资源返回值
/// </summary>
public class Res_Asset
{
    /// <summary>
    /// 角色维度 g，g2...
    /// </summary>
    public string RoleRegion { get; set; }

    /// <summary>
    /// 角色名称 Value1
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// 租户 Value2
    /// </summary>
    public string Tenant { get; set; }

    /// <summary>
    /// 资源类别 Type p p2 
    /// </summary>
    public string AssetType { get; set; }

    /// <summary>
    /// 资源类别名称 Value3 
    /// </summary>
    public string AssetName { get; set; }

    /// <summary>
    /// 特征值 Value4，Value5  。。。。
    /// </summary>
    public List<string> Patterns { get; set; }

    /// <summary>
    /// 转化为返回值
    /// </summary>
    /// <param name="inClass"></param>
    /// <param name="roleRegion"></param>
    /// <returns></returns>
    public static Res_Asset Cast(Auth_assets_Class inClass, string roleRegion)
    {
        var newModel = new Res_Asset();
        newModel.RoleRegion = roleRegion;
        newModel.RoleName = inClass.Value1;
        newModel.Tenant = inClass.Value2;
        newModel.AssetType = inClass.Type;
        newModel.AssetName = inClass.Value3;

        var list = new List<string>();
        for (int i = 4; i <= 12; i++)
        {
            PropertyInfo pInfo = AuthCommon.GetAssetPatternName(i);
            var val = pInfo.GetValue(inClass);
            if (val == null)
            {
                break;
            }
            else
            {
                if (string.IsNullOrEmpty(val.ToString()))
                {
                    break;
                }
                else
                {
                    list.Add(val.ToString());
                }
            }
        }

        newModel.Patterns = list;
        return newModel;
    }
}

/// <summary>
/// 比较类
/// </summary>
public class AssetCompare : IEqualityComparer<Res_Asset>
{
    /// <summary>
    /// 比较方法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Equals(Res_Asset x, Res_Asset y)
    {
        if (x.Tenant != y.Tenant)
        {
            return false;
        }

        if (x.AssetType != y.AssetType)
        {
            return false;
        }

        if (x.AssetName != y.AssetName)
        {
            return false;
        }

        int a = x.Patterns.Count;
        int b = y.Patterns.Count;

        if (a != b)
        {
            return false;
        }

        for (int i = 0; i < a; i++)
        {
            if (x.Patterns[i] != y.Patterns[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 计算HashCode
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int GetHashCode([DisallowNull] Res_Asset obj)
    {
        string cal =
            //obj.RoleRegion + //g g2，不同角色维度可以不同
            //obj.RoleName +  //Value1不考虑，不同角色可以不同
            obj.Tenant +
            obj.AssetType +
            obj.AssetName;

        foreach (var pattern in obj.Patterns)
        {
            cal += pattern;
        }

        return cal.GetHashCode();
    }
}
