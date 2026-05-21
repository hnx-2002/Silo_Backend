using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace T2ACore;

/// <summary>
/// 数据集管理服务
/// </summary>
public static class FunRagFlowDataset
{

    /// <summary>
    /// 创建数据集
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="request">创建数据集请求</param>
    /// <returns>创建的数据集信息</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<Res_DatasetInfo> CreateDataset(
        string baseUrl, string apiKey, Req_CreateDataset request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets";
        var restRequest = new RestRequest(url, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(request);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse<Res_DatasetInfo>(response, "创建数据集失败");
    }

    /// <summary>
    /// 删除数据集
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="ids">要删除的数据集 ID 列表，空列表表示删除所有</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> DeleteDatasets(
        string baseUrl, string apiKey, List<string> ids = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets";
        var restRequest = new RestRequest(url, Method.Delete);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(new { ids = ids ?? new List<string>() });

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "删除数据集失败");
    }

    /// <summary>
    /// 更新数据集配置
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> UpdateDataset(string baseUrl,
        string apiKey, string datasetId, Req_UpdateDataset request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}";
        var restRequest = new RestRequest(url, Method.Put);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(request);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "更新数据集失败");
    }

    /// <summary>
    /// 获取数据集列表
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="page">页码，默认 1</param>
    /// <param name="pageSize">每页数量，默认 30</param>
    /// <param name="orderBy">排序字段（create_time 或 update_time）</param>
    /// <param name="desc">是否降序，默认 true</param>
    /// <param name="name">数据集名称过滤</param>
    /// <param name="id">数据集 ID 过滤</param>
    /// <returns>数据集列表</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<List<Res_DatasetInfo>> GetDatasets(
        string baseUrl, string apiKey, int page = 1, int pageSize = 30,
        string orderBy = null, bool desc = true, string name = null, string id = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets";
        var restRequest = new RestRequest(url, Method.Get);
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddParameter("page", page);
        restRequest.AddParameter("page_size", pageSize);

        if (!string.IsNullOrEmpty(orderBy))
            restRequest.AddParameter("orderby", orderBy);

        restRequest.AddParameter("desc", desc);

        if (!string.IsNullOrEmpty(name))
            restRequest.AddParameter("name", name);

        if (!string.IsNullOrEmpty(id))
            restRequest.AddParameter("id", id);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        var data = RagFlowResponse.HandleResponse<List<Res_DatasetInfo>>(response, "获取数据集列表失败");
        return data ?? new List<Res_DatasetInfo>();
    }

    /// <summary>
    /// 根据 ID 获取数据集
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <returns>数据集信息</returns>
    /// <exception cref="RagFlowException">API 调用失败或数据集不存在时抛出</exception>
    public static async Task<Res_DatasetInfo> GetDatasetById(
        string baseUrl, string apiKey, string datasetId)
    {
        var datasets = await GetDatasets(baseUrl, apiKey, id: datasetId);

        if (datasets.Count == 0)
        {
            return null;
            throw new RagFlowException($"数据集 {datasetId} 不存在");
        }

        return datasets[0];
    }

    /// <summary>
    /// 查找或创建数据集
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <param name="datasetName"></param>
    /// <param name="newDatasetDescription"></param>
    /// <returns></returns>
    public static async Task<Res_DatasetInfo> GetOrCreateDataset(string baseUrl,
        string apiKey, string datasetName, string newDatasetDescription = "")
    {
        List<Res_DatasetInfo> datasets = await GetDatasets(baseUrl, apiKey);

        var dataset = datasets.Find(x => x.Name == datasetName);
        if (dataset == null)
        {
            var reqCreateDataset = Req_CreateDataset
                .New(datasetName, newDatasetDescription);
            dataset = await CreateDataset(baseUrl, apiKey, reqCreateDataset);
        }

        return dataset;

    }
}
