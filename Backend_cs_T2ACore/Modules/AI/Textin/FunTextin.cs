using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// Textin方法
/// </summary>
public static class FunTextin
{
    /// <summary>
    /// textin合规扩展名
    /// </summary>
    private static List<string> ValidTypes
    {
        get
        {
            return new List<string> {
                "jpg", "jpeg", "bmp", "pdf", "doc", "docx",
                "webp", "tif", "tiff", "html", "mhtml",
                "xls", "xlsx", "ppt", "pptx", "wps" };
        }
    }

    /// <summary>
    /// 文件格式预检
    /// </summary>
    /// <param name="fileExt"></param> 
    /// <returns></returns> 
    public static (bool Status, string Msg) PreFileTypeCheck(string fileExt)
    {
        var status = ValidTypes.Contains(fileExt);
        var msg = status ? "OK" : "不支持" + fileExt + "格式";
        return (status, msg);
    }

    /// <summary>
    /// 执行处理
    /// </summary>
    /// <param name="fileName"></param>  
    /// <param name="ms"></param>
    /// <param name="baseUrl"></param>
    /// <param name="appKey"></param>  
    /// <returns></returns>
    public static (bool Status, string Msg, string taskId, int StatusCode) Execute(
        string fileName, MemoryStream ms, string baseUrl, string appKey)
    {
        Res_TextInCreate resTextIn = FunTextInHttp.CreateTask(
            baseUrl, appKey, fileName, ms.ToArray());

        if (resTextIn == null)
        {
            return (false, "请求结果为null", null, 0);

        }

        if (resTextIn.Code != 200)
        {
            return (false, resTextIn.Msg, null, 0);
        }
        else
        {
            return (true, "OK", resTextIn.Data.Task_ids.First(), 11);
        }
    }

    /// <summary>
    /// 检查任务状态
    /// </summary> 
    /// <param name="baseUrl"></param>
    /// <param name="appKey"></param>
    /// <param name="taskId"></param>
    /// <param name="fileName"></param> 
    /// <param name="fileBusiness"></param>
    /// <param name="account"></param>
    /// <param name="username"></param>
    /// <param name="tenant"></param>
    /// <param name="getType">`md`,`xlsx`，默认md</param>
    /// <returns></returns>
    public static async Task<(bool Status, string Msg, string resContent, string ResPath)> Check(
        string baseUrl, string appKey, string taskId, string fileName, IFileMange_Business fileBusiness,
        string account, string username, string tenant, string getType = "md")
    {
        (bool resStatus, string resMsg, string resContent, byte[] resBytes) =
            await GetText(taskId, baseUrl, appKey, getType);
        if (!resStatus)
        {
            return (false, resMsg, null, null);
        }

        byte[] fileBytes = getType == "md"
                           ? Encoding.UTF8.GetBytes(resContent)
                           : resBytes;
        var resUpload = await fileBusiness.UploadFileByBytes(fileBytes,
            fileName + "." + getType, "application/octet-stream",
            account, username, tenant);

        if (resUpload.Status)
        {
            return (true, resUpload.Msg, resContent, resUpload.FilePath);
        }
        else
        {
            return (false, resUpload.Msg, null, "");
        }


    }

    /// <summary>
    /// 获取Textin返回值
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="baseUrl"></param>
    /// <param name="appKey"></param>
    /// <param name="getType">`md`,`xlsx`，默认md</param>
    /// <returns></returns>
    public static async Task<(bool Status, string Msg, string MD, byte[] fileBytes)> GetText(
        string taskId, string baseUrl, string appKey, string getType = "md")
    {
        while (true)
        {
            var resList = FunTextInHttp.GetList(baseUrl, appKey);
            if (resList.Code != 200)
            {
                return (false, resList.Msg, null, null);
            }

            var taskData = resList.Data.List.Find(x => x.Task_id == taskId);
            if (taskData == null)
            {
                return (false, "执行失败，请重试", null, null);
            }

            switch (taskData.Status)
            {
                case 2:

                    if (getType == "xlsx")
                    {
                        var xlsxBytes = FunTextInHttp.GetResultExcel(
                            baseUrl, appKey, taskId);
                        return (true, "OK", null, xlsxBytes);
                    }
                    else
                    {
                        var mdStr = FunTextInHttp.GetResultMarkdown(
                            baseUrl, appKey, taskId);
                        return (true, "OK", mdStr, null);

                    }

                case 3:
                case -1:
                    return (false, "文本提取失败，请重试", null, null);

                default:
                    await Task.Delay(1000); // ✅ 非阻塞等待
                    continue;
            }
        }
    }
}
