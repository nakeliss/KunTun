using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace KTPicbot
{
    public delegate void Output(string msg);
    public delegate void SetServerAddress(string address);
    public delegate void SendMsg(string type, string msg, long id);
    public delegate void ToMsgParsing(string msg);
    public delegate string FormatMsg(string msg);
    public delegate void WsSend(string msg);
    public delegate void WsHelper(string msg);

}
