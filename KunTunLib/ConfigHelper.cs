using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KunTunLib
{
    public partial class ConfigHelper
    {
        string configFilePath = @"./config/config.json";


        public void LoadConfigFile()
        {
            var a= new FileInfo(configFilePath).Exists;
            if (a==false)
            {
                try
                {
                    CreateDefaultConfigFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            try
            {
                JObject o = JObject.Parse(File.ReadAllText(configFilePath));
                Config.loginAccont = (long)o["Main"]["LoginAccont"];
                Config.websocketServerAddress= (string)o["Main"]["WebsocketServerAddress"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void CreateDefaultConfigFile()
        {
            JObject defCfg = JObject.FromObject(new
            {
                Main = new
                {
                    LoginAccont = 0,
                    WebsocketServerAddress = @"ws://localhost:6700",
                }
            });

            try
            {
                File.WriteAllText(configFilePath, defCfg.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
