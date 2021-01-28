using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Shell;

namespace GoldenSunEditor
{
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();

            this.Loaded += WindowLoad;
            this.MouseDown += WindowDrag;
        }

        private void WindowDrag (object sender, MouseButtonEventArgs e)
        {
            this.DragMove ();
        }

        private void WindowLoad (object sender, RoutedEventArgs e)
        {
            WindowInteropHelper windowHelper = new WindowInteropHelper (this);

            BlurryHelper.EnableBlur (windowHelper);

            Canvas canvas = UIHelper.GetVisualChild <Canvas> (this, "canvas1");

            StackPanel stackPanel = new StackPanel()
            {
                Margin = new Thickness (10, 10, 10, 10)
            };

            canvas.Children.Add (stackPanel);

            Button [] buttons = new Button [3];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons [i] = new Button ()
                {
                    Content = "BUTTON" + " " + (i + 1),
                    Margin = new Thickness (0, 0, 0, 10),
                    Width = 100,
                    Height = 25,
                };

                buttons [i].Click += new RoutedEventHandler (LoadWindowMain);

                stackPanel.Children.Add (buttons [i]);
            }




            /*
            var helper = new WindowInteropHelper (this);
            var hwnd = helper.Handle;
            var src = HwndSource.FromHwnd (hwnd);

            src.CompositionTarget.BackgroundColor = Colors.Transparent;

            WindowChrome windowChrome = new WindowChrome()
            {
                CaptionHeight = 500,
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                NonClientFrameEdges = NonClientFrameEdges.None,
                ResizeBorderThickness = new Thickness(0),
                UseAeroCaptionButtons = false
            };

            WindowChrome.SetWindowChrome (this, windowChrome);

            PathGeometry path = new PathGeometry();
            //path.StartFigure();
            path.AddGeometry(new RectangleGeometry() { Rect = new Rect (0,0,100,100)});//AddArc(new RectangleF(0, 0, 500, 500), 0, 360);
            //path.CloseFigure();

            var dwmBlurBehind = new DwmBlurBehind (true);
            dwmBlurBehind.SetRegion(Graphics.FromHwnd(hwnd), new Region(path));
            DwmApi.DwmEnableBlurBehindWindow(hwnd, ref dwmBlurBehind);
            */
        }






        private void LoadWindowMain (object sender, RoutedEventArgs e)
        {
            Window windowMain = new WindowMain();

            windowMain.Show();

            this.Close();
        }








        /*
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();

                this.SourceInitialized += MainWindow_SourceInitialized;
                this.KeyDown += MainWindow_KeyDown;
            }

            void MainWindow_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Escape) this.Close();
            }

            void MainWindow_SourceInitialized(object sender, EventArgs e)
            {
                var helper = new WindowInteropHelper(this);
                var hwnd = helper.Handle;
                var src = HwndSource.FromHwnd(hwnd);

                src.CompositionTarget.BackgroundColor = Colors.Transparent;

                WindowChrome.SetWindowChrome(this, new WindowChrome
                {
                    CaptionHeight = 500,
                    CornerRadius = new CornerRadius(0),
                    GlassFrameThickness = new Thickness(0),
                    NonClientFrameEdges = NonClientFrameEdges.None,
                    ResizeBorderThickness = new Thickness(0),
                    UseAeroCaptionButtons = false
                });

                GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                path.StartFigure();
                path.AddArc(new RectangleF(0, 0, 500, 500), 0, 360);
                path.CloseFigure();

                var dbb = new DwmBlurBehind(true);
                dbb.SetRegion(Graphics.FromHwnd(hwnd), new Region(path));
                DwmApi.DwmEnableBlurBehindWindow(hwnd, ref dbb);
            }
        }
        */
    }
}