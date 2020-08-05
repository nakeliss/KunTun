using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using System.Xml.Schema;

using KunTunLib;


namespace KTWebSocket
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public delegate void Output(string msg);
    public delegate void MessageToParsing(string msg);
    public delegate void WebSocketStatus(bool status);



    [Export(typeof(IView))]
    [ExportPluginMetadata(1, "WebSocket", "Websocket实现", "nakeliss", "1.0")]
    public partial class KTWebSocketControl : UserControl,IView
    {
        public KTWebSocketControl()
        {
            InitializeComponent();
            CoreInstall();
        }


        public static void SendMessage(string msg)
        {
            WebSocketHelper.websocket.Send(msg);
        }



        private void CoreInstall()
        {
            WebSocketHelper.eOutput += Output;
            WebSocketHelper.eWSStatus += WebSocketHelper_eWSStatus;
        }

        private void WebSocketHelper_eWSStatus(bool status)
        {
            ConnectedButton.Dispatcher.Invoke(new Action(delegate
            {
                if (status)
                {
                    ConnectedButton.IsEnabled = false;
                    DisconnectedButton.IsEnabled = true;
                }
                else
                {
                    ConnectedButton.IsEnabled = true;
                    DisconnectedButton.IsEnabled = false;
                }
            }));
        }

        private void Output(string msg)
        {
            OutputField.Dispatcher.Invoke(new Action(() =>
            {
                OutputField.AppendText("\n" + msg);
                OutputField.ScrollToEnd();

            }));
        }

        private void ConnectedButton_Click(object sender, RoutedEventArgs e)
        {
            Output(">>>>>Starting Connected...");
            WebSocketHelper.WebsocketIn(ServerAddressField.Text);
        }

        private void DisconnectedButton_Click(object sender, RoutedEventArgs e)
        {
            WebSocketHelper.websocket.Close();
        }

        private void KTWebSocketControl_Loaded(object sender, RoutedEventArgs e)
        {
            ServerAddressField.Text = Config.websocketServerAddress;
        }
    }



}
