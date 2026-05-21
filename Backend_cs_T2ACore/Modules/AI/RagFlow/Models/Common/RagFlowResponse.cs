using Newtonsoft.Json;
using RestSharp;
using System;

namespace T2ACore;

/// <summary>
/// API 响应基类
/// </summary>
public class RagFlowResponse
{
    /// <summary>
    /// 响应码，0 表示成功
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 处理 API 响应
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    /// <param name="response">API 响应</param>
    /// <param name="errorMessage">默认错误消息</param>
    /// <returns>响应数据</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static T HandleResponse<T>(RestResponse response, string errorMessage)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<RagFlowResponse<T>>(response.Content);
                if (res.Code == 0)
                {
                    return res.Data;
                }
                else
                {
                    throw new RagFlowException($"{errorMessage}: 错误码 {res.Code} - {res.Message}");
                }
            }
            catch (Exception ex)
            {
                FunConsole.Log("出错，原始数据为：" + response.Content + "\r\n" + ex.ToString(), ConsoleColor.Red);
                throw;
            }
        }
        else
        {
            FunConsole.Log("StatusCode为：" + response.StatusCode.ToString() +
                "\r\n返回值为：" + response.Content, ConsoleColor.DarkRed);
            throw new RagFlowException($"{errorMessage}: {response.StatusCode}");
        }
    }

    /// <summary>
    /// 处理 API 响应（无数据）
    /// </summary>
    /// <param name="response">API 响应</param>
    /// <param name="errorMessage">默认错误消息</param>
    /// <returns>是否成功</returns>
    /// <exception cref="RagFlowException">API 调用失败时抛出</exception>
    public static bool HandleResponse(RestResponse response, string errorMessage)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<RagFlowResponse>(response.Content);
                if (res.Code == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                    throw new RagFlowException($"{errorMessage}: 错误码 {res.Code} - {res.Message}");
                }
            }
            catch (Exception ex)
            {
                FunConsole.Log("出错，原始数据为：" + response.Content + "\r\n" + ex.ToString(), ConsoleColor.Red);
                throw;
            }
        }
        else
        {
            FunConsole.Log("StatusCode为：" + response.StatusCode.ToString() +
                "\r\n返回值为：" + response.Content, ConsoleColor.DarkRed);
            throw new RagFlowException($"{errorMessage}: {response.StatusCode}");
        }
    }

}

/// <summary>
/// 泛型 API 响应
/// </summary>
/// <typeparam name="T">响应数据类型</typeparam>
public class RagFlowResponse<T> : RagFlowResponse
{
    /// <summary>
    /// 响应数据
    /// </summary>
    public T Data { get; set; }
}
