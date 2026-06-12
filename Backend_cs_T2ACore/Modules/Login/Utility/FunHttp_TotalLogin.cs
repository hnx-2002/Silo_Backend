using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public class FunHttp_TotalLogin
{

    /// <summary>
    /// 验证Token
    /// </summary>
    /// <param name="tp_token"></param> 
    /// <param name="totalURL"></param> 
    /// <returns></returns>
    public static string CheckToken(string tp_token, string totalURL)
    {
        string tokenStr = Config.BaseConfig.TokenString;

        var options = new RestClientOptions(Config.BaseConfig.AuthPath + "/TokenTest")
        {
            ThrowOnAnyError = false,
        };

        var client = new RestClient(options);

        var request = new RestRequest().AddHeader("Content-Type", "application/json");
        request.Method = Method.Post;
        request.AddCookie(tokenStr, tp_token, "/", Config.BaseConfig.AuthDomain);
        request.AddCookie("RemoteURL", totalURL, "/", Config.BaseConfig.AuthDomain);
        var response = client.ExecuteAsync(request).GetAwaiter().GetResult();
        //return response.Content;

        try
        {
            JObject resJo = (JObject)JsonConvert.DeserializeObject(response.Content);
            if (resJo["status"].ToString().ToUpper() == "OK")
            {
                return "OK";
            }
            else
            {
                FunConsole.ConsoleLog("TPAuth--NotOK", response.Content);
                return "Error";
            }
        }
        catch (Exception ex)
        {
            string error = ex.ToString();
            FunConsole.ConsoleLog("TPAuth--error", error, response.Content);
            return "Error";
        }

    }


    /// <summary>
    /// 登录获取Token
    /// </summary> 
    /// <returns></returns>
    public static (bool Status, string Msg, string Token) Login(LoginInfo info)
    {
        string tokenStr = Config.BaseConfig.TokenString;

        try
        {
            var options = new RestClientOptions(Config.BaseConfig.AuthPath + "/ExtCheckLogin")
            {
                ThrowOnAnyError = false,
            };

            string body = "{" +
                "\"loginType\":\"account\"," +
                "\"account\":\"" + info.Account + "\"," +
                "\"mobile\":\"\"," +
                "\"email\":\"\"," +
                "\"password\":\"" + info.Password + "\"," +
                "\"vcode\":\"\"}";

            var client = new RestClient(options);

            var request = new RestRequest().AddHeader("Content-Type", "application/json");
            request.AddBody(body);
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request).GetAwaiter().GetResult();
            var res = (JObject)JsonConvert.DeserializeObject(response.Content);

            if (res["status"].ToString() == "1")
            {
                var token = res[tokenStr].ToString();
                var userinfo = UserInfo.GetFromToken(token);

                return (true, "OK", token);
            }
            else
            {
                return (false, res["msg"].ToString(), "");
            }
        }
        catch (Exception ex)
        {
            return (false, ex.ToString(), "");
        }


    }


}