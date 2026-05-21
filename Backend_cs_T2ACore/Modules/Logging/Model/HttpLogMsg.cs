//cyl added 20220623
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace T2ACore
{
    /// <summary>
    /// Http请求日志
    /// </summary>
    public class HttpLogMsg
    {
        //private static NLog.Logger _logger; 
        //static HttpLogMsg()
        //{
        //    _logger = NLog.LogManager.GetCurrentClassLogger();
        //}

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string OperationTime { get; set; }

        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperationAccount { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 请求接口
        /// </summary>
        public string AccessApiUrl { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public string ReqResultCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 从Http中获取LogInfo
        /// </summary>
        /// <param name="httpContext"></param> 
        /// <param name="account"></param> 
        /// <param name="userName"></param> 
        public static void LogInfo(HttpContext httpContext, string account, string userName)
        {
            var logMessage = GetLogMsg(httpContext, account, userName);
            string logContent = LogInfoFormat(logMessage);
            //RedisLog.Error(logContent);
            //_logger.Info(logContent);
            //return logContent;
            Logger.CreateLogger(nameof(HttpLogMsg)).LogInformation(logContent);
        }

        /// <summary>
        /// 从Http中获取LogInfo，并将异常信息输出
        /// </summary>  
        /// <param name="ex"></param>
        /// <param name="httpContext"></param>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        public static void LogError(Exception ex, HttpContext httpContext, string account, string userName)
        {
            var logMessage = GetLogMsg(httpContext, account, userName);
            string logContent = LogInfoFormat(logMessage);
            //_logger.Error(ex, logContent);
            //return logContent; 
            Logger.CreateLogger(nameof(HttpLogMsg)).LogError(logContent);
        }

        private static HttpLogMsg GetLogMsg(HttpContext httpContext, string account, string userName)
        {
            //var (account, username) = ScopeUser.GetUserAccountName();

            HttpLogMsg logMessage = new HttpLogMsg();
            var start = httpContext.Items["startTime"];
            logMessage.StartTime = start == null ? "" : start.ToString();
            logMessage.IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            logMessage.Method = httpContext.Request.Method;
            logMessage.OperationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff:ffffff");
            logMessage.OperationAccount = account;
            logMessage.OperationName = userName;
            logMessage.AccessApiUrl = ScopeTool.GetAbsoluteURL();

            logMessage.Params = ScopeTool.BodyContent;
            logMessage.ReqResultCode = httpContext.Response.StatusCode.ToString();

            return logMessage;
        }

        /// <summary>
        /// 构造输出格式
        /// </summary>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        public static string LogInfoFormat(HttpLogMsg logMessage)
        {
            StringBuilder strInfo = new StringBuilder();
            strInfo.Append("\r\n开始时间:" + logMessage.StartTime);
            strInfo.Append("\r\n操作时间:" + logMessage.OperationTime);
            strInfo.Append("\r\n操作人账号:" + logMessage.OperationAccount);
            strInfo.Append("\r\n操作人:" + logMessage.OperationName);
            strInfo.Append("\r\n用户Ip:" + logMessage.IpAddress);
            strInfo.Append("\r\nUserAgent:" + ScopeTool.GetBrowseAgent());
            strInfo.Append("\r\nReferer:" + ScopeTool.GetReferer());
            strInfo.Append("\r\n请求接口:" + logMessage.AccessApiUrl);
            strInfo.Append("\r\n请求方法:" + logMessage.Method);
            strInfo.Append("\r\n请求参数:" + logMessage.Params);
            strInfo.Append("\r\n请求结果代码:" + logMessage.ReqResultCode);
            return strInfo.ToString();
        }
    }
}
