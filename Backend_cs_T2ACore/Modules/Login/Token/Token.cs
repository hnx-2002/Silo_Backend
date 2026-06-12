using Duende.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace T2ACore;

/// <summary>
/// Token相关方法
/// </summary>
public class Token : IToken
{
    private readonly ICache redis;

    /// <summary>
    /// 构造函数
    /// </summary>
    public Token(ICache inDI4)
    {
        redis = inDI4;
    }


    /// <summary>
    /// 获取Token
    /// </summary> 
    /// <param name="inAccount">账号</param>
    /// <param name="inUser">用户</param>
    /// <returns>返回Token的Id</returns>
    public (bool Status, string Msg, string Token) TokenGet(string inAccount, User_Class inUser)
    {
        //res.Mobile = jo["phone_number"] == null ? "" : jo["phone_number"].ToString();
        //res.Email = jo["email"] == null ? "" : jo["email"].ToString();
        //res.EWeChatId = jo["EWeChatId"] == null ? "" : jo["EWeChatId"].ToString();
        //res.Tenant = jo["Tenant"] == null ? "" : jo["Tenant"].ToString();
        //res.Gender = jo["gender"] == null ? "" : jo["gender"].ToString();

        try
        {
            var claims = new Dictionary<string, object>();
            claims.Add("Tenant", inUser.Tenant);

            var tokenHandler = new JwtSecurityTokenHandler();
            string keyString = Config.LoginConfig.SecretKey; //适用范围+私钥
            var key = Encoding.UTF8.GetBytes(keyString);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddHours(Config.LoginConfig.TokenExpires);   //过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim(JwtClaimTypes.Audience, Config.LoginConfig.Audience),
                new Claim(JwtClaimTypes.Issuer, Config.LoginConfig.Issuer),
                new Claim(JwtClaimTypes.ClientId, inAccount),
                new Claim(JwtClaimTypes.Name, inUser.UserName),
                new Claim(JwtClaimTypes.Id, inUser.Id.ToString().ToLower()),
            }),
                Claims = claims, //企业微信Id
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);

            return (true, "OK", tokenString);
        }
        catch (Exception ex)
        {
            return (false, ex.ToString(), null);
        }


    }

    /// <summary>
    /// 验证Token
    /// </summary> 
    /// <param name="inToken">Token</param> 
    /// <returns></returns>
    public (bool Status, string Msg, User_Class User) TokenTest(string inToken)
    {
        //tempUser 未校验token真伪
        var tempUser1 = UserInfo.GetFromToken(inToken);

        if (tempUser1 == null)
        {
            return (false, "Bad Token", null);
        }

        var tempUser = tempUser1.GetUserFromToken();

        if (tempUser == null)
        {
            return (false, "BadTokenToUser", null);
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string keyString = Config.LoginConfig.SecretKey; //适用范围+私钥
            var key = Encoding.UTF8.GetBytes(keyString);
            var tokenValidationPara = new TokenValidationParameters
            {
                //这里为验证规则
                //颁发给谁
                ValidAudience = Config.LoginConfig.Audience,
                //Token颁发机构
                ValidIssuer = Config.LoginConfig.Issuer,

                //这里的key要进行加密
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = new TimeSpan(30000),
                RequireExpirationTime = true,
                ValidateLifetime = true
            };

            //验证token
            tokenHandler.ValidateToken(inToken, tokenValidationPara,
                                       out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            string name = jwtToken.Claims.First(x => x.Type == "name").Value;        //得到用户名

            var account = jwtToken.Claims.First(x => x.Type == "client_id").Value;        //得到account

            //// var userObj = redis.Get("TPAuth_" + account);

            ////var user = redis.GetFromCache<User_Class>(userObj[0].ToString());     
            //var user = redis.GetFromCache<User_Class>("TPAuth_" + account);

            //if (user == null )
            //{
            //    return (false, "no auth stored", user);
            //}

            return (true, "success", tempUser);
        }
        catch (Exception ex)
        {
            string error = ex.ToString();
            // 验证失败的话给出error
            if (ex.Message.StartsWith("IDX10223")) //IDX10223是token超时的编号
            {
                return (false, "The token is expired.", tempUser);
            }
            else
            {
                Logger.Debug("鉴权出错\r\n" + DateTime.Now.ToString() + "\r\n" + error);
                return (false, "Unauthorized", tempUser);
            }
        }
    }
}
