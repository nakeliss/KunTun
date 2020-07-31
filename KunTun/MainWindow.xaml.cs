using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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

using KunTunPluginLib;


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
            if (dir.Exists)
            {

            }
            else
            {
                dir.Create();
            }

            Thickness PluginsMenuBtnsThickness = new Thickness() { Left = 10, Top = 5, Right = 10, Bottom = 0 };
            FontFamily PluginsMenuBtnsFontFamily = new FontFamily("Microsoft YaHei");

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

                int i = 0;

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

        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
