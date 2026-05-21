using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using RestSharp;

namespace T2ACore;

/// <summary>
/// 检索服务
/// </summary>
public static class FunRagFlowRetrieval
{
    /// <summary>
    /// 从数据集检索分块
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="request">检索请求</param>
    /// <returns>检索结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<(List<Res_ChunkInfo> Chunks, List<Res_DocumentAggregate> Documents, int Total)> Retrieve(
        string baseUrl, string apiKey, Req_Retrieval request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/retrieval";
        var restRequest = new RestRequest(url, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        //restRequest.AddJsonBody(request);
        restRequest.AddParameter("application/json",
            request, ParameterType.RequestBody);

        FunConsole.Log(JsonConvert.SerializeObject(request), ConsoleColor.Magenta);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        var data = RagFlowResponse.HandleResponse<Res_RetrievalData>(response, "检索失败");
        return (data.Chunks, data.DocAggs, data.Total);
    }

    /// <summary>
    /// 简单检索（仅通过问题）
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param> 
    /// <param name="datasetIds">要搜索的数据集 ID 列表</param>
    /// <param name="question">用户查询或查询关键词</param>
    /// <param name="pageSize">返回的最大分块数量，默认 30</param>
    /// <returns>检索结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<List<Res_ChunkInfo>> RetrieveSimple(
        string baseUrl, string apiKey, List<string> datasetIds, string question, int pageSize = 30)
    {
        var request = new Req_Retrieval
        {
            Question = question,
            Dataset_ids = datasetIds,
            Page_size = pageSize,
            Top_k = 1024
        };

        var (chunks, _, _) = await Retrieve(baseUrl, apiKey, request);
        return chunks;
    }

    /// <summary>
    /// 简单检索（仅通过问题）
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="question">用户查询或查询关键词</param>
    /// <param name="datasetName">数据集名称</param>
    /// <param name="rerankId">重排模型</param>
    /// <param name="pageSize">返回的最大分块数量，默认 30</param>
    /// <returns>检索结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<List<Res_ChunkInfo>> RetrieveSimple(
        string baseUrl, string apiKey, string datasetName,
        string question, string rerankId = "Qwen3-Reranker-8B", int pageSize = 30)
    {
        var datasets = await FunRagFlowDataset.GetDatasets(baseUrl, apiKey, name: datasetName);
        if (datasets == null || datasets.Count < 1)
        {
            FunConsole.Log("未找到" + datasetName + "的数据集", ConsoleColor.Red);
            return null;
        }
        var datasetIds = datasets.Select(x => x.Id).ToList();

        var request = new Req_Retrieval
        {
            Question = question,
            Dataset_ids = datasetIds,
            Page_size = pageSize,
            Rerank_id = rerankId,
            Top_k = 1024
        };

        var (chunks, _, _) = await Retrieve(baseUrl, apiKey, request);
        return chunks;
    }

    /// <summary>
    /// 带高亮的检索
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetIds">要搜索的数据集 ID 列表</param>
    /// <param name="question">用户查询或查询关键词</param> 
    /// <param name="highlight">是否高亮匹配词</param>
    /// <param name="pageSize">返回的最大分块数量，默认 30</param>
    /// <returns>检索结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<List<Res_ChunkInfo>> RetrieveWithHighlight(
        string baseUrl, string apiKey, List<string> datasetIds, string question,
        bool highlight = true, int pageSize = 30)
    {
        var request = new Req_Retrieval
        {
            Question = question,
            Dataset_ids = datasetIds,
            Highlight = highlight,
            Page_size = pageSize,
            Top_k = 1024
        };

        var (chunks, _, _) = await Retrieve(baseUrl, apiKey, request);
        return chunks;
    }
}
