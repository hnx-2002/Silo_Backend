using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 分块管理服务
/// </summary>
public static class FunRagFlowChunk
{
    /// <summary>
    /// 添加分块到文档
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="request">添加分块请求</param>
    /// <returns>创建的块信息</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<Res_ChunkInfo> AddChunk(string baseUrl,
        string apiKey, string datasetId, string documentId, Req_AddChunk request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}/chunks";

        var restRequest = new RestRequest(url, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        //restRequest.AddJsonBody(request);
        restRequest.AddParameter("application/json",
            request, ParameterType.RequestBody);

        FunConsole.Log(JsonConvert.SerializeObject(request), ConsoleColor.Magenta);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        var data = RagFlowResponse.HandleResponse<Res_ChunkData>(response, "添加分块失败");
        return data.Chunk;
    }

    /// <summary>
    /// 获取分块列表
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="page">页码，默认 1</param>
    /// <param name="pageSize">每页数量，默认 1024</param>
    /// <param name="keywords">关键词过滤</param>
    /// <param name="chunkId">分块 ID 过滤</param>
    /// <returns>分块列表和总数</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<(List<Res_ChunkInfo> Chunks, Res_DocumentInfo Document, int Total)> GetChunks(
        string baseUrl, string apiKey, string datasetId, string documentId,
        int page = 1, int pageSize = 1024, string keywords = null, string chunkId = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}/chunks";
        var restRequest = new RestRequest(url, Method.Get);
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddParameter("page", page);
        restRequest.AddParameter("page_size", pageSize);

        if (!string.IsNullOrEmpty(keywords))
            restRequest.AddParameter("keywords", keywords);

        if (!string.IsNullOrEmpty(chunkId))
            restRequest.AddParameter("id", chunkId);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        var data = RagFlowResponse.HandleResponse<Res_ChunkListData>(response, "获取分块列表失败");
        return (data.Chunks, data.Doc, data.Total);
    }

    /// <summary>
    /// 根据 ID 获取分块
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="chunkId">分块 ID</param>
    /// <returns>分块信息</returns>
    /// <exception cref="RagFlowException">API 调用失败或分块不存在时抛出</exception>
    public static async Task<Res_ChunkInfo> GetChunkById(string baseUrl,
        string apiKey, string datasetId, string documentId, string chunkId)
    {
        var (chunks, _, _) = await GetChunks(baseUrl,
            apiKey, datasetId, documentId, chunkId: chunkId);

        if (chunks.Count == 0)
        {
            throw new RagFlowException($"分块 {chunkId} 不存在");
        }

        return chunks[0];
    }

    /// <summary>
    /// 删除分块
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="chunkIds">要删除的分块 ID 列表，null 表示删除所有</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> DeleteChunks(string baseUrl,
        string apiKey, string datasetId, string documentId, List<string> chunkIds = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}/chunks";
        var restRequest = new RestRequest(url, Method.Delete);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(new { chunk_ids = chunkIds ?? new List<string>() });

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "删除分块失败");
    }

    /// <summary>
    /// 更新分块
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="chunkId">分块 ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> UpdateChunk(string baseUrl, string apiKey,
        string datasetId, string documentId, string chunkId, Req_UpdateChunk request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}/chunks/{chunkId}";
        var restRequest = new RestRequest(url, Method.Put);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(request);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "更新分块失败");
    }
}
