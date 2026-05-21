using SqlSugar;
using System;
using System.Collections.Generic; 

namespace T2ACore;

/// <summary>
/// YG Added 20230713
/// @YG 添加一些描述或使用方法，这里
/// </summary>
public class SqlSugarCustomFunc
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<SqlFuncExternal> GetCustomSqlSugarFunc()
    {
        var expMethods = new List<SqlSugar.SqlFuncExternal>();

        // min 开窗方法
        expMethods.Add(new SqlFuncExternal()
        {
            UniqueMethodName = "WindowMin",
            MethodValue = (expInfo, dbType, expContext) =>
            {
                if (dbType == SqlSugar.DbType.MySql)
                {
                    if (expInfo.Args.Count == 2)
                    {
                        return string.Format("MIN({0}) OVER (PARTITION BY {1})",
                            expInfo.Args[0].MemberName, expInfo.Args[1].MemberName);
                    }
                    else if (expInfo.Args.Count == 3)
                    {
                        return string.Format("MIN({0}) OVER (PARTITION BY {1} ORDER BY {2})",
                            expInfo.Args[0].MemberName, expInfo.Args[1].MemberName, expInfo.Args[2].MemberName);
                    }
                    else
                    {
                        throw new NotImplementedException("其余传参未实现");
                    }
                }
                else
                {
                    throw new NotImplementedException("其余数据库未实现");
                }
            }
        });

        // ANY_VALUE
        expMethods.Add(new SqlFuncExternal()
        {
            UniqueMethodName = "AnyValue",
            MethodValue = (expInfo, dbType, expContext) =>
            {
                if (dbType == SqlSugar.DbType.MySql)
                {
                    return string.Format("ANY_VALUE({0})", expInfo.Args[0].MemberName);
                }
                else
                {
                    throw new NotImplementedException("其余数据库未实现");
                }
            }
        });

        // UNIX_TIMESTAMP
        expMethods.Add(new SqlFuncExternal()
        {
            UniqueMethodName = "ToUnixTimestamp",
            MethodValue = (expInfo, dbType, expContext) =>
            {
                if (dbType == SqlSugar.DbType.MySql)
                {
                    return string.Format("UNIX_TIMESTAMP({0})", expInfo.Args[0].MemberName);
                }
                else
                {
                    throw new NotImplementedException("其余数据库未实现");
                }
            }
        });

        return expMethods;
    }

    /// <summary>
    /// 作为开窗函数使用的 min 方法
    /// MIN(cacColumn) OVER (PARTITION BY partitionColumn)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="M"></typeparam>
    /// <param name="calcColumn"></param>
    /// <param name="partitionColumn"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static T WindowMin<T, M>(T calcColumn, M partitionColumn = default(M))
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// MySQL 中的 ANY_VALUE 函数
    /// </summary>
    /// <typeparam name="VALUE"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static VALUE AnyValue<VALUE>(VALUE value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 将数值转为 Unix 时间戳
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int ToUnixTimestamp(DateTime val)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 将数值转为 Unix 时间戳
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int ToUnixTimestamp(DateTime? val)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 将数值转为 Unix 时间戳
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int ToUnixTimestamp(DateTimeOffset val)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 将数值转为 Unix 时间戳
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int ToUnixTimestamp(DateTimeOffset? val)
    {
        throw new NotImplementedException();
    }
}
