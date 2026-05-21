using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public static class UserListExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="users"></param>
    public static List<User_Class> KillPassword(this List<User_Class> users)
    {
        foreach (var user in users)
        {
            user.Password = "******";
        }
        return users;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    public static User_Class KillPassword(this User_Class user)
    {
        user.Password = "******";
        return user;
    }
}
