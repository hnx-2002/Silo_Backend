using System;
using System.Linq;
using System.Linq.Expressions;

namespace T2ACore;

/// <summary>
///
/// </summary>
public static class PredicateBuilder
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, object>> ToObject<T>()
    {
        return f => true;
    }

    /// <summary>
    /// 两个表的Expression表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, T2, bool>> True<T, T2>()
    {
        //如果联合查询，报表别名错误，那一定是你的联合查询的表别名 没有按照这个查询条件==>child, parent
        //2022112 cyl 修改联合查询 别名child, parent==>o,p
        return (o, p) => true;
    }

    /// <summary>
    /// 三个表的Expression表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, T2, T3, bool>> True<T, T2, T3>()
    {
        //如果联合查询，报表别名错误，那一定是你的联合查询的表别名 没有按照这个查询条件==>o, p, q
        return (o, p, q) => true;
    }

    /// <summary>
    /// 四个表的Expression表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, T2, T3, T4, bool>> True<T, T2, T3, T4>()
    {
        //如果联合查询，报表别名错误，那一定是你的联合查询的表别名 没有按照这个查询条件==>o, p, q,r
        return (o, p, q, r) => true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> True<T>()
    {
        return f => true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> False<T>()
    {
        return f => false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
        Func<Expression, Expression, Expression> merge)
    {
        // build parameter map (from parameters of second to parameters of first)
        var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        // replace parameters in the second lambda expression with parameters from the first
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        // apply composition of lambda expression bodies to parameters from the first expression
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.AndAlso);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.OrElse);
    }

    /// <summary>
    /// 两个expression拼接
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T1, T2, bool>> And<T1, T2>(this Expression<Func<T1, T2, bool>> first, Expression<Func<T1, T2, bool>> second)
    {
        return first.Compose(second, Expression.And);
    }

    /// <summary>
    /// 两个expression拼接
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T1, T2, T3, bool>> And<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> first, Expression<Func<T1, T2, T3, bool>> second)
    {
        return first.Compose(second, Expression.And);
    }

    /// <summary>
    /// 两个expression拼接
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T1, T2, T3, T4, bool>> And<T1, T2, T3, T4>(this Expression<Func<T1, T2, T3, T4, bool>> first, Expression<Func<T1, T2, T3, T4, bool>> second)
    {
        return first.Compose(second, Expression.And);
    }
}