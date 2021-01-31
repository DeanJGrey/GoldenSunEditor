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
using System.Security;
using System.Drawing.Drawing2D;

namespace GoldenSunEditor
{
    public partial class WindowMain : Window
    {
        public WindowMain ()
        {
            InitializeComponent ();

            this.Loaded += WindowLoaded;
            this.MouseDown += WindowMouseDown;
        }

        private void ButtonWindowCloseClick (object sender, RoutedEventArgs e)
        {
            this.Close ();
        }

        private void ButtonWindowMinimiseClick (object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void InterfaceCreate ()
        {
            // DADDY CANVAS
            Canvas canvas = new Canvas ()
            {
                Width = this.Width,
                Height = this.Height
            };

            this.AddChild (canvas);

                // MOMMA STACKPANEL
                DockPanel dockPanelMomma = new DockPanel ()
                {
                    Width = canvas.Width,
                    Height = canvas.Height,
                };

                canvas.Children.Add (dockPanelMomma);

                    // WINDOW BUTTONS STACKPANEL
                    StackPanel stackPanelWindowButtons = new StackPanel ()
                    {
                        Orientation = Orientation.Horizontal,
                        FlowDirection = FlowDirection.RightToLeft
                    };

                    DockPanel.SetDock (stackPanelWindowButtons, Dock.Top);

                    dockPanelMomma.Children.Add (stackPanelWindowButtons);

                        // CLOSE BUTTON
                        Button buttonCloseWindow = new Button ()
                        {
                            Width = 16,
                            Height = 16,
                            Style = (Style) FindResource ("buttonRed"),
                            Margin = new Thickness (10,10,0,10),
                        };

                        buttonCloseWindow.Click += ButtonWindowCloseClick;

                        stackPanelWindowButtons.Children.Add (buttonCloseWindow);

                        // MINIMISE BUTTON
                        Button buttonMinimiseWindow = new Button ()
                        {
                            Width = 16,
                            Height = 16,
                            Style = (Style) FindResource ("buttonGreen"),
                            Margin = new Thickness (10,10,0,10),
                        };

                        buttonMinimiseWindow.Click += ButtonWindowMinimiseClick;

                        stackPanelWindowButtons.Children.Add (buttonMinimiseWindow);

                    // TABCONTROL
                    TabControl tabControl = new TabControl ()
                    {
                        Margin = new Thickness (10, 10, 10, 10)
                    };

                    DockPanel.SetDock (tabControl, Dock.Bottom);

                    dockPanelMomma.Children.Add (tabControl);

                    for (int i = 0; i < 20; i++)
                    {
                        TabItem tabItem = new TabItem ()
                        {
                            Header = "tabItem" + " " + i,
                            Height = 20,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center
                        };

                        tabItem.Content = new CheckBox ();

                        tabControl.Items.Add (tabItem);
                    }
        }

        private void WindowLoaded (object sender, RoutedEventArgs e)
        {
            BlurryHelper.EnableBlur (new WindowInteropHelper (this));             // BLUR

            InterfaceCreate ();
        }

        private void WindowMouseDown (object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                this.DragMove ();
        }
    }
}