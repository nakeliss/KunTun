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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTPicbot
{
    public partial class ConfigHelper
    {
        public static event Output output;        

        string configFilePath =Config.configFolderPath+ @"/KTPicBot_Config.json";
        string whitelistFilePath = Config.configFolderPath+ @"/KTPicBot_WhiteList.json";

        #region 配置文件读取
        public void ConfigLoad()
        {
            if (!new FileInfo(configFilePath).Exists)
            {
                CreateDefaultConfigFile();
            }

            try
            {
                JObject o = JObject.Parse(File.ReadAllText(configFilePath));

                BotConfig.is_Bot_ON = (bool)o["Main"]["Is_Bot_ON"];
                BotConfig.is_Setu_ON = (bool)o["Main"]["Is_Setu_ON"];
                BotConfig.is_ImageSearch_ON = (bool)o["Main"]["Is_ImageSearch_ON"];

                BotConfig.strRegsetu = (string)o["Setu"]["Reg"];

                output("瑟图正则：" + BotConfig.strRegsetu);
                output("是否启用瑟图：" + BotConfig.is_Setu_ON.ToString());
                output("是否启用图片搜索：" + BotConfig.is_ImageSearch_ON.ToString());

                output("配置加载完成");
            }

            catch (Exception ex)
            {
                output("配置加载失败");
                output(ex.Message);
            }

        }
        #endregion


        #region 白名单配置读取
        public void WhiteListLoad()
        {
            if (!new FileInfo(whitelistFilePath).Exists)
            {
                CreateDefaultWhiteListFile();
            }

            try
            {
                Dictionary<string, Dictionary<string, int[]>> aa = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int[]>>>(File.ReadAllText(whitelistFilePath));
                BotWhiteList.wlsetu_u = new long[aa["Setu"]["User_id"].Length];
                for (int i = 0; i < aa["Setu"]["User_id"].Length; i++)
                {
                    BotWhiteList.wlsetu_u[i] = Convert.ToInt32(aa["Setu"]["User_id"].GetValue(i));
                    output("用户白名单：[" + i + "][" + BotWhiteList.wlsetu_u[i].ToString() + "]");
                }

                BotWhiteList.wlsetu_g = new long[aa["Setu"]["Group_id"].Length];
                for (int i = 0; i < aa["Setu"]["Group_id"].Length; i++)
                {
                    BotWhiteList.wlsetu_g[i] = Convert.ToInt32(aa["Setu"]["Group_id"].GetValue(i));
                    output("群组白名单：[" + i + "][" + BotWhiteList.wlsetu_g[i].ToString() + "]");
                }

                BotWhiteList.wlsetu_d = new long[aa["Setu"]["Discuss_id"].Length];
                for (int i = 0; i < aa["Setu"]["Discuss_id"].Length; i++)
                {
                    BotWhiteList.wlsetu_d[i] = Convert.ToInt32(aa["Setu"]["Discuss_id"].GetValue(i));
                    output("讨论组白名单：[" + i + "][" + BotWhiteList.wlsetu_d[i].ToString() + "]");
                }
                output("白名单加载完成");
            }
            catch (Exception ex)
            {
                output("白名单加载失败");
                output(ex.Message);
            }

        }

        #endregion

        //默认白名单文件
        void CreateDefaultWhiteListFile()
        {
            JObject defWL = JObject.FromObject(new
            {
                Setu = new
                {
                    Group_id = new JArray { },
                    User_id = new JArray { },
                    Discuss_id = new JArray { }
                },
            });
            File.WriteAllText(whitelistFilePath, defWL.ToString());

        }
        //默认配置文件
        void CreateDefaultConfigFile()
        {
            JObject defCfg = JObject.FromObject(new
            {
                Main = new
                {
                    Is_Bot_ON=false,
                    Is_Setu_ON = false,
                    Is_ImageSearch_ON = true,
                },
                Setu = new
                {
                    Reg = @"^(咕咕).*([色瑟][图圖])|(setu)|^--setu$",//setu 正则表达式
                    RegPic = @"",//图片触发setu
                },
                ImageSearch = new
                {

                }
            });
            File.WriteAllText(configFilePath, defCfg.ToString());
        }








    }
}
