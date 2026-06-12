using SqlSugar;
using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public partial class User_Business : IUser_Business
{

    ///////////////////////////////////////////////////////////
    //  以下为生成代码
    ///////////////////////////////////////////////////////////

    #region 查询
    /// <summary>
    /// 获取所有实体
    /// </summary>
    /// <returns></returns>
    public List<User_Class> GetAll()
    {
        var cacheData = CacheFunc.GetFromCache<List<User_Class>>(connId + CacheKey);
        if (cacheData == null || cacheData.Count < 1)
        {
            var newData = dbHelper.QueryList<User_Class>(connId);
            CacheFunc.Insert(connId + CacheKey, newData);
            return newData.KillPassword();
        }
        else
        {
            return cacheData.KillPassword();
        }
    }



    /// <summary>
    /// 获取全部数据数量
    /// </summary>
    /// <returns></returns>
    public int GetAllCount()
    {
        return dbHelper.QueryCount<User_Class>(connId, (x) => true);
    }

    /// <summary>
    /// 直接从数据库获取分页全部数据
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public (int TotalCount, List<User_Class> Datas) GetAllPaged(int pageIndex, int pageSize)
    {
        var (count, users) = dbHelper.QueryPageList<User_Class>(connId,
            (x) => true, (x) => new
            {
                x.Id
            }, default, pageIndex, pageSize);

        return (count, users.KillPassword());

    }
    /// <summary>
    /// 根据主键得到一批实体
    /// </summary>
    /// <param name="inSet"></param>
    /// <returns></returns>
    public List<User_Class> GetByIds(int[] inSet)
    {
        return dbHelper.QueryByIds<User_Class, int>(connId, inSet).KillPassword();
    }

    /// <summary>
    /// 根据主键从全部实体中获取单个实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public User_Class GetOneFromAll(int id)
    {
        return GetAll().Find(p => p.Id == id).KillPassword();
    }

    /// <summary>
    /// 根据主键从全部实体中获取单个实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public User_Class GetOne(int id)
    {
        return dbHelper.QueryOne<User_Class, int>(connId, id).KillPassword();
    }

    /// <summary>
    /// 根据UserId查询
    /// </summary>
    /// <param name="inSearch_UserId">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_UserId(Guid inSearch_UserId)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => x.UserId == inSearch_UserId,
            (x) => x.UserId, default).KillPassword();
    }

    /// <summary>
    /// 根据UserId集合查询
    /// </summary>
    /// <param name="inSearch_UserIds">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_UserIds(List<Guid> inSearch_UserIds)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => inSearch_UserIds.Contains(x.UserId),
            (x) => x.UserId, default).KillPassword();
    }

    /// <summary>
    /// 根据Tenant查询
    /// </summary>
    /// <param name="inSearch_Tenant">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Tenant(string inSearch_Tenant)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => x.Tenant == inSearch_Tenant,
            (x) => x.Tenant, default).KillPassword();
    }

    /// <summary>
    /// 根据Tenant集合查询
    /// </summary>
    /// <param name="inSearch_Tenants">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Tenants(List<string> inSearch_Tenants)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => inSearch_Tenants.Contains(x.Tenant),
            (x) => x.Tenant, default).KillPassword();
    }

    /// <summary>
    /// 根据Account查询
    /// </summary>
    /// <param name="inSearch_Account">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Account(string inSearch_Account)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => x.Account == inSearch_Account,
            (x) => x.Account, default).KillPassword();
    }

    /// <summary>
    /// 根据Account集合查询
    /// </summary>
    /// <param name="inSearch_Accounts">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Accounts(List<string> inSearch_Accounts)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => inSearch_Accounts.Contains(x.Account),
            (x) => x.Account, default).KillPassword();
    }

    /// <summary>
    /// 根据Create_account查询
    /// </summary>
    /// <param name="inSearch_Create_account">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Create_account(string inSearch_Create_account)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => x.Create_account == inSearch_Create_account,
            (x) => x.Create_account, default).KillPassword();
    }

    /// <summary>
    /// 根据Create_account集合查询
    /// </summary>
    /// <param name="inSearch_Create_accounts">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Create_accounts(List<string> inSearch_Create_accounts)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => inSearch_Create_accounts.Contains(x.Create_account),
            (x) => x.Create_account, default).KillPassword();
    }

    /// <summary>
    /// 根据Update_account查询
    /// </summary>
    /// <param name="inSearch_Update_account">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Update_account(string inSearch_Update_account)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => x.Update_account == inSearch_Update_account,
            (x) => x.Update_account, default).KillPassword();
    }

    /// <summary>
    /// 根据Update_account集合查询
    /// </summary>
    /// <param name="inSearch_Update_accounts">查询字段</param>
    /// <returns></returns>
    public List<User_Class> SearchBy_Update_accounts(List<string> inSearch_Update_accounts)
    {
        return dbHelper.QueryListByClause<User_Class>(connId,
            (x) => inSearch_Update_accounts.Contains(x.Update_account),
            (x) => x.Update_account, default).KillPassword();
    }

    /// <summary>
    /// 高级检索
    /// </summary>
    /// <param name="inSearchModel">查询类</param>
    /// <returns></returns>
    public List<User_Class> MultiSearch(User_Search inSearchModel)
    {
        var exp = Expressionable.Create<User_Class>();
        exp.AndIF((inSearchModel.UserId != Guid.Empty), x => x.UserId.Equals(inSearchModel.UserId));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Tenant)), x => x.Tenant.Contains(inSearchModel.Tenant));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Account)), x => x.Account.Contains(inSearchModel.Account));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Create_account)), x => x.Create_account.Contains(inSearchModel.Create_account));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Update_account)), x => x.Update_account.Contains(inSearchModel.Update_account));
        return dbHelper.QueryListByClause(connId, exp.ToExpression(), x => x.Id).KillPassword();
    }
    /// <summary>
    /// 高级模糊检索
    /// </summary>
    /// <param name="inSearchModel">查询类</param>
    /// <returns></returns>
    public List<User_Class> BlurSearch(User_Search inSearchModel)
    {
        var exp = Expressionable.Create<User_Class>();
        exp.OrIF((inSearchModel.UserId != Guid.Empty), x => x.UserId.Equals(inSearchModel.UserId));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Tenant)), x => x.Tenant.Contains(inSearchModel.Tenant));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Account)), x => x.Account.Contains(inSearchModel.Account));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Create_account)), x => x.Create_account.Contains(inSearchModel.Create_account));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Update_account)), x => x.Update_account.Contains(inSearchModel.Update_account));
        return dbHelper.QueryListByClause(connId, exp.ToExpression(), x => x.Id).KillPassword();
    }
    /// <summary>
    /// 高级分页检索
    /// </summary>
    /// <param name="inSearchModel">查询类</param>
    /// <returns></returns>
    public (int, List<User_Class>) MultiPagedSearch(Sys_user_paged_Search inSearchModel)
    {
        var exp = Expressionable.Create<User_Class>();
        exp.AndIF((inSearchModel.UserId != Guid.Empty), x => x.UserId.Equals(inSearchModel.UserId));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Tenant)), x => x.Tenant.Contains(inSearchModel.Tenant));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Account)), x => x.Account.Contains(inSearchModel.Account));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Create_account)), x => x.Create_account.Contains(inSearchModel.Create_account));
        exp.AndIF((!string.IsNullOrWhiteSpace(inSearchModel.Update_account)), x => x.Update_account.Contains(inSearchModel.Update_account));
        var (count, datas) = dbHelper.QueryPageList(
            connId, exp.ToExpression(),
            x => x.Id, OrderByType.Desc,
            inSearchModel.Page, inSearchModel.PageSize);

        return (count, datas.KillPassword());
    }
    /// <summary>
    /// 高级模糊分页检索
    /// </summary>
    /// <param name="inSearchModel">查询类</param>
    /// <returns></returns>
    public (int, List<User_Class>) BlurPagedSearch(Sys_user_paged_Search inSearchModel)
    {
        var exp = Expressionable.Create<User_Class>();
        exp.OrIF((inSearchModel.UserId != Guid.Empty), x => x.UserId.Equals(inSearchModel.UserId));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Tenant)), x => x.Tenant.Contains(inSearchModel.Tenant));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Account)), x => x.Account.Contains(inSearchModel.Account));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Create_account)), x => x.Create_account.Contains(inSearchModel.Create_account));
        exp.OrIF((!string.IsNullOrWhiteSpace(inSearchModel.Update_account)), x => x.Update_account.Contains(inSearchModel.Update_account));
        var (count, datas) = dbHelper.QueryPageList(
            connId, exp.ToExpression(),
            x => x.Id, OrderByType.Desc,
            inSearchModel.Page, inSearchModel.PageSize);

        return (count, datas.KillPassword());
    }
    #endregion 查询

    #region 添加
    /// <summary>
    /// 添加一个实体
    /// </summary>
    /// <param name="inEntity"></param>
    /// <returns></returns>
    public bool Add(User_Class inEntity)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.AddOne(connId, inEntity);
        });
    }

    /// <summary>
    /// 添加一批实体
    /// </summary>
    /// <param name="inSet"></param>
    /// <returns></returns>
    public int Add(List<User_Class> inSet)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.AddBatch(connId, inSet);
        });
    }
    #endregion 添加

    #region 修改
    /// <summary>
    /// 修改一个实体
    /// </summary>
    /// <param name="inEntity">实体</param>
    /// <returns></returns>
    public bool Update(User_Class inEntity)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.UpdateOne(connId, inEntity);
        });
    }

    /// <summary>
    /// 修改一批实体
    /// </summary>
    /// <param name="inSet">实体</param>
    /// <returns></returns>
    public int Update(List<User_Class> inSet)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.UpdateBatch(connId, inSet);
        });
    }
    #endregion 修改

    #region 删除
    /// <summary>
    /// 删除一个实体
    /// </summary>
    /// <param name="inEntity">实体</param>
    /// <returns></returns>
    public bool Delete(User_Class inEntity)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.DeleteOne(connId, inEntity);
        });
    }

    /// <summary>
    /// 删除一批实体
    /// </summary>
    /// <param name="inSet">实体集合</param>
    /// <returns></returns>
    public int Delete(List<User_Class> inSet)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.DeleteBatch(connId, inSet);
        });
    }

    /// <summary>
    /// 根据主键删除一个实体
    /// </summary>
    /// <param name="inId">主键</param>
    /// <returns></returns>
    public bool DeleteById(int inId)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.DeleteOneById<User_Class, int>(connId, inId);
        });
    }

    /// <summary>
    /// 根据主键删除一批实体
    /// </summary>
    /// <param name="inArr">主键集合</param>
    /// <returns></returns>
    public int DeleteByIds(int[] inArr)
    {
        return CacheFunc.WithCacheCleared(connId + CacheKey, () =>
        {
            return dbHelper.DeleteBatchByIds<User_Class, int>(connId, inArr);
        });
    }

    #endregion 删除
}
