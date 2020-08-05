using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes;
using KunTunLib;
using KTWebSocket;
using Newtonsoft.Json.Linq;
using MaterialDesignThemes.Wpf;

namespace KTPicbot
{

    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IView))]
    [ExportPluginMetadata(3, "图片机器人", "图片机器人", "nakeliss", "1.0")]
    public partial class KTPicBot : UserControl, IView
    {
        ConfigHelper configHelper = new ConfigHelper();
        MessageParsing messageParsing = new MessageParsing();

        public KTPicBot()
        {
            InitializeComponent();
            MessageParsing.output+= new Output(Output);
            ConfigHelper.output += new Output(Output);
            configHelper.ConfigLoad();
            configHelper.WhiteListLoad();

            ON_OFFBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99FF0000"));

            WebSocketHelper.eMsg2Parsing += WebSocketHelper_eMsg2Parsing;
        }

        bool on_off =false;
        private void WebSocketHelper_eMsg2Parsing(string msg)
        {
            if (on_off)
            {
                messageParsing.MsgParsing(msg);
            }

        }



        /// <summary>
        /// 消息输出到文本框
        /// </summary>
        /// <param name="msg"></param>
        private void Output(string msg)
        {
            OutputField.Dispatcher.Invoke(new Action(() =>
            {
                OutputField.AppendText("\n" + msg);
                OutputField.ScrollToEnd();
            }));
        }

        private void ReloadConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Output("开始重载配置文件···");
            configHelper.ConfigLoad();
            Output("重载结束");
        }

        private void ReloadWhiteListButton_Click(object sender, RoutedEventArgs e)
        {
            Output("开始重载白名单文件···");
            configHelper.WhiteListLoad();
            Output("重载结束");
        }





        private void ON_OFF_Checked(object sender, RoutedEventArgs e)
        {
            on_off = true;
            ON_OFFstatus.Content = "已启用";
            ON_OFFBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9900BF23"));
        }

        private void ON_OFF_Unchecked(object sender, RoutedEventArgs e)
        {
            on_off = false;
            ON_OFFstatus.Content = "未启用";
            ON_OFFBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99FF0000"));
        }
    }
}
