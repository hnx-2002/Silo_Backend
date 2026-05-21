using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public interface IDBHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int AddBatch<T>(string connId, List<T> entities, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int AddBatch<T>(string connId, T[] entities, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool AddOne<T>(string connId, T entity, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="insertColumns"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool AddOne<T>(string connId, T entity, Expression<Func<T, object>> insertColumns, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int DeleteBatch<T>(string connId, List<T> entities, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="ids"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int DeleteBatchByIds<T, TKey>(string connId, List<TKey> ids, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="ids"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int DeleteBatchByIds<T, TKey>(string connId, TKey[] ids, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="where"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>

    int DeleteByClause<T>(string connId, Expression<Func<T, bool>> where, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool DeleteOne<T>(string connId, T entity, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="id"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool DeleteOneById<T, TKey>(string connId, TKey id, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    bool Exists<T>(string connId, Expression<Func<T, bool>> predicate, string tableName = "", bool blUseNoLock = false) where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connId"></param>
    /// <returns></returns>
    SqlSugarScopeProvider GetDB(string connId);
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="lstIds"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<T> QueryByIds<T, TKey>(string connId, List<TKey> lstIds, string tableName = "", bool blUseNoLock = false) where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="lstIds"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<T> QueryByIds<T, TKey>(string connId, TKey[] lstIds, string tableName = "", bool blUseNoLock = false) where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="expression"></param>
    /// <param name="relationId"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    List<T> QueryChildrenList<T, TKey>(string connId, Expression<Func<T, object>> expression, TKey relationId, string tableName = "") where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    int QueryCount<T>(string connId, Expression<Func<T, bool>> predicate, string tableName = "", bool blUseNoLock = false) where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="column"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<TResult> QueryDistinct<T, TResult>(string connId, Expression<Func<T, TResult>> column, string tableName = "", bool blUseNoLock = false) where T : class, new();
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="column"></param>
    /// <param name="predict"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<TResult> QueryDistinctByClause<T, TResult>(string connId, Expression<Func<T, TResult>> column, Expression<Func<T, bool>> predict, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<T> QueryList<T>(string connId, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<T> QueryListByClause<T>(string connId, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<TResult> QueryMuch<T1, T2, T3, TResult>(string connId, Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, Expression<Func<T1, T2, T3, bool>> whereLambda, Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<TResult> QueryMuch<T1, T2, TResult>(string connId, Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda, Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    (int Count, List<TResult> ResData) QueryMuch3Page<T1, T2, T3, TResult>(string connId, Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, Expression<Func<T1, T2, T3, bool>> whereLambda, Expression<Func<T1, T2, T3, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    TResult QueryMuchFirst<T1, T2, T3, TResult>(string connId, Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, Expression<Func<T1, T2, T3, bool>> whereLambda, Expression<Func<T1, T2, T3, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    TResult QueryMuchFirst<T1, T2, TResult>(string connId, Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda, Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    (int Count, List<TResult> ResData) QueryMuchPageList<T1, T2, TResult>(string connId, Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda, Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false) where T1 : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="connId"></param>
    /// <param name="pkValue"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    T QueryOne<T, TKey>(string connId, TKey pkValue, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    T QueryOneByClause<T>(string connId, Expression<Func<T, bool>> predicate, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    T QueryOneByClause<T>(string connId, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="whereLambda"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    (int Count, List<T> ResData) QueryPageList<T>(string connId, Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="take"></param>
    /// <param name="predicate"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    List<T> QuerySomeListByClause<T>(string connId, int take, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    List<T> QuerySql<T>(string connId, string sql, List<SugarParameter> parameters) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="predicate"></param>
    /// <param name="field"></param>
    /// <param name="tableName"></param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    TResult QuerySum<T, TResult>(string connId, Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> field, string tableName = "", bool blUseNoLock = false) where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="connId"></param>
    /// <param name="childListExpression"></param>
    /// <param name="parentIdExpression"></param>
    /// <param name="rootId"></param>
    /// <param name="predicate"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    List<TResult> QueryTreeList<T, TResult>(string connId, Expression<Func<TResult, IEnumerable<object>>> childListExpression, Expression<Func<TResult, object>> parentIdExpression, object rootId, Expression<Func<TResult, bool>> predicate, Expression<Func<TResult, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, string tableName = "") where TResult : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="childListExpression"></param>
    /// <param name="parentIdExpression"></param>
    /// <param name="rootId"></param>
    /// <param name="predicate"></param>
    /// <param name="orderByPredicate"></param>
    /// <param name="orderByType"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    List<T> QueryTreeList<T>(string connId, Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootId, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int UpdateBatch<T>(string connId, List<T> entities, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entities"></param>
    /// <param name="where"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool UpdateBatch<T>(string connId, List<T> entities, Expression<Func<T, bool>> where, string tableName = "") where T : class, new();

    /// <summary>
    /// 更新,忽略某个字段,或某几个字段
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">要更新的实体</param>
    /// <param name="columns">lamdba表达式,如x => new { x.Name, x.CreateTime }</param>
    /// <param name="where">lamdba判断</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int UpdateIgnore<T>(string connId,
        T entity,
        Expression<Func<T, object>> columns,
        Expression<Func<T, bool>> where,
        string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool UpdateOne<T>(string connId, T entity, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="where"></param>
    /// <param name="usingColumns"></param>
    /// <param name="ignoreColumns"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool UpdateOne<T>(string connId, T entity, Expression<Func<T, bool>> where, Expression<Func<T, object>> usingColumns = null, Expression<Func<T, object>> ignoreColumns = null, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="entity"></param>
    /// <param name="where"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    bool UpdateOne<T>(string connId, T entity, Expression<Func<T, bool>> where, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connId"></param>
    /// <param name="columns"></param>
    /// <param name="where"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    int UpdateSome<T>(string connId, Expression<Func<T, T>> columns, Expression<Func<T, bool>> where, string tableName = "") where T : class, new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="rollback"></param>
    /// <returns></returns>
    (bool Status, string Error) WithTransaction(Action action, Action rollback = null);
}