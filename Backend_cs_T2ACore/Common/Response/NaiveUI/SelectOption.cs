using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 对应NaiveUI的Select等控件的选项
/// </summary>
public class SelectOption<T>
{
    /// <summary>
    /// 自定义一个选项的类名
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// 是否禁用一个选项
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 选项显示内容
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 存储值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="label"></param>
    /// <param name="val"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static SelectOption<T> New(string label, T val, bool disabled = false)
    {
        var res = new SelectOption<T>();
        res.Label = label;
        res.Value = val;
        res.Disabled = disabled;
        return res;
    }

    /// <summary>
    /// 构造函数，label和value相同
    /// </summary>
    /// <param name="label"></param> 
    /// <param name="disabled"></param> 
    /// <returns></returns>
    public static SelectOption<string> New(string label, bool disabled = false)
    {
        var res = new SelectOption<string>();
        res.Label = label;
        res.Value = label;
        res.Disabled = disabled;
        return res;
    }

    /// <summary>
    /// 构造函数，label和value相同
    /// </summary>
    /// <param name="labels"></param> 
    /// <returns></returns>
    public static List<SelectOption<string>> NewList(List<string> labels)
    {
        var res = new List<SelectOption<string>>();
        foreach (var label in labels)
        {
            res.Add(SelectOption<string>.New(label));
        }
        return res;
    }

}
