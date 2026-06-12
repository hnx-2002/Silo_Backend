using Microsoft.AspNetCore.SignalR;
using netDxf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 使用SignalR发送消息
/// </summary>
public class ChatResHub : Hub
{
    /// <summary>
    /// 以个人账号为房间号建立链接
    /// 若无Token则无法建立链接
    /// </summary> 
    /// <param name="taskIdStr"></param>
    /// <returns></returns>
    public async Task OnConnectedWithAccount(string taskIdStr)
    {
        //var (account, userName) = ScopeUser.GetUserAccountName();
        if (!string.IsNullOrEmpty(taskIdStr))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, taskIdStr);
            FunConsole.Log(taskIdStr, System.ConsoleColor.Cyan);
        }
    }

    /// <summary>
    /// 发送状态消息
    /// </summary>
    /// <param name="msgHub"></param>
    /// <param name="taskId"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static void SendStatus(
        IHubContext<ChatResHub> msgHub,
        string taskId, string msg)
    {
        Task.Run(async () =>
        {
            var group = msgHub.Clients.Group(taskId);
            if (group != null)
            {
                await group.SendAsync("ReceiveStatus", msg);
                FunConsole.Log(msg, System.ConsoleColor.Cyan);
            }
        });
    }

    /// <summary>
    /// 发送Response消息
    /// </summary>
    /// <param name="msgHub"></param>
    /// <param name="taskId"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static void SendResponse(
        IHubContext<ChatResHub> msgHub,
        string taskId, string msg)
    {
        Task.Run(async () =>
        {
            var group = msgHub.Clients.Group(taskId);
            if (group != null)
            {
                await group.SendAsync("ReceiveResponse", msg);
            }
        });
    }

    /// <summary>
    /// 发送Response消息
    /// </summary>
    /// <param name="msgHub"></param>
    /// <param name="taskId"></param> 
    /// <returns></returns>
    public static void SendResponseDone(
        IHubContext<ChatResHub> msgHub,
        string taskId)
    {
        Task.Run(async () =>
        {
            var group = msgHub.Clients.Group(taskId);
            if (group != null)
            {
                await group.SendAsync("ReceiveResponseDone");
            }
        });
    }

}
