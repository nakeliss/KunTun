using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KunTunLib;

namespace KunTun
{


    public partial class MainWindow
    {
        [ImportMany]
        public Lazy<IView, IMetadata>[] Plugins { get; set; }
        private CompositionContainer container = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowX_ContentRendered(object sender, EventArgs e)
        {

            var dir = new DirectoryInfo(@"./plugins");
            if (!dir.Exists)
            {
                dir.Create();
            }

            if (dir.Exists)
            {
                var catalog = new DirectoryCatalog(dir.FullName, "*.dll");
                container = new CompositionContainer(catalog);

                try
                {
                    container.ComposeParts(this);
                }
                catch (CompositionException compositionEx)
                {
                    Console.WriteLine(compositionEx.ToString());
                    MessageBox.Show(compositionEx.ToString());
                }

                Plugins.OrderBy(p => p.Metadata.Priority);


                int i = 0;
                Thickness PluginsMenuBtnsThickness = new Thickness() { Left = 10, Top = 10, Right = 10, Bottom = 0 };
                FontFamily PluginsMenuBtnsFontFamily = new FontFamily("Microsoft YaHei");


                foreach (var plugin in Plugins)
                {
                    i = i++;
                    Button btn;
                    PluginsMenu.Children.Add(btn = new Button()
                    {
                        Content = plugin.Metadata.Name,
                        Margin = PluginsMenuBtnsThickness,
                        FontFamily= PluginsMenuBtnsFontFamily,
                        Name= "PluginsMenuBtns"+i,


                    }) ;

                    Grid grid;
                    PluginsContents.Children.Add(grid=new Grid() { 
                        Name="PluginsContentsGrid"+i,
                        Visibility = Visibility.Hidden,
                    });
                    grid.Children.Add((UIElement)plugin.Value);


                    btn.Click += (o, j) => {
                        foreach (UIElement item in PluginsContents.Children)
                        {
                            item.Visibility =Visibility.Hidden ;
                            grid.Visibility = Visibility.Visible;
                        }
                    };
                }
            }


        }


        protected override void OnClosing(CancelEventArgs e)
        {
            container?.Dispose();
            base.OnClosing(e);

        }

        private void mBtnHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tgeMainMenuPin_Checked(object sender, RoutedEventArgs e)
        {
            tgeMainMenu.IsEnabled = false;
        }

        private void tgeMainMenuPin_Unchecked(object sender, RoutedEventArgs e)
        {
            tgeMainMenu.IsEnabled = true;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            windowX.Close();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);

            //Storyboard sb = new Storyboard();
            //DoubleAnimation da1 = new DoubleAnimation()
            //{
            //    From = windowX.Width,
            //    To= SystemParameters.WorkArea.Width,
            //    //AutoReverse = true,
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500)),
            //};
            //DoubleAnimation da2 = new DoubleAnimation()
            //{
            //    From = windowX.Height,
            //    To= SystemParameters.WorkArea.Height,
            //    //AutoReverse = true,
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500)),
            //};
            //Storyboard.SetTarget(da1, windowX);
            //Storyboard.SetTargetProperty(da1, new PropertyPath("Width"));
            //sb.Children.Add(da1);
            //Storyboard.SetTarget(da2, windowX);
            //Storyboard.SetTargetProperty(da2, new PropertyPath("Height"));
            //sb.Children.Add(da2);
            //sb.Begin();


            //windowX.Width = SystemParameters.FullPrimaryScreenWidth;
            //windowX.Height = SystemParameters.FullPrimaryScreenHeight;
            //windowX.Top = (SystemParameters.FullPrimaryScreenHeight - windowX.Height) / 2;
            //windowX.Left = (SystemParameters.FullPrimaryScreenWidth - windowX.Width) / 2;


        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            windowX.WindowState = WindowState.Minimized;
        }

        private void WindowTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            windowX.DragMove();
        }

        private void colorZone_MenuBG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tgeMainMenu.IsChecked = false;
        }

        private void windowX_Initialized(object sender, EventArgs e)
        {
            Core.LoadAll();
        }

        private void windowX_StateChanged(object sender, EventArgs e)
        {
            

            
        }
    }
}
