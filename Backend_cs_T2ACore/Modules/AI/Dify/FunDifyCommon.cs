using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

internal class FunDifyCommon
{
    /// <summary>
    /// 流式处理返回值
    /// </summary>
    /// <param name="client">RestSharpClient</param>
    /// <param name="request">RestSharpRequest</param>
    /// <param name="useAudio">是否解析语音</param>
    /// <param name="sendMsgEnd">是否通过SinalR发送Message_end消息</param>
    /// <param name="chatHub">SignalR的hub</param>
    /// <param name="roomName">SignalR的房间名，这里使用任务号</param>
    /// <param name="useVirtualStatusMsg">是否使用虚拟状态信息</param>
    /// <param name="virtualMsgs">虚拟信息集合</param>
    /// <returns></returns>
    public static async Task<(bool, string, string, string)> ExecuteStreamResponse(
        RestClient client, RestRequest request, bool useAudio, bool sendMsgEnd,
        IHubContext<ChatResHub> chatHub, string roomName,
        bool useVirtualStatusMsg, Queue<string> virtualMsgs)
    {
        Stream responseStream = await client.DownloadStreamAsync(request);
        if (responseStream == null)
        {
            return (false, "响应流为空", null, null);
        }
        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

        string textResponse = "";
        string audioResponse = "";

        // 按行读取（假设每块 JSON 以换行符分隔）
        string line;
        Res_DifyGen lineRes;
        bool isError = false;
        string errorMsg = "Error";
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                //FunConsole.ConsoleLog($"JSON内容为{line}", ConsoleColor.Yellow);

                if (line.StartsWith("data: "))
                {
                    line = line.Substring(6);
                }

                // 解析单块 JSON
                lineRes = JsonConvert.DeserializeObject<Res_DifyGen>(line);
                if (lineRes != null)
                {
                    //FunConsole.ConsoleLog($"事件{lineRes.Event}", ConsoleColor.Yellow);

                    if (lineRes.Event == "message")
                    {
                        var msgRes = Res_ChatResHub.New("text", lineRes.Answer);
                        var msgTxt = JsonConvert.SerializeObject(msgRes);

                        textResponse += lineRes.Answer;

                        FunConsole.Log(lineRes.Answer, ConsoleColor.Green);

                        if (chatHub != null && !string.IsNullOrEmpty(roomName))
                        {
                            if (useVirtualStatusMsg) //使用虚拟状态消息
                            {
                                msgTxt = virtualMsgs.Dequeue();
                                ChatResHub.SendStatus(chatHub, roomName, msgTxt);
                                virtualMsgs.Enqueue(msgTxt);
                            }
                            else
                            {
                                ChatResHub.SendResponse(chatHub, roomName, msgTxt);
                            }
                        }

                    }
                    if (lineRes.Event == "message_end")
                    {
                        if (chatHub != null && !string.IsNullOrEmpty(roomName) && sendMsgEnd)
                        {
                            var msgRes = Res_ChatResHub.New("message_end", "");
                            var msgTxt = JsonConvert.SerializeObject(msgRes);
                            ChatResHub.SendResponse(chatHub, roomName, msgTxt);
                        }
                    }
                    else if (lineRes.Event == "tts_message")
                    {
                        if (useAudio)
                        {
                            if (chatHub != null && !string.IsNullOrEmpty(roomName))
                            {
                                var msgRes = Res_ChatResHub.New("audio", lineRes.Audio);
                                var msgTTS = JsonConvert.SerializeObject(msgRes);
                                ChatResHub.SendResponse(chatHub, roomName, msgTTS);
                            }

                            audioResponse += lineRes.Audio;
                        }
                    }
                    else if (lineRes.Event == "error")
                    {
                        isError = true;
                        FunConsole.Log($"发生错误, JSON内容为{line}", ConsoleColor.Red);

                        errorMsg = lineRes.Message;

                        //try
                        //{
                        //    var error = JsonConvert.DeserializeObject<Res_DifyGen>(line);
                        //}
                        //catch (Exception)
                        //{
                        //    return (false, msg, null);
                        //}

                        break;
                    }
                }
            }
            catch (JsonException ex)
            {
                if (line == "event: ping")
                {
                    FunConsole.Log("Dify正在Ping");
                }
                else
                {
                    FunConsole.Debug($"JSON 解析失败: {ex.Message}, JSON内容为{line}");
                }
            }

        }

        if (isError)
        {
            return (false, errorMsg, line, null);
        }

        return (true, "OK", textResponse, audioResponse);
    }

}
