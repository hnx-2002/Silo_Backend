using System;
using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public interface IAuthBusiness
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inSet"></param>
    /// <returns></returns>
    int Add(List<Auth_assets_Class> inSet);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inSet"></param>
    /// <returns></returns>
    int Add(List<Auth_role_Class> inSet);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool CreateTables();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assetType"></param>
    /// <param name="roleCode"></param>
    /// <param name="tenant"></param>
    /// <param name="pType"></param>
    /// <param name="patternList"></param>
    /// <returns></returns>
    int DeleteAssetsForRole(string assetType, string roleCode, string tenant, string pType, List<List<string>> patternList = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleRegion"></param>
    /// <param name="roleCode"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    int DeleteRolesForRole(string roleRegion, string roleCode, string tenant);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userAccounts"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    int DeleteRolesForUsers(List<string> userAccounts, string tenant);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    int DeleteSysRole(Guid[] ids);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="gType"></param>
    /// <returns></returns>
    List<RoleList> GetAllPersons(string tenant, string gType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    List<Res_AssetGroup> GetAssetsForUser(string account);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    List<RoleList> GetRolesListForUser(string account);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    List<RoleTree> GetRolesTreeForUser(string account);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool InitTestData();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inSearch_Role_code"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    List<Auth_role_Class> SearchBy_RoleCode_Tenant(string inSearch_Role_code, string tenant);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inSet"></param>
    /// <param name="account"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    bool UpdateSysRole(List<Auth_role_Class> inSet, string account, string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="tenant"></param>
    /// <param name="newRoles"></param>
    /// <returns></returns>
    (bool Status, string Message) UpdateUserRoles(List<string> accounts, string tenant, List<Auth_assets_Class> newRoles);
}