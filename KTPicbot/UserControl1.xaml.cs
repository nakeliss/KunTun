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
using KunTunPluginLib;

namespace KTPicbot
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IView))]
    [ExportPluginMetadata(1, "图片机器人", "这是图片机器人", "nakeliss", "1.0")]
    public partial class UserControl1 : UserControl, IView
    {
        public UserControl1()
        {
            InitializeComponent();
        }
    }
}
