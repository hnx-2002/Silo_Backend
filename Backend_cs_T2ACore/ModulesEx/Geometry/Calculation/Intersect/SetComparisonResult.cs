namespace TPGeometryPro;

/// <summary>
/// 一种枚举类型，列出两个任意性质的集合之间的所有关系类型。
/// </summary>
public enum SetComparisonResult
{
    /// <summary>
    /// 左边的集合是空的，右边的集合不是。
    /// </summary>
    LeftEmpty = 1,

    /// <summary>
    /// 右边的集合是空的，左边的集合不是。
    /// </summary>
    RightEmpty = 2,

    /// <summary>
    /// 两个集合都是空的。
    /// </summary>
    BothEmpty = 3,

    /// <summary>
    /// 两个集合都不是空的，也没有重叠。
    /// </summary>
    Disjoint = 4,

    /// <summary>
    /// 两个集合的重叠不是空的，并且是二者的严格子集。
    /// </summary>
    Overlap = 8,

    /// <summary>
    /// 两个集合都不为空，左集合是右集合的严格子集。
    /// </summary>
    /// <remarks>
    /// 左集合正确地包含在右集合中
    /// </remarks>
    Subset = 16,

    /// <summary>
    /// 两个集合都不是空的，左集合是右集合的严格超集。
    /// </summary>
    /// <remarks>
    /// 右集合正确地包含在左集合中
    /// </remarks>
    Superset = 32,

    /// <summary>
    /// 两个非空集合是相等的。
    /// </summary>
    Equal = 64
}
