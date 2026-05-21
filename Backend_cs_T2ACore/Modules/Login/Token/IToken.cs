namespace T2ACore;

/// <summary>
/// Token相关方法
/// </summary>
public interface IToken
{
    /// <summary>
    /// 获取Token
    /// </summary> 
    /// <param name="inAccount"></param>
    /// <param name="inUser"></param>
    /// <returns></returns>
    (bool Status, string Msg, string Token) TokenGet(string inAccount, User_Class inUser);

    /// <summary>
    /// 验证Token
    /// </summary> 
    /// <param name="inToken"></param> 
    /// <returns></returns>
    public (bool Status, string Msg, User_Class User) TokenTest(string inToken);
}
