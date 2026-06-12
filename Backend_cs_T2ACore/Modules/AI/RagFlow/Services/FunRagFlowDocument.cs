using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;

namespace T2ACore;

/// <summary>
/// 文件管理服务
/// </summary>
public static class FunRagFlowDocument
{
    /// <summary>
    /// 上传文档到数据集
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="files">要上传的文件的文件名和流</param>
    /// <returns>上传的文档信息列表</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<List<Res_DocumentInfo>> UploadDocuments(
        string baseUrl, string apiKey, string datasetId, List<(string, MemoryStream)> files)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents";
        var restRequest = new RestRequest(url, Method.Post);
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");

        foreach (var (fileName, fileStream) in files)
        {
            restRequest.AddFile("file", fileStream.ToArray(), fileName);
        }

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse<List<Res_DocumentInfo>>(response, "上传文档失败");
    }

    /// <summary>
    /// 上传单个文档到数据集
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="fileName">文件名</param>
    /// <param name="fileStream">文件流</param>
    /// <returns>上传的文档信息</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<Res_DocumentInfo> UploadDocument(
        string baseUrl, string apiKey, string datasetId, string fileName, MemoryStream fileStream)
    {
        var documents = await UploadDocuments(baseUrl, apiKey,
            datasetId, new List<(string, MemoryStream)> { (fileName, fileStream) });
        return documents.FirstOrDefault() ?? throw new RagFlowException("上传文档失败");
    }

    /// <summary>
    /// 更新文档配置
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> UpdateDocument(string baseUrl,
        string apiKey, string datasetId, string documentId, Req_UpdateDocument request)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}";
        var restRequest = new RestRequest(url, Method.Put);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(request);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "更新文档失败");
    }

    /// <summary>
    /// 下载文档为字节数组
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <returns>文档字节数组</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<byte[]> DownloadDocumentAsBytes(
        string baseUrl, string apiKey, string datasetId, string documentId)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents/{documentId}";
        var restRequest = new RestRequest(url, Method.Get);
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new RagFlowException(response.Content ?? "下载文档失败");
        }

        return response.RawBytes ?? Array.Empty<byte>();
    }

    /// <summary>
    /// 获取文档列表
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="page">页码，默认 1</param>
    /// <param name="pageSize">每页数量，默认 30</param>
    /// <param name="orderBy">排序字段（create_time 或 update_time）</param>
    /// <param name="desc">是否降序，默认 true</param>
    /// <param name="keywords">关键词过滤</param>
    /// <param name="documentId">文档 ID 过滤</param>
    /// <param name="documentName">文档名称过滤</param>
    /// <returns>文档列表</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<(List<Res_DocumentInfo> Documents, int Total)> GetDocuments(
        string baseUrl, string apiKey, string datasetId,
        int page = 1, int pageSize = 30, string orderBy = null, bool desc = true,
        string keywords = null, string documentId = null, string documentName = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents";
        var restRequest = new RestRequest(url, Method.Get);
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddParameter("page", page);
        restRequest.AddParameter("page_size", pageSize);

        if (!string.IsNullOrEmpty(orderBy))
            restRequest.AddParameter("orderby", orderBy);

        restRequest.AddParameter("desc", desc);

        if (!string.IsNullOrEmpty(keywords))
            restRequest.AddParameter("keywords", keywords);

        if (!string.IsNullOrEmpty(documentId))
            restRequest.AddParameter("id", documentId);

        if (!string.IsNullOrEmpty(documentName))
            restRequest.AddParameter("name", documentName);

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        var data = RagFlowResponse.HandleResponse<Res_DocumentListData>(response, "获取文档列表失败");
        return (data.Docs, data.Total);
    }

    /// <summary>
    /// 根据 ID 获取文档
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentId">文档 ID</param>
    /// <returns>文档信息</returns>
    /// <exception cref="RagFlowException">API 调用失败或文档不存在时抛出</exception>
    public static async Task<Res_DocumentInfo> GetDocumentById(
        string baseUrl, string apiKey, string datasetId, string documentId)
    {
        var (documents, _) = await GetDocuments(baseUrl,
            apiKey, datasetId, documentId: documentId);

        if (documents.Count == 0)
        {
            throw new RagFlowException($"文档 {documentId} 不存在");
        }

        return documents[0];
    }

    /// <summary>
    /// 删除文档
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="ids">要删除的文档 ID 列表，空列表表示删除所有</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> DeleteDocuments(string baseUrl,
        string apiKey, string datasetId, List<string> ids = null)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/documents";
        var restRequest = new RestRequest(url, Method.Delete);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(new { ids = ids ?? new List<string>() });

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "删除文档失败");
    }

    /// <summary>
    /// 解析文档
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentIds">要解析的文档 ID 列表</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> ParseDocuments(string baseUrl,
        string apiKey, string datasetId, List<string> documentIds)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/chunks";
        var restRequest = new RestRequest(url, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(new { document_ids = documentIds });

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "解析文档失败");
    }

    /// <summary>
    /// 停止解析文档
    /// </summary>
    /// <param name="baseUrl">RagFlow API 基础地址</param>
    /// <param name="apiKey">API 密钥</param>
    /// <param name="datasetId">数据集 ID</param>
    /// <param name="documentIds">要停止解析的文档 ID 列表</param>
    /// <returns>操作结果</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static async Task<bool> StopParsingDocuments(string baseUrl,
        string apiKey, string datasetId, List<string> documentIds)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/v1/datasets/{datasetId}/chunks";
        var restRequest = new RestRequest(url, Method.Delete);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Authorization", $"Bearer {apiKey}");
        restRequest.AddJsonBody(new { document_ids = documentIds });

        var client = new RestClient();
        var response = await client.ExecuteAsync(restRequest);

        return RagFlowResponse.HandleResponse(response, "停止解析文档失败");
    }

    /// <summary>
    /// 重新生成空白文件
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="apiKey"></param>
    /// <param name="datasetId"></param>
    /// <param name="docName"></param>
    /// <returns></returns>
    public static async Task<Res_DocumentInfo> ReGenBlankDocument(
        string baseUrl, string apiKey, string datasetId, string docName)
    {
        var (documents, total) = await GetDocuments(
            baseUrl, apiKey, datasetId);
        var taskDoc = documents.Find(x => x.Name == docName);
        if (taskDoc != null)
        {
            await FunRagFlowChunk.DeleteChunks(baseUrl, apiKey, datasetId, taskDoc.Id);
            await DeleteDocuments(baseUrl, apiKey, datasetId,
                new List<string> { taskDoc.Id });
        }
        taskDoc = await UploadDocument(baseUrl, apiKey,
            datasetId, docName, new MemoryStream());
        return taskDoc;
    }
}
