using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using KunTunLib;
using KTWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTPicbot
{
    struct BotConfig
    {
        public static bool is_Bot_ON;
        public static bool is_Setu_ON;
        public static bool is_ImageSearch_ON;
        public static string strRegsetu;
        public static string regImageSearch;

    }
    struct BotWhiteList
    {
        public static long[] wlsetu_u;
        public static long[] wlsetu_g;
        public static long[] wlsetu_d;
    }
    public partial class MessageParsing
    {
        public static event Output output;


        #region 消息解析实现

        /// <summary>
        /// 消息解析类
        /// </summary>
        /// <param name="msg"></param>
        public void MsgParsing(string msg)
        {
            output("<<<<<<<[开始进行消息处理]>>>>>>>");

            S2J s2j = JsonConvert.DeserializeObject<S2J>(msg);
            string msgtype = null;
            long id = 0;

            if (s2j.message_type == "private")
            {
                msgtype = "send_private_msg";
                id = s2j.user_id;
            }
            else if (s2j.message_type == "group")
            {
                msgtype = "send_group_msg";
                id = s2j.group_id;
            }
            else if (s2j.message_type == "discuss")
            {
                msgtype = "send_discuss_msg";
                id = s2j.discuss_id;
            }


            if (s2j.message != null)//开始套娃
            {
                try
                {
                    //搜图
                    string regImageSearch = @"^(\[CQ:at,qq=" + Config.loginAccont.ToString() + @"\]\s\[CQ:image)";
                    if (Regex.IsMatch(s2j.message, regImageSearch) && BotConfig.is_ImageSearch_ON)
                    {

                    }
                }
                catch (Exception)
                {

                }

                //setu
                Regex regSetu0 = new Regex(@"(^(咕咕).*([色瑟][图圖])|(setu))|^--setu$");
                Regex regSetu = new Regex(BotConfig.strRegsetu);
                Regex isR18Setu = new Regex(@"([Rr]18)");
                Regex r18SetuKeyword = new Regex(@"(?<=([Rr]18))[\s\S]*?(?=((车|([色瑟][图圖])|([Ss][Ee][Tt][Uu])).*(来|([gG][kK][dD]))))");
                //Regex rg = new Regex(@"(?<=( ))[\s\S]*?(?=( ))");

                output(BotConfig.strRegsetu);
                output(s2j.message);
                output(BotConfig.is_Setu_ON.ToString());

                output(regSetu.IsMatch(s2j.message).ToString());

                if (regSetu.IsMatch(s2j.message) && BotConfig.is_Setu_ON)
                {
                    //判断是否为R18
                    if (isR18Setu.IsMatch(s2j.message))//判断为R18
                    {
                        //尝试捕获关键词
                        try
                        {
                            string keyword = r18SetuKeyword.Match(s2j.message).Value;
                            SendSetu(s2j, msgtype, id, 1, "&keyword=" + keyword);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (BotWhiteList.wlsetu_g.Length == 0 || BotWhiteList.wlsetu_u.Length == 0 || BotWhiteList.wlsetu_d.Length == 0 || BotWhiteList.wlsetu_g.Contains(id) || BotWhiteList.wlsetu_u.Contains(id) || BotWhiteList.wlsetu_d.Contains(id))//判断非R18
                    {

                        SendSetu(s2j, msgtype, id);
                    }
                    else
                    {

                        MsgSend(msgtype, "不被允许的操作", id);
                    }
                }


            }

        }



        /// <summary>
        /// setu发送方法
        /// </summary>
        /// <param name="s2j"></param>
        /// <param name="msgtype"></param>
        /// <param name="id"></param>
        /// <param name="r18">是否使用R18模式[0否][1是][2混合]</param>
        /// <param name="keyword">关键词</param>
        void SendSetu(S2J s2j, string msgtype, long id, int r18 = 0, string keyword = null)
        {
            string urlSetu = @"https://api.lolicon.app/setu/?" + "r18=" + r18 + keyword;

            try
            {
                string s = HttpHelper.GetResponseString(HttpHelper.CreateGetHttpResponse(urlSetu));
                Debug.WriteLine(s);
                JObject o = JObject.Parse(s);
                int code = (int)o["code"];
                if (code == 0)
                {
                    string url = (string)o["data"][0]["url"];
                    string msgat = @"[CQ:at,qq=" + s2j.user_id + "]";
                    string msginfo = @"[PID:" + (string)o["data"][0]["pid"] + "][URL:" + url + "]";
                    if (msgtype == "send_private_msg")
                    {
                        MsgSend(msgtype, msginfo, id);
                    }
                    else
                    {
                        MsgSend(msgtype, msgat + msginfo, id);
                    }

                    MsgSend(msgtype, "[CQ:image,file=" + url + "]", id);
                }
                else if (code == 404)
                {
                    MsgSend(msgtype, "没有符合条件的色图", id);
                }
                else if (code == 429)
                {
                    MsgSend(msgtype, "达到调用额度限制", id);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        void MsgSend(string type, string msg, long id)
        {
            JObject j2s = null;
            if (type == "send_private_msg")
            {
                j2s = JObject.FromObject(new
                {
                    action = type,
                    @params = new
                    {
                        user_id = id,
                        message = msg
                    }
                });
            }
            else if (type == "send_group_msg")
            {
                j2s = JObject.FromObject(new
                {
                    action = type,
                    @params = new
                    {
                        group_id = id,
                        message = msg
                    }
                });
            }
            else if (type == "send_discuss_msg")
            {
                j2s = JObject.FromObject(new
                {
                    action = type,
                    @params = new
                    {
                        discuss_id = id,
                        message = msg
                    }
                });
            }

            WebSocketHelper.websocket.Send(j2s.ToString());
            output(j2s.ToString());
        }

        void MsgDel(long id)
        {
            JObject j = JObject.FromObject(new
            {
                action = "delete_msg",
                @params = new
                {
                    message_id = id
                }
            });
            Thread.Sleep(120000);
            WebSocketHelper.websocket.Send(j.ToString());
            Debug.WriteLine(j.ToString());
        }



        #region S2J,J2S


        class S2J
        {
            public long group_id { get; set; }
            public long message_id { get; set; }
            public long user_id { get; set; }
            public long discuss_id { get; set; }
            public string message_type { get; set; }
            public string post_type { get; set; }
            public int times { get; set; }
            public string message { get; set; }
            public int retcode { get; set; }
            public string status { get; set; }
        }
        //public class J2S
        //{
        //    public string action { get; set; }
        //    public Params @params { get; set; }
        //}
        //public class Params
        //{
        //    public int user_id { get; set; }
        //    public int group_id { get; set; }
        //    public int discuss_id { get; set; }
        //    public string message { get; set; }
        //}

        #endregion
        #endregion

        #region 格式化消息
        public string FormatMessage(string msg)
        {

            return msg;
        }
        #endregion

    }
}
