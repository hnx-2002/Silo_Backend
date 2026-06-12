using Microsoft.Extensions.Logging;
using System;

namespace T2ACore;

/// <summary>
/// 日志类
/// </summary>
public sealed class Logger
{
    private static ILoggerFactory _hostLoggerFactory;

    /// <summary>
    /// Logger 变量
    /// </summary>
    public static ILogger LogInstance { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="loggerFactory"></param>
    public static void Initialize(ILoggerFactory loggerFactory)
    {
        // Debug.Assert(_hostLoggerFactory == null);
        _hostLoggerFactory = loggerFactory;
        LogInstance = loggerFactory.CreateLogger("GlobalLogger");
    }

    /// <summary>
    /// 创建Logger
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public static ILogger CreateLogger(string categoryName)
    {
        return _hostLoggerFactory.CreateLogger(categoryName);
    }

    /// <summary>
    ///创建Logger
    /// </summary> 
    /// <returns></returns>
    public static ILogger CreateLogger<T>()
    {
        return _hostLoggerFactory.CreateLogger(typeof(T).FullName ?? "GlobalLogger");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Trace(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Debug(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Info(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogInformation(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Warn(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogWarning(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Error(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void Fatal(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Trace(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(strMsg, args);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Debug(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Info(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogInformation(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Warn(string strMsg, params object[] args)
    {
        LogInstance.LogWarning(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="err"></param>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Error(Exception err, string strMsg, params object[] args)
    {
        LogInstance.LogError(err, strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Error(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void Fatal(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Trace(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Debug(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Info(string strMsg, Exception err)
    {
        LogInstance.LogInformation(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Warn(string strMsg, Exception err)
    {
        LogInstance.LogWarning(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Error(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void Fatal(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void TraceAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void DebugAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void InfoAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogInformation(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void WarnAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogWarning(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void ErrorAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    public static void FatalAuto(string strMsg)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void TraceAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void DebugAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void InfoAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogInformation(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void WarnAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogWarning(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void ErrorAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="args"></param>
    public static void FatalAuto(string strMsg, params object[] args)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(strMsg, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void TraceAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogTrace(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void DebugAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogDebug(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void InfoAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogInformation(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void WarnAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogWarning(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void ErrorAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogError(err, strMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="err"></param>
    public static void FatalAuto(string strMsg, Exception err)
    {
        strMsg = strMsg.Replace("\r\n", "");
        LogInstance.LogCritical(err, strMsg);
    }
}
