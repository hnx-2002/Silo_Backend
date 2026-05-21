using Minio.DataModel.Tracing;
using Minio.Handlers;
using System;
using System.Net;

namespace T2ACore;

/// <summary>
/// mino请求日志记录
/// </summary>
/// <remarks>
/// </remarks>
public class MinioRequestLog : IRequestLogger
{
    /// <summary>
    /// 进度值回调处理
    /// </summary>
    public Action<int> BackLogFunc { get; set; }

    /// <summary>
    /// 日志回写方法
    /// </summary>
    /// <param name="requestToLog"></param>
    /// <param name="responseToLog"></param>
    /// <param name="durationMs"></param>
    public void LogRequest(RequestToLog requestToLog, ResponseToLog responseToLog, double durationMs)
    {
        if (responseToLog.StatusCode == HttpStatusCode.OK)
        {
            foreach (var header in requestToLog.Parameters)
            {
                if (header.Value == null) continue;
                //LogHelpter.AddLog($"minio上传{header.name}:{header.value.ToString()}");
                //partNumber 文件块序号，从1开始
                if (header.Name.Equals("partNumber"))
                {
                    int partNumber = Convert.ToInt32(header.Value);
                    partNumber = partNumber - 1;
                    BackLogFunc(partNumber);
                }
            }
        }
    }


}
