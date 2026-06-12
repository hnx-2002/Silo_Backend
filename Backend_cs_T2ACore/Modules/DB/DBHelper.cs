using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TPGeometryPro;

namespace T2ACore;

/// <summary>
/// SqlSugar帮助类
/// </summary>
public class DBHelper : IDBHelper
{
    private readonly SqlSugarScope db;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DBHelper()
    {
        var connList = new List<ConnectionConfig>();

        foreach (var dbconf in Config.DBConfig.DBConns)
        {
            var conn = new ConnectionConfig();
            conn.ConfigId = dbconf.Tenant;
            conn.DbType = GetDBType(dbconf.DatabaseType);
            conn.ConnectionString = dbconf.LinkString;
            conn.IsAutoCloseConnection = true;

            connList.Add(conn);

            if (conn.DbType == DbType.DuckDB)
            {
                //注册DLL防止找不到DLL
                InstanceFactory.CustomAssemblies = [
                    typeof(SqlSugar.DuckDB.DuckDBProvider).Assembly
                ];
            } 
        }

        db = new SqlSugarScope(connList, dbscope =>
        {
            MakeDBConfig(dbscope, "Default");
            MakeDBConfig(dbscope, "System");
        });

        //if (Config.DBConfig.OutputToConsole)
        //{
        //    db = new SqlSugarScope(connList, dbscope =>
        //    {
        //        MakeDBConfig(dbscope, "Default");
        //        MakeDBConfig(dbscope, "System");
        //    });
        //}
        //else
        //{
        //    db = new SqlSugarScope(connList);
        //}
    }

    private static void MakeDBConfig(SqlSugarClient dbscope, string ConnId)
    {
        var exe = dbscope.GetConnection(ConnId);
        exe.Aop.OnLogExecuting = (sql, p) =>
        {
            if (Config.DBConfig.OutputToConsole)
            {
                Console.WriteLine("-----------------------SQL执行中(" + ConnId + ")start------------------------------------");
                foreach (var param in p)
                {
                    Console.WriteLine($"SET {param.ParameterName} = '{param.Value}';");
                }
                Console.WriteLine(sql);
                Console.WriteLine("-----------------------SQL执行中(" + ConnId + ")end--------------------------------------");
            }
        };
        exe.Aop.OnLogExecuted = (sql, p) =>
        {
            if (Config.DBConfig.OutputToConsole)
            {
                Console.WriteLine("-----------------------SQL执行后(" + ConnId + ")start------------------------------------");
                //Console.WriteLine(UtilMethods.GetNativeSql(sql, p));
                Console.WriteLine(UtilMethods.GetSqlString(dbscope.CurrentConnectionConfig.DbType, sql, p));
                Console.WriteLine("执行时间:" + exe.Ado.SqlExecutionTime.ToString());
                Console.WriteLine("-----------------------SQL执行后(" + ConnId + ")end--------------------------------------");
            }
        };
        exe.Aop.OnError = (ex) =>
        {
            if (Config.DBConfig.OutputToConsole)
            {
                Console.WriteLine("-----------------------SQL执行错误(" + ConnId + ")start--------------------------------------");
                Console.WriteLine($"Error {ex.Message}");
                Console.WriteLine("-----------------------SQL执行错误(" + ConnId + ")end----------------------------------------");
            }
        };
        exe.Aop.OnDiffLogEvent = (diff) =>
        {
            if (Config.DBConfig.LogDiff)
            {
                // 记录差异数据
                var diffData = new List<dynamic>();
                if (diff.BeforeData is null)
                {
                    //说明是新增插入
                    for (int i = 0; i < diff.AfterData.Count; i++)
                    {
                        var diffColumns = new List<dynamic>();
                        var afterColumns = diff.AfterData[i].Columns;
                        for (int j = 0; j < afterColumns.Count; j++)
                        {
                            diffColumns.Add(new
                            {
                                afterColumns[j].IsPrimaryKey,
                                afterColumns[j].ColumnName,
                                afterColumns[j].ColumnDescription,
                                BeforeValue = "",
                                AfterValue = afterColumns[j].Value,
                            });
                        }
                        diffData.Add(new
                        {
                            diff.AfterData[i].TableName,
                            diff.AfterData[i].TableDescription,
                            Columns = diffColumns
                        });
                    }
                }
                else if (diff.AfterData is null)
                {
                    //说明是删除
                    for (int i = 0; i < diff.BeforeData.Count; i++)
                    {
                        var diffColumns = new List<dynamic>();
                        var beforeColumns = diff.BeforeData[i].Columns;
                        for (int j = 0; j < beforeColumns.Count; j++)
                        {
                            diffColumns.Add(new
                            {
                                beforeColumns[j].IsPrimaryKey,
                                beforeColumns[j].ColumnName,
                                beforeColumns[j].ColumnDescription,
                                BeforeValue = beforeColumns[j].Value,
                                AfterValue = "",
                            });
                        }
                        diffData.Add(new
                        {
                            diff.BeforeData[i].TableName,
                            diff.BeforeData[i].TableDescription,
                            Columns = diffColumns
                        });
                    }
                }
                else if (diff.BeforeData is not null && diff.AfterData is not null)
                {
                    //说明是更新
                    for (int i = 0; i < diff.AfterData.Count; i++)
                    {
                        var diffColumns = new List<dynamic>();
                        var afterColumns = diff.AfterData[i].Columns;
                        var beforeColumns = diff.BeforeData[i].Columns;
                        for (int j = 0; j < afterColumns.Count; j++)
                        {
                            if (afterColumns[j].Value.Equals(beforeColumns[j].Value)) continue;
                            diffColumns.Add(new
                            {
                                afterColumns[j].IsPrimaryKey,
                                afterColumns[j].ColumnName,
                                afterColumns[j].ColumnDescription,
                                BeforeValue = beforeColumns[j].Value,
                                AfterValue = afterColumns[j].Value,
                            });
                        }
                        diffData.Add(new
                        {
                            diff.AfterData[i].TableName,
                            diff.AfterData[i].TableDescription,
                            Columns = diffColumns
                        });
                    }
                }

                var dbDiffLog = new DBDiffLog
                {
                    // 差异数据（字段描述、列名、值、表名、表描述）
                    DiffData = JsonConvert.SerializeObject(diffData),
                    // 传进来的对象
                    BusinessData = JsonConvert.SerializeObject(diff.BusinessData),
                    // 操作类型枚举（insert、update、delete）
                    DiffType = diff.DiffType.ToString(),
                    //执行的Sql
                    Sql = UtilMethods.GetSqlString(exe.CurrentConnectionConfig.DbType, diff.Sql, diff.Parameters),
                    Parameters = JsonConvert.SerializeObject(diff.Parameters),
                    Elapsed = diff.Time == null ? 0 : (long)diff.Time.Value.TotalMilliseconds,
                    //SystemCode = GlobalConfig.BaseConfig?.SystemCode ?? PlatformConst.Namespace,
                    //TenantId = db.CurrentConnectionConfig.ConfigId?.ToString() ?? string.Empty,
                };

                StringBuilder stringBuilderDifflog = new();
                stringBuilderDifflog.Append($"*****{DateTime.Now:yyyy-MM-dd}差异日志开始*****{Environment.NewLine}");
                stringBuilderDifflog.Append($"{JsonConvert.SerializeObject(dbDiffLog)}{Environment.NewLine}");
                stringBuilderDifflog.Append($"*****差异日志结束*****{Environment.NewLine}");
                Logger.CreateLogger("SqlSugar.Difflog").LogDebug("{stringBuilderDifflog}", stringBuilderDifflog);
            }
             
        };
        //exe.Aop.OnExecutingChangeSql = (sql, p) =>
        //{
        //    Console.WriteLine("-----------------------OnExecutingChangeSql--------------------------------------");
        //    Console.WriteLine(sql);
        //    return default;
        //}; 
    }

    private static SqlSugar.DbType GetDBType(string dbType)
    {
        SqlSugar.DbType dbTypeEnum = default;
        switch (dbType)
        {
            case "MYSQL":
                dbTypeEnum = SqlSugar.DbType.MySql;
                break;
            case "MSSQL":
                dbTypeEnum = SqlSugar.DbType.SqlServer;
                break;
            case "SQLITE":
                dbTypeEnum = SqlSugar.DbType.Sqlite;
                break;
            case "PGSQL":
                dbTypeEnum = SqlSugar.DbType.PostgreSQL;
                break;
            case "DUCKDB":
                dbTypeEnum = SqlSugar.DbType.DuckDB;
                break;
            default:
                break;
        }
        return dbTypeEnum;
    }

    private ISugarQueryable<T> GetConn<T>(string connId, string tableName) where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Queryable<T>();
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res;
    }

    #region 获取db

    /// <summary>
    /// 获取DB
    /// </summary>
    /// <returns></returns>
    public SqlSugarScopeProvider GetDB(string connId)
    {
        return db.GetConnectionScope(connId);
    }


    #endregion 获取db

    #region 单表查询

    /// <summary>
    /// 执行sql语句并返回List"T"
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public List<T> QuerySql<T>(string connId, string sql,
        List<SugarParameter> parameters) where T : class, new()
    {
        return db.GetConnectionScope(connId).Ado.SqlQuery<T>(sql, parameters);
    }

    /// <summary>
    /// 根据主键查询单条数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="pkValue">主键值</param>
    /// <param name="tableName">指定表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>泛型实体</returns>
    public T QueryOne<T, TKey>(string connId, TKey pkValue,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock).InSingle(pkValue)
            : res.InSingle(pkValue);
    }

    /// <summary>
    /// 根据条件查询单条数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public T QueryOneByClause<T>(string connId, Expression<Func<T, bool>> predicate,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock).First(predicate)
            : res.First(predicate);
    }

    /// <summary>
    /// 根据条件查询单条数据,使用排序表达式
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="orderByPredicate">排序字段</param>
    /// <param name="orderByType">排序顺序</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public T QueryOneByClause<T>(string connId,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.OrderBy(orderByPredicate, orderByType)
                 .With(SqlWith.NoLock)
                 .First(predicate)
            : res.OrderBy(orderByPredicate, orderByType)
                 .First(predicate);
    }

    /// <summary>
    /// 根据主键集合查询数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），
    /// 如果是联合主键，请使用Where条件</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>数据实体列表</returns> 
    public List<T> QueryByIds<T, TKey>(string connId, TKey[] lstIds,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.In(lstIds)
                 .With(SqlWith.NoLock)
                 .ToList()
            : res.In(lstIds)
                 .ToList();
    }

    /// <summary>
    /// 根据主键集合查询数据
    /// </summary>
    /// <param name="connId">租户Code</param> 
    /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），
    /// 如果是联合主键，请使用Where条件</param>
    /// <param name="tableName">表名</param>      
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>数据实体列表</returns> 
    public List<T> QueryByIds<T, TKey>(string connId, List<TKey> lstIds,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.In(lstIds)
                 .With(SqlWith.NoLock)
                 .ToList()
            : res.In(lstIds)
                 .ToList();
    }

    /// <summary>
    /// 查询全部实体
    /// </summary> 
    /// <param name="connId">租户Code</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>泛型实体集合</returns>
    public List<T> QueryList<T>(string connId,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock)
                 .ToList()
            : res.ToList();
    }

    /// <summary>
    /// 根据条件查询数据，条件、排序均为表达式
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="orderByPredicate">排序字段</param>
    /// <param name="orderByType">排序顺序</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>泛型实体集合</returns>
    public List<T> QueryListByClause<T>(string connId,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.WhereIF(predicate != null, predicate)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .With(SqlWith.NoLock)
                 .ToList()
            : res.WhereIF(predicate != null, predicate)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .ToList();
    }

    /// <summary>
    /// 根据条件查询一定数量数据,排序使用表达式
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="take">获取前xx条</param>
    /// <param name="predicate">条件表达式树</param> 
    /// <param name="orderByPredicate">排序字段</param>
    /// <param name="orderByType">排序顺序</param> 
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public List<T> QuerySomeListByClause<T>(string connId,
        int take, Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
       string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.WhereIF(predicate != null, predicate)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .Take(take)
                 .With(SqlWith.NoLock)
                 .ToList()
            : res.WhereIF(predicate != null, predicate)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .Take(take)
                 .ToList();
    }



    /// <summary>
    /// 判断满足条件的数据是否存在
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public bool Exists<T>(string connId,
        Expression<Func<T, bool>> predicate,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock)
                 .Where(predicate).Any()
            : res.Where(predicate).Any();
    }

    /// <summary>
    /// 根据条件获取数据总数
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public int QueryCount<T>(string connId,
        Expression<Func<T, bool>> predicate,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock)
                 .Count(predicate)
            : res.Count(predicate);
    }

    /// <summary>
    /// 获取某字段去重
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="column">选取的字段</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public List<TResult> QueryDistinct<T, TResult>(string connId,
        Expression<Func<T, TResult>> column,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock)
                 .Distinct()
                 .Select(column)
                 .ToList()
            : res.Distinct()
                 .Select(column)
                 .ToList();
    }

    /// <summary>
    /// 获取某字段去重
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="column">选取的字段</param>
    /// <param name="predict">条件</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public List<TResult> QueryDistinctByClause<T, TResult>(string connId,
        Expression<Func<T, TResult>> column, Expression<Func<T, bool>> predict,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.With(SqlWith.NoLock)
                 .WhereIF(predict != null, predict)
                 .Distinct()
                 .Select(column)
                .ToList()
            : res.WhereIF(predict != null, predict)
                 .Distinct()
                 .Select(column)
                 .ToList();
    }



    /// <summary>
    /// 获取数据某个字段的合计
    /// TResult: int,double,decimal,long,float
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="field">字段</param>
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public TResult QuerySum<T, TResult>(string connId,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> field,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return blUseNoLock
            ? res.Where(predicate)
                 .With(SqlWith.NoLock)
                 .Sum(field)
            : res.Where(predicate)
                 .Sum(field);
    }

    /// <summary>
    /// 获取该节点下所有子节点
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="expression">条件表达式</param>
    /// <param name="relationId">相关联的Id, 可以是parentId，treeId</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public List<T> QueryChildrenList<T, TKey>(string connId,
        Expression<Func<T, object>> expression, TKey relationId,
        string tableName = "") where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return res.ToChildList(expression, relationId);
    }



    /// <summary>
    /// 查询树形结构，返回List"T",返回所有数据
    /// 适用Children为非继承类的情况，Children字段加Ignore
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="childListExpression">子集合字段</param>
    /// <param name="parentIdExpression">父级Id字段</param>
    /// <param name="rootId">根节点值</param> 
    /// <param name="predicate">条件</param> 
    /// <param name="orderByPredicate">排序</param> 
    /// <param name="orderByType">排序规则</param> 
    /// <param name="tableName">表名</param> 
    /// <returns></returns>
    public List<T> QueryTreeList<T>(string connId,
        Expression<Func<T, IEnumerable<object>>> childListExpression,
        Expression<Func<T, object>> parentIdExpression, object rootId,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderByPredicate,
        OrderByType orderByType = OrderByType.Asc,
        string tableName = "") where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        return res.WhereIF(predicate != null, predicate)
                  .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                  .ToTree(childListExpression, parentIdExpression, rootId);
    }


    /// <summary>
    /// 查询树形结构，返回List"T",返回所有数据
    /// 适用Children为继承类的情况
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="childListExpression">子集合字段</param>
    /// <param name="parentIdExpression">父级Id字段</param>
    /// <param name="rootId">根节点值</param> 
    /// <param name="predicate">条件</param> 
    /// <param name="orderByPredicate">排序</param> 
    /// <param name="orderByType">排序规则</param> 
    /// <param name="tableName">表名</param> 
    /// <returns></returns>
    public List<TResult> QueryTreeList<T, TResult>(string connId,
        Expression<Func<TResult, IEnumerable<object>>> childListExpression,
        Expression<Func<TResult, object>> parentIdExpression, object rootId,
        Expression<Func<TResult, bool>> predicate,
        Expression<Func<TResult, object>> orderByPredicate,
        OrderByType orderByType = OrderByType.Asc,
        string tableName = "") where TResult : class, new()
    {
        ISugarQueryable<TResult> res = GetConn<TResult>(connId, tableName);
        return res.WhereIF(predicate != null, predicate)
                  .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                  .ToTree(childListExpression, parentIdExpression, rootId);
    }

    /// <summary>
    /// 单表分页查询 
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则，asc desc</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页数量</param> 
    /// <param name="tableName">表名</param>
    /// <param name="blUseNoLock"></param>
    /// <returns></returns>
    public (int Count, List<T> ResData) QueryPageList<T>(string connId,
        Expression<Func<T, bool>> whereLambda,
        Expression<Func<T, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        int pageIndex = 1, int pageSize = 20,
        string tableName = "", bool blUseNoLock = false) where T : class, new()
    {
        ISugarQueryable<T> res = GetConn<T>(connId, tableName);
        var totalCount = 0;
        var page = blUseNoLock
            ? res.WhereIF(whereLambda != null, whereLambda)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .With(SqlWith.NoLock)
                 .ToPageList(pageIndex, pageSize, ref totalCount)
            : res.WhereIF(whereLambda != null, whereLambda)
                 .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .ToPageList(pageIndex, pageSize, ref totalCount);

        return (totalCount, page);
    }


    #endregion 单表查询

    #region 多表查询

    /// <summary>
    /// 两表查询, 返回单个实体TResult
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) => 
    /// new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) =>
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则 asc desc</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public TResult QueryMuchFirst<T1, T2, TResult>(string connId,
        Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda,
        Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        bool blUseNoLock = false) where T1 : class, new()
    {
        return blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .First()
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .First();
    }

    /// <summary>
    /// 三表查询, 返回单个实体TResult
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="T3">实体3</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) => 
    /// new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) =>
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则 asc desc</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public TResult QueryMuchFirst<T1, T2, T3, TResult>(string connId,
        Expression<Func<T1, T2, T3, object[]>> joinExpression,
        Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda,
        Expression<Func<T1, T2, T3, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        bool blUseNoLock = false) where T1 : class, new()
    {
        return blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .First()
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .First();
    }


    /// <summary>
    /// 两表查询, 返回多个实体TResult
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] 
    /// {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) => 
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则 asc desc</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public List<TResult> QueryMuch<T1, T2, TResult>(string connId,
        Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda,
        Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        bool blUseNoLock = false) where T1 : class, new()
    {
        return blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .ToList()
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .ToList();
    }


    /// <summary>
    /// 三表查询, 返回多个实体TResult
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="T3">实体3</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) => 
    /// new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) => 
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则 asc desc</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public List<TResult> QueryMuch<T1, T2, T3, TResult>(string connId,
        Expression<Func<T1, T2, T3, object[]>> joinExpression,
        Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda,
        Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        bool blUseNoLock = false) where T1 : class, new()
    {
        return blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .ToList()
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .ToList();
    }
    #endregion 多表查询

    #region 多表分页查询

    /// <summary>
    /// 查询-2表查询 带分页，带条件，带排序条件
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) =>
    /// new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) =>
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询条件表达式 (w1, w2) =>w1.UserNo == "")</param> 
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则，asc desc</param>
    /// <param name="pageIndex">当前页面索引</param>
    /// <param name="pageSize">分布大小</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public (int Count, List<TResult> ResData) QueryMuchPageList<T1, T2, TResult>(string connId,
        Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda,
        Expression<Func<T1, T2, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        int pageIndex = 1, int pageSize = 20,
        bool blUseNoLock = false) where T1 : class, new()
    {
        var totalCount = 0;
        var res = blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .ToPageList(pageIndex, pageSize, ref totalCount)
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .ToPageList(pageIndex, pageSize, ref totalCount);
        return (totalCount, res);

    }

    /// <summary>
    /// 查询-3表查询 带分页，带条件，带排序条件
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体2</typeparam>
    /// <typeparam name="T3">实体3</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="connId">租户Code</param>
    /// <param name="joinExpression">关联表达式 (join1,join2) => 
    /// new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
    /// <param name="selectExpression">返回表达式 (s1, s2) =>
    /// new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
    /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
    /// <param name="orderByPredicate">排序表达式</param>
    /// <param name="orderByType">排序规则，asc desc</param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns>值</returns>
    public (int Count, List<TResult> ResData) QueryMuch3Page<T1, T2, T3, TResult>(string connId,
        Expression<Func<T1, T2, T3, object[]>> joinExpression,
        Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda,
        Expression<Func<T1, T2, T3, object>> orderByPredicate, OrderByType orderByType = OrderByType.Asc,
        int pageIndex = 1, int pageSize = 20,
        bool blUseNoLock = false) where T1 : class, new()
    {
        var totalCount = 0;
        var page = blUseNoLock
            ? db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .With(SqlWith.NoLock)
                .ToPageList(pageIndex, pageSize, ref totalCount)
            : db.GetConnectionScope(connId)
                .Queryable(joinExpression)
                .WhereIF(whereLambda != null, whereLambda)
                .OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .Select(selectExpression)
                .ToPageList(pageIndex, pageSize, ref totalCount);

        return (totalCount, page);
    }

    #endregion 多表分页查询

    #region 新增

    /// <summary>
    /// 写入单个实体数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">实体数据</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool AddOne<T>(string connId, T entity,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Insertable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand() > 0;
    }

    /// <summary>
    /// 写入单个实体数据，指定列的数据，其他留空
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">实体数据</param>
    /// <param name="insertColumns">插入的列</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool AddOne<T>(string connId, T entity, Expression<Func<T, object>> insertColumns,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Insertable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }

        if (insertColumns == null)
        {
            return res.ExecuteCommand() > 0;
        }
        return res.InsertColumns(insertColumns)
            .ExecuteCommand() > 0;
    }

    /// <summary>
    /// 批量写入实体数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entities">实体类</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int AddBatch<T>(string connId, List<T> entities,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Insertable(entities);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand();
    }

    /// <summary>
    /// 批量写入实体数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entities">实体类</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int AddBatch<T>(string connId, T[] entities,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Insertable(entities);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand();
    }

    #endregion 新增

    #region 修改

    /// <summary>
    /// 更新实体数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">实体</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool UpdateOne<T>(string connId, T entity,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommandHasChange();
    }

    /// <summary>
    /// 根据条件更新
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity"></param>
    /// <param name="where"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool UpdateOne<T>(string connId, T entity, Expression<Func<T, bool>> where,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.Where(where)
                  .ExecuteCommandHasChange();
    }

    /// <summary>
    /// 根据条件更新单条数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">实体</param>  
    /// <param name="where">条件表达式</param>
    /// <param name="usingColumns">使用的列集合</param>
    /// <param name="ignoreColumns">忽略的列集合</param> 
    /// <param name="tableName">表名</param> 
    /// <returns></returns>
    public bool UpdateOne<T>(string connId, T entity,
        Expression<Func<T, bool>> where,
        Expression<Func<T, object>> usingColumns = null,
        Expression<Func<T, object>> ignoreColumns = null,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        if (ignoreColumns != null)
        {
            res = res.IgnoreColumns(ignoreColumns);
        }
        if (usingColumns != null)
        {
            res = res.UpdateColumns(usingColumns);
        }
        return res.Where(where)
                  .ExecuteCommandHasChange();

    }

    /// <summary>
    /// 批量更新实体数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entities"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int UpdateBatch<T>(string connId, List<T> entities,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable(entities);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand();
    }

    /// <summary>
    /// 批量根据条件更新
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entities"></param>
    /// <param name="where"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool UpdateBatch<T>(string connId, List<T> entities, Expression<Func<T, bool>> where,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable(entities);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }

        return res.Where(where)
                  .ExecuteCommandHasChange();
    }


    /// <summary>
    /// 更新某个字段,或某几个字段
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="columns">lamdba表达式,如it => new Student()
    /// { Name = "a", CreateTime = DateTime.Now }</param>
    /// <param name="where">lamdba判断</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int UpdateSome<T>(string connId,
        Expression<Func<T, T>> columns,
        Expression<Func<T, bool>> where,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable<T>();
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.SetColumns(columns)
                  .Where(where)
                  .ExecuteCommand();
    }


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
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Updateable<T>(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.IgnoreColumns(columns)
                  .Where(where)
                  .ExecuteCommand();
    }



    #endregion 修改

    #region 删除

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entity">实体类</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool DeleteOne<T>(string connId, T entity,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable(entity);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommandHasChange();
    }

    /// <summary>
    /// 删除批量数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="entities">实体类集合</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int DeleteBatch<T>(string connId, List<T> entities,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable(entities);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand();
    }

    /// <summary>
    /// 删除指定Id的数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="id"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public bool DeleteOneById<T, TKey>(string connId, TKey id,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable<T>(id);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommandHasChange();
    }

    /// <summary>
    /// 删除指定Id集合的数据(批量删除)
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="ids"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int DeleteBatchByIds<T, TKey>(string connId, TKey[] ids,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable<T>();
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.In(ids).ExecuteCommand();
    }

    /// <summary>
    /// 删除指定ID集合的数据(批量删除)
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="ids"></param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int DeleteBatchByIds<T, TKey>(string connId, List<TKey> ids,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable<T>();
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.In(ids).ExecuteCommand();
    }

    /// <summary>
    /// 根据条件删除数据
    /// </summary>
    /// <param name="connId">租户Code</param>
    /// <param name="where">过滤条件</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public int DeleteByClause<T>(string connId, Expression<Func<T, bool>> where,
        string tableName = "") where T : class, new()
    {
        var res = db.GetConnectionScope(connId).Deleteable(where);
        if (!string.IsNullOrEmpty(tableName))
        {
            res = res.AS(tableName);
        }
        return res.ExecuteCommand();
    }

    #endregion 删除

    #region 事务

    /// <summary>
    /// 执行事务
    /// </summary> 
    /// <param name="action">执行的内容</param>
    /// <param name="rollback">回滚的操作</param>
    /// <returns></returns>
    public (bool Status, string Error) WithTransaction(Action action, Action rollback = default)
    {
        try
        {
            db.BeginTran();
            action();
            db.CommitTran();
            return (true, null);
        }
        catch (Exception ex)
        {
            string error = ex.ToString();
            db.RollbackTran();
            if (rollback != default)
            {
                rollback();
            }
            return (false, error);
        }
    }

    #endregion
}

