
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTunLib
{
    public struct Config
    {
        public static long loginAccont;
        public static string dataFolderPath = @"./data";
        public static string configFolderPath= @"./config";
        public static string websocketServerAddress= @"ws://localhost:6700";
    }


    public partial class Core
    {
        public static void LoadAll()
        {
            if (!new DirectoryInfo(Config.configFolderPath).Exists)
            {
                Directory.CreateDirectory(Config.configFolderPath);
            }
            if (!new DirectoryInfo(Config.dataFolderPath).Exists)
            {
                Directory.CreateDirectory(Config.dataFolderPath);
            }

            ConfigHelper configHelper = new ConfigHelper();
            configHelper.LoadConfigFile();
        }
    }
}
