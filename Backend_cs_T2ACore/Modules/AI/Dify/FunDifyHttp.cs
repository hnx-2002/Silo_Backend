using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System; 
using System.IO; 
using System.Threading.Tasks; 

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public class FunDifyHttp
{ 
    /// <summary>
    /// 生成问题答案,异步方法
    /// </summary> 
    /// <param name="baseUrl"></param> 
    /// <param name="appKey"></param> 
    /// <param name="taskId1"></param> 
    /// <param name="question"></param> 
    /// <param name="account"></param> 
    /// <param name="chatHub"></param> 
    /// <param name="inputs"></param> 
    /// <returns></returns>
    public static async Task<(bool, string, string, string)> Generate(
        string baseUrl, string appKey, string taskId1, string question, string account,
        IHubContext<ChatResHub> chatHub, JObject inputs = null)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }

        try
        {
            if (inputs == null)
            {
                inputs = new JObject();
            }

            var ask = Req_DifyAsk.Cast(inputs, question, account);

            var askObj = JsonConvert.SerializeObject(ask);

            var url = baseUrl + "/v1/chat-messages";

            var options = new RestClientOptions(url)
            {
                AutomaticDecompression = System.Net.DecompressionMethods.None,
                ThrowOnAnyError = false,
            };

            var client = new RestClient(options);

            var request = new RestRequest()
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", "Bearer " + appKey)
                .AddJsonBody(askObj);
            request.Method = Method.Post;
            request.RequestFormat = DataFormat.Json;

            return await FunDifyCommon.ExecuteStreamResponse(
                client, request, true, true, chatHub, taskId1, false, null);

        }
        catch (Exception ex)
        {
            FunConsole.Debug(ex.ToString());
            return (false, ex.ToString(), null, null);
        }
    }

    /// <summary>
    /// 上传图片
    /// </summary>  
    /// <param name="baseUrl"></param>   
    /// <param name="appKey"></param>  
    /// <param name="imageMs"></param>  
    /// <param name="fileName"></param>   
    /// <param name="account"></param>    
    /// <returns></returns>
    public static (bool, string, string) UploadFile(
        string baseUrl, string appKey, MemoryStream imageMs, string fileName, string account)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }

        try
        {
            var ext = Path.GetExtension(fileName).Substring(1);

            var ask = Req_DifyAsk.UploadCast(account);
            var configPara = Parameter.CreateParameter("data",
                JsonConvert.SerializeObject(ask), ParameterType.GetOrPost);

            //FunConsole.ConsoleLog($"{askObj}", ConsoleColor.Yellow);
            var url = baseUrl + "/v1/files/upload";

            var options = new RestClientOptions(url)
            {
                AutomaticDecompression = System.Net.DecompressionMethods.None,
                ThrowOnAnyError = false,
            };

            var client = new RestClient(options);

            var request = new RestRequest()
                .AddHeader("Authorization", "Bearer " + appKey);

            request.AlwaysMultipartFormData = true;
            request.Method = Method.Post;
            request.AddFile("file", imageMs.ToArray(), fileName, "image/" + ext);//TODO contentType先不传试试
            request.AddParameter(configPara);

            var response = client.ExecuteAsync(request).GetAwaiter().GetResult();

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //FunConsole.Log(response.Content, ConsoleColor.Yellow);
                var joRes = JsonConvert.DeserializeObject<JObject>(response.Content);

                return (true, "OK", joRes["id"].ToString());
            }
            else
            {
                return (false, response.ResponseStatus.ToString(), null);
            }

        }
        catch (Exception ex)
        {
            FunConsole.Debug(ex.ToString());
            return (false, ex.ToString(), null);
        }
    }

}
