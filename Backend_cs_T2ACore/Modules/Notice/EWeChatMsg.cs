// cyl added 20220623
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace T2ACore
{
    /// <summary>
    /// 企业微信消息推送
    /// </summary>
    public class EWeChatMsg
    {
        /// <summary>
        /// 企业微信消息规范化，将用户传进来的消息规范化为，满足企业微信接口的格式
        /// </summary>
        /// <param name="touser">用户列表(企业微信Id)</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="agentid">自检应用Id</param>
        /// <param name="content">消息内容(根据消息类型决定)</param>
        /// <param name="safe">默认0</param>
        /// <param name="enable_id_trans">默认0</param>
        /// <param name="enable_duplicate_check">默认0</param>
        /// <param name="saduplicate_check_intervalfe">默认0</param>
        /// <returns></returns>
        public static string EWeChatMsgNormalize(List<string> touser,
                                                 string msgType,
                                                 string agentid,
                                                 JObject content,
                                                 int safe = 0,
                                                 int enable_id_trans = 0,
                                                 int enable_duplicate_check = 0,
                                                 int saduplicate_check_intervalfe = 1800)
        {
            if (!touser.Any())
            {
                return "touser is empty";
            }
            var jo = new JObject
                        {
                            { "touser", string.Join("|", touser.ToArray())},
                            { "msgtype", msgType},
                            { "agentid", agentid},
                            { msgType, content  },
                            { "safe", safe},
                            { "enable_id_trans", enable_id_trans},
                            { "enable_duplicate_check", enable_duplicate_check},
                            { "duplicate_check_interval", saduplicate_check_intervalfe},
                        };
            return JsonConvert.SerializeObject(jo);
        }
    }
}
