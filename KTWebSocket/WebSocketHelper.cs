using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KTWebSocket
{
    public partial class WebSocketHelper
    {
        public static event Output eOutput;
        public static event MessageToParsing eMsg2Parsing;
        public static event WebSocketStatus eWSStatus;

        public static WebSocket4Net.WebSocket websocket;

        public static void WebsocketIn(string address)
        {

            try
            {
                websocket = new WebSocket4Net.WebSocket(address);
            }
            catch (Exception ex)
            {
                eOutput(ex.ToString());
            }


            websocket.Opened += async (s, j) =>
            {
                await Task.Run(new Action(() =>
                {
                    eOutput("\nWebsocket已连接,服务已启动,正在等待…");
                    eWSStatus(true);
                }));
            };

            websocket.Error += async (s, j) =>
            {
                await Task.Run(new Action(() => eOutput("\nError:" + j.Exception.ToString())));
            };
            websocket.Closed += async (s, j) =>
            {
                await Task.Run(new Action(() => 
                { 
                    eOutput("\n关闭链接");
                    eWSStatus(false);
                }));
            };
            websocket.MessageReceived += async (s, j) =>
            {
                await Task.Run(new Action(() =>
                {
                    eOutput("\n收到数据\n" + j.Message);
                    eMsg2Parsing(j.Message);

                }));
            };

            websocket.Open();
        }
    }
}
