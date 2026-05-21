using System.Collections.Generic;
using System.Linq.Expressions;

namespace T2ACore;

/// <summary>
///
/// </summary>
public class ParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> map;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="map"></param>
    public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
        this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="map"></param>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static Expression ReplaceParameters(
        Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
    {
        return new ParameterRebinder(map).Visit(exp);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    protected override Expression VisitParameter(ParameterExpression p)
    {
        ParameterExpression replacement;
        if (map.TryGetValue(p, out replacement)) p = replacement;
        return base.VisitParameter(p);
    }
}