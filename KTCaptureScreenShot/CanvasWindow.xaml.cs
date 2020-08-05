using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace KTCaptureScreenShot
{
    /// <summary>
    /// CanvasWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CanvasWindow : Window
    {
        private double x;
        private double y;
        private double width;
        private double height;

        private bool isMouseDown = false;

        public CanvasWindow()
        {
            InitializeComponent();
        }

        private void CaptureWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            x = e.GetPosition(null).X;
            y = e.GetPosition(null).Y;
        }

        private void CaptureWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                // 1. 通过一个矩形来表示目前截图区域
                System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                double dx = e.GetPosition(null).X;
                double dy = e.GetPosition(null).Y;
                double rectWidth = Math.Abs(dx - x);
                double rectHeight = Math.Abs(dy - y);
                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                rect.Width = rectWidth;
                rect.Height = rectHeight;
                rect.Fill = brush;
                rect.Stroke = brush;
                rect.StrokeThickness = 1;
                if (dx < x)
                {
                    Canvas.SetLeft(rect, dx);
                    Canvas.SetTop(rect, dy);
                }
                else
                {
                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);
                }

                CaptureCanvas.Children.Clear();
                CaptureCanvas.Children.Add(rect);

                if (e.LeftButton == MouseButtonState.Released)
                {
                    CaptureCanvas.Children.Clear();
                    // 2. 获得当前截图区域
                    width = Math.Abs(e.GetPosition(null).X - x);
                    height = Math.Abs(e.GetPosition(null).Y - y);

                    if (e.GetPosition(null).X > x)
                    {
                        CaptureScreen(x, y, width, height);
                    }
                    else
                    {
                        CaptureScreen(e.GetPosition(null).X, e.GetPosition(null).Y, width, height);
                    }


                    isMouseDown = false;
                    x = 0.0;
                    y = 0.0;
                    this.Close();
                }
            }
        }

        private void CaptureScreen(double x, double y, double width, double height)
        {
            int ix = Convert.ToInt32(x);
            int iy = Convert.ToInt32(y);
            int iw = Convert.ToInt32(width);
            int ih = Convert.ToInt32(height);

            Bitmap bitmap = new Bitmap(iw, ih);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih));
            }

        }
    }
}
