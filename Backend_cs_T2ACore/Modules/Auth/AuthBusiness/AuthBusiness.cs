using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace T2ACore;

/// <summary>
/// 鉴权
/// </summary>
public class AuthBusiness : IAuthBusiness
{
    private readonly IDBHelper dbHelper;
    private readonly string connId = "System";

    /// <summary>
    /// 构造函数
    /// </summary> 
    public AuthBusiness(IDBHelper inDI3)
    {
        dbHelper = inDI3;
    }

    /// <summary>
    /// 创建表
    /// </summary>
    /// <returns></returns>
    public bool CreateTables()
    {
        try
        {
            dbHelper.GetDB("System").CodeFirst.InitTables<Auth_role_Class>();
            dbHelper.GetDB("System").CodeFirst.InitTables<Auth_assets_Class>();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 更新用户角色，批量，带事务
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="tenant"></param>
    /// <param name="newRoles"></param>
    /// <returns></returns>
    public (bool Status, string Message) UpdateUserRoles(List<string> accounts, string tenant, List<Auth_assets_Class> newRoles)
    { 
        return dbHelper.WithTransaction(() =>
         {
             DeleteRolesForUsers(accounts, tenant);
             Add(newRoles);
         }); 
    }


    /// <summary>
    /// 根据租户，角色维度，查找所有的人
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="gType"></param>
    /// <returns></returns>
    public List<RoleList> GetAllPersons(string tenant, string gType)
    {
        var res = dbHelper.GetDB(connId)
            .Queryable<RoleList>()
            .Where(x => x.Section == "g" &&
                        x.IsPerson == "Person" &&
                        x.Tenant == tenant &&
                        x.Region == gType)
            .ToList();
        return res;

    }


    /// <summary> 
    /// 添加一批角色实体 
    /// </summary> 
    /// <param name="inSet">角色实体的集合</param> 
    /// <returns></returns> 
    public int Add(List<Auth_role_Class> inSet)
    {
        return dbHelper.AddBatch(connId, inSet);
    }

    /// <summary> 
    /// 添加一批资源实体 
    /// </summary> 
    /// <param name="inSet">资源实体集合</param> 
    /// <returns></returns> 
    public int Add(List<Auth_assets_Class> inSet)
    {
        return dbHelper.AddBatch(connId, inSet);
    }

    /// <summary>
    /// 根据role_code,role_tenant_code查询
    /// </summary>
    /// <param name="inSearch_Role_code">查询字段角色编码</param>
    /// <param name="tenant">查询字段租户</param>
    /// <returns></returns>
    public List<Auth_role_Class> SearchBy_RoleCode_Tenant(string inSearch_Role_code, string tenant)
    {
        return dbHelper.QueryListByClause<Auth_role_Class>(connId,
            (x) => x.Role_code == inSearch_Role_code &&
                   x.Role_tenant_code == tenant,
            (x) => x.Role_code, default);
    }


    /// <summary> 
    /// 修改一批实体，只改角色编码和角色名 
    /// TODO 更新缓存
    /// </summary> 
    /// <param name="inSet">实体</param> 
    /// <param name="account">实体</param> 
    /// <param name="userName">实体</param> 
    /// <returns></returns> 
    public bool UpdateSysRole(List<Auth_role_Class> inSet, string account, string userName)
    {
        var ids = inSet.Select(x => x.Id).ToList();

        var ovRoles = dbHelper.GetDB(connId)
            .Queryable<Auth_role_Class>()
            .Where(x => ids.Contains(x.Id))
            .ToList();

        var ovRoleCodes = ovRoles.Select(x => x.Role_code).ToList();

        var assets = dbHelper.GetDB(connId)
            .Queryable<Auth_assets_Class>()
            .Where(x => ovRoleCodes.Contains(x.Value1) ||
                        ovRoleCodes.Contains(x.Value2))
            .ToList();

        var changeMap = new List<(string, string)>();

        var time = FunCommon.GetStandardTimeStamp();
        foreach (var item in ovRoles)
        {
            var temp = inSet.Find(x => x.Id == item.Id);

            item.Update_account = account;
            item.Update_username = userName;
            item.Update_time = time;

            if (item.Role_code != temp.Role_code)
            {
                changeMap.Add((item.Role_code, temp.Role_code));
                item.Role_code = temp.Role_code;
            }

            if (item.Role_title != temp.Role_title)
            {
                item.Role_title = temp.Role_title;
            }
        }

        foreach (var item in assets)
        {
            var map1 = changeMap.FindAll(x => x.Item1 == item.Value1);
            if (map1.Count > 0)
            {
                item.Value1 = map1.First().Item2;
            }
            var map2 = changeMap.FindAll(x => x.Item1 == item.Value2);
            if (map2.Count > 0)
            {
                item.Value2 = map2.First().Item2;
            }
        }

        var (status, msg) = dbHelper.WithTransaction(() =>
        {
            dbHelper.GetDB(connId).Updateable(ovRoles).ExecuteCommand();
            dbHelper.GetDB(connId).Updateable(assets).ExecuteCommand();
        });

        if (!status)
        {
            FunConsole.ConsoleLog(msg);
        }

        return status;
    }

    /// <summary>
    /// 删除角色，TODO，缓存，递归删除。。。
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public int DeleteSysRole(Guid[] ids)
    {
        //var roles = dbHelper.GetDB(connId)
        //    .Queryable<Auth_role_Class>()
        //    .Where(x => ids.Contains(x.Id))
        //    .ToList();

        //var roleCodes = roles.Select(x => x.Role_code).ToList(); 

        return dbHelper.GetDB(connId)
            .Deleteable<Auth_role_Class>()
            .Where(x => ids.Contains(x.Id))
            .ExecuteCommand();
    }

    /// <summary>
    /// 删除人员的权限资源
    /// </summary>
    /// <param name="userAccounts"></param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public int DeleteRolesForUsers(List<string> userAccounts, string tenant)
    {
        return dbHelper.DeleteByClause<Auth_assets_Class>(connId,
            x => userAccounts.Contains(x.Value1) &&
                 x.Value3 == tenant &&
                 x.Value4 == "Person");
    }


    /// <summary>
    /// 删除角色资源
    /// TODO，需要考虑中间一层删除之后，后续如何对接
    /// </summary>
    /// <param name="roleRegion"></param>
    /// <param name="roleCode"></param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public int DeleteRolesForRole(string roleRegion, string roleCode, string tenant)
    {
        return dbHelper.DeleteByClause<Auth_assets_Class>(connId,
            x => x.Section == "g" &&
                 x.Type == roleRegion &&
                 x.Value1 == roleCode &&
                 x.Value3 == tenant &&
                 x.Value4 == null);
    }

    /// <summary>
    /// 为某个角色删除权限资源
    /// </summary>
    /// <param name="assetType">资源类型(p,p2,p3)</param>
    /// <param name="roleCode">角色编码(admin)</param>
    /// <param name="tenant">租户</param>
    /// <param name="pType">业务资源类型(Menu,Controller)</param>
    /// <param name="patternList">特征值，如Menu，则为MenuKey，若Controller，则为Route，Method</param>
    /// <returns></returns>
    public int DeleteAssetsForRole(string assetType, string roleCode,
        string tenant, string pType, List<List<string>> patternList = null)
    {
        var query = dbHelper.GetDB(connId).Deleteable<Auth_assets_Class>();
        Expression<Func<Auth_assets_Class, bool>> expression = (x) =>
            x.Section == "p" &&
            x.Type == assetType &&
            x.Value1 == roleCode &&
            x.Value2 == tenant &&
            x.Value3 == pType;

        if (patternList == null)
        {
            return query.Where(expression).ExecuteCommand();
        }

        foreach (List<string> patterns in patternList)
        {
            var subExpress = expression.And((x) => 1 == 1);
            for (int i = 0; i < patterns.Count; i++)
            {
                var tester = patterns[i];
                var proInfo = AuthCommon.GetAssetPatternName(i + 4);
                Expression<Func<Auth_assets_Class, bool>> patternTest = (x) =>
                    proInfo.GetValue(x).ToString() == tester;
                subExpress = subExpress.And(patternTest);
            }
            query.Where(subExpress);
        }

        return query.ExecuteCommand();
    }

    /// <summary>
    /// 根据账号获取,拍平数据
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public List<RoleList> GetRolesListForUser(string account)
    {
        var res = dbHelper.GetDB(connId).Queryable<RoleList>()
            .Where(x => x.Section == "g")
            .ToChildList(x => x.RoleCode, account, true);
        return res;
    }


    /// <summary>
    /// 根据账号获取，树形
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public List<RoleTree> GetRolesTreeForUser(string account)
    {
        var roleLists = GetRolesListForUser(account).GroupBy(x => x.Region);
        var res = new List<RoleTree>();

        foreach (var roleList in roleLists)
        {
            var tree = MakeRolesTree(roleList.ToList(), account);
            res.AddRange(tree);
        }

        return res;
    }

    /// <summary>
    /// 递归构造树
    /// </summary>
    /// <param name="list"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private List<RoleTree> MakeRolesTree(List<RoleList> list, string key)
    {
        var res = new List<RoleTree>();
        var roots = list.FindAll(x => x.RoleCode == key);
        if (roots.Count != 0)
        {
            foreach (var root in roots)
            {
                RoleTree tree = root.CastToTree();
                var maker = MakeRolesTree(list, root.UpRole);
                tree.Havings = maker;
                res.Add(tree);
            }
        }
        return res;
    }

    /// <summary>
    /// 获取某人的所有权限资源
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public List<Res_AssetGroup> GetAssetsForUser(string account)
    {
        var roleLists = GetRolesListForUser(account).GroupBy(x => x.Region);

        var res = new List<Res_AssetGroup>();

        foreach (var roleList in roleLists)
        {
            Res_AssetGroup group = new();
            group.RoleRegion = roleList.Key;
            group.Tenant = roleList.First()?.Tenant;

            var roles = roleList.Select(x => x.UpRole);

            //TODO 考虑缓存
            var finds = dbHelper.GetDB(connId)
                .Queryable<Auth_assets_Class>()
                .Where(x => x.Section == "p" &&
                            roles.Contains(x.Value1))
                .ToList();

            foreach (var asset in finds)
            {
                Res_Asset result = Res_Asset.Cast(asset, roleList.Key);
                group.Assets.Add(result);
            }

            res.Add(group);
        }

        return res;
    }

    /// <summary>
    /// 初始化测试数据
    /// </summary>
    /// <returns></returns>
    public bool InitTestData()
    {
        dbHelper.GetDB(connId).DbMaintenance.TruncateTable("auth_assets");
        dbHelper.GetDB(connId).DbMaintenance.TruncateTable("auth_role");

        var roles = new List<Auth_role_Class>();
        roles.Add(Auth_role_Class.TestDataMaker("sinoma", "中材国际", "g", "Org"));
        roles.Add(Auth_role_Class.TestDataMaker("tcdri", "天津水泥院", "g", "Org"));
        roles.Add(Auth_role_Class.TestDataMaker("digital", "数字所", "g", "Org"));
        roles.Add(Auth_role_Class.TestDataMaker("dev2", "开发二组", "g", "Org"));

        roles.Add(Auth_role_Class.TestDataMaker("admin", "管理员", "g2", "Job"));
        roles.Add(Auth_role_Class.TestDataMaker("common", "员工", "g2", "Job"));

        var groups = new List<Auth_assets_Class>();
        groups.Add(Auth_assets_Class.TestDataMaker_G("g", "dev2", "digital"));
        groups.Add(Auth_assets_Class.TestDataMaker_G("g", "digital", "tcdri"));
        groups.Add(Auth_assets_Class.TestDataMaker_G("g", "tcdri", "sinoma"));

        groups.Add(Auth_assets_Class.TestDataMaker_G("g", "1713", "dev2", true));
        groups.Add(Auth_assets_Class.TestDataMaker_G("g2", "1713", "admin", true));

        groups.Add(Auth_assets_Class.TestDataMaker_G("g", "1714", "dev2", true));
        groups.Add(Auth_assets_Class.TestDataMaker_G("g2", "1714", "common", true));

        var policies = new List<Auth_assets_Class>();
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "tcdri", "Menu", "menu1"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "dev2", "Menu", "menu1"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "dev2", "Menu", "menu2"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "dev2", "Menu", "menu3"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "dev2", "Menu", "menu4"));

        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "common", "Menu", "menu1"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "common", "Menu", "menu2"));

        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "admin", "Menu", "menu1"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "admin", "Menu", "menu2"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "admin", "Menu", "menu3"));
        policies.Add(Auth_assets_Class.TestDataMaker_P("p", "admin", "Menu", "menu4"));

        var (status, msg) = dbHelper.WithTransaction(() =>
        {
            dbHelper.GetDB(connId).Insertable(roles).ExecuteCommand();
            dbHelper.GetDB(connId).Insertable(groups).ExecuteCommand();
            dbHelper.GetDB(connId).Insertable(policies).ExecuteCommand();
        });

        if (status)
        {
            FunConsole.ConsoleLog("测试数据", msg);
        }

        return status;


    }




}
