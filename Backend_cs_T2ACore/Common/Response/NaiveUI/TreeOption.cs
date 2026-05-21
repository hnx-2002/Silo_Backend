using System.Collections.Generic;
using System.Linq;

namespace T2ACore;

/// <summary>
/// 对应NaiveUI的Tree等控件的选项
/// </summary>
public class TreeOption<TKey>
{
    /// <summary>
    /// 节点的 key，需要唯一，可使用 key-field 修改字段名
    /// </summary>
    public TKey Key { get; set; }

    /// <summary>
    /// 节点的内容，可使用 label-field 修改字段名
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 是否禁用节点的 checkbox
    /// </summary>
    public string CheckboxDisabled { get; set; }

    /// <summary>
    /// 是否禁用节点
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 节点是否是叶节点，在异步展开状态下是必须的
    /// </summary>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// 节点的前缀
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// 节点的后缀
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 节点的子节点
    /// </summary>
    public List<TreeOption<TKey>> Children { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="label"></param>
    /// <param name="key"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static TreeOption<TKey> New(string label, TKey key, bool disabled = false)
    {
        var res = new TreeOption<TKey>();
        res.Label = label;
        res.Key = key;
        res.Disabled = disabled;
        res.IsLeaf = false;
        res.Children = new List<TreeOption<TKey>>();
        return res;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="label"></param>
    /// <param name="key"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static TreeOption<TKey> NewLeaf(string label, TKey key, bool disabled = false)
    {
        var res = new TreeOption<TKey>();
        res.Label = label;
        res.Key = key;
        res.Disabled = disabled;
        res.IsLeaf = true;
        res.Children = null;
        return res;
    }


    /// <summary>
    /// 创建树形集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="idName"></param>
    /// <param name="parentIdName"></param>
    /// <param name="labelName"></param>
    /// <returns></returns>
    public static List<TreeOption<TKey>> BuildTree<T>(List<T> items,
        string idName, string parentIdName, string labelName)
    {
        var idProperty = typeof(T).GetProperty(idName);
        var parentIdProperty = typeof(T).GetProperty(parentIdName);
        var labelProperty = typeof(T).GetProperty(labelName);

        var itemDict = items.ToDictionary(
            item => (TKey)idProperty.GetValue(item),
            item => item
        );

        var tree = new List<TreeOption<TKey>>();
        var lookup = new Dictionary<TKey, TreeOption<TKey>>();

        foreach (var item in items)
        {
            var id = (TKey)idProperty.GetValue(item);
            var parentId = (TKey)parentIdProperty.GetValue(item);
            var label = labelProperty?.GetValue(item)?.ToString();

            if (!lookup.ContainsKey(id))
            {
                lookup[id] = New(label, id);
            }

            var node = lookup[id];

            if (parentId == null || parentId.Equals(default(TKey)))
            {
                tree.Add(node);
            }
            else
            {
                if (!lookup.ContainsKey(parentId))
                {
                    lookup[parentId] = New(label, parentId);
                }

                lookup[parentId].Children.Add(node);
            }
        }

        foreach (var node in lookup.Values)
        {
            if (node.Children.Count == 0)
            {
                var leafNode = NewLeaf(node.Label, node.Key, node.Disabled);
                lookup[node.Key] = leafNode;
            }
        }

        return tree;
    }
}
