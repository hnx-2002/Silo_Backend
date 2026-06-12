using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System; 

namespace T2ACore;

/// <summary>
/// 通过Http访问TextIn
/// </summary>
public class FunTextInHttp
{
    /// <summary>
    /// 创建任务
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <param name="fileName"></param>
    /// <param name="fileBytes"></param>
    public static Res_TextInCreate CreateTask(string baseUrl, string apiKey, string fileName, byte[] fileBytes)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }
         
        var url = baseUrl + "/api/contracts/v3/parser/external/task/create" +
            "?parse_type=document" +
            "&merge_images=0" +
            "&table_flavor=html" +
            "&remove_watermark=0";
        var options = new RestClientOptions(url)
        {
            ThrowOnAnyError = false,
        };

        var client = new RestClient(options);

        var request = new RestRequest()
            .AddHeader("x-api-key", apiKey);

        request.AlwaysMultipartFormData = true;

        request.AddParameter("parse_task_config", "{\"ui_selected_fields\":[" +
            "\"title\",\"paragraph\",\"table\",\"image\",\"stamp\",\"catalog\"," +
            "\"formula\",\"handwritten\",\"headerandfooter\"]}");

        request.AddFile("documents", fileBytes, fileName);

        request.Method = Method.Post;
        var response = client.ExecuteAsync(request).GetAwaiter().GetResult();


        try
        {
            var res = JsonConvert.DeserializeObject<Res_TextInCreate>(response.Content);
            return res;
        }
        catch (Exception ex)
        {
            FunConsole.Log("出错，原始数据为：" + response.Content + "\r\n" + ex.ToString(), ConsoleColor.Red);
            throw;
        }


    }

    /// <summary>
    /// 获取List
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    public static Res_TextInList GetList(string baseUrl, string apiKey)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }

        var url = baseUrl + "/api/contracts/v3/parser/external/task/list";
        var options = new RestClientOptions(url)
        {
            ThrowOnAnyError = false,
        };
        var client = new RestClient(options);

        var request = new RestRequest()
            .AddHeader("x-api-key", apiKey);
        request.Method = Method.Get;
        var response = client.ExecuteAsync(request).GetAwaiter().GetResult();

        try
        {
            var res = JsonConvert.DeserializeObject<Res_TextInList>(response.Content);
            return res;
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// 下载Markdown
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <param name="taskFileId"></param>
    /// <returns></returns>
    public static string GetResultMarkdown(string baseUrl, string apiKey, string taskFileId)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }

        var ja = new JArray();
        ja.Add(taskFileId);
        var jo = new JObject();
        jo["task_ids"] = ja;

        var url = baseUrl + "/api/contracts/v3/parser/external/md_file/export";
        var options = new RestClientOptions(url)
        {
            ThrowOnAnyError = false,
        };
        var client = new RestClient(options);

        var request = new RestRequest()
            .AddHeader("x-api-key", apiKey)
            .AddJsonBody(jo.ToString());

        request.Method = Method.Post;
        var response = client.ExecuteAsync(request).GetAwaiter().GetResult();

        return response.Content; 
    }

    /// <summary>
    /// 下载Excel
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <param name="taskFileId"></param>
    /// <returns></returns>
    public static byte[] GetResultExcel(string baseUrl, string apiKey, string taskFileId)
    {
        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
        }

        var ja = new JArray();
        ja.Add(taskFileId);
        var jo = new JObject();
        jo["task_ids"] = ja;

        var url = baseUrl + "/api/contracts/v3/parser/external/excel_file/export";
        var options = new RestClientOptions(url)
        {
            ThrowOnAnyError = false,
        };
        var client = new RestClient(options);

        var request = new RestRequest()
            .AddHeader("x-api-key", apiKey)
            .AddJsonBody(jo.ToString());

        request.Method = Method.Post;
        var response = client.ExecuteAsync(request).GetAwaiter().GetResult();

        return response.RawBytes;
    }
}
