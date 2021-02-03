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

            this.Loaded += ThisLoaded;
            this.MouseDown += ThisMouseDrag;
        }

        private void ThisMouseDrag (object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                this.DragMove ();
        }

        private void ThisLoaded (object sender, RoutedEventArgs e)
        {
            BlurryHelper.EnableBlur (new WindowInteropHelper (this));                           // Window Blur Effect

            InterfaceCreate ();
        }

        private void LoadWindowMain (object sender, RoutedEventArgs e)
        {
            Window windowMain = new WindowMain();

            windowMain.Show();

            this.Close();
        }

        private void ButtonMinimiseWindowClick (object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonCloseWindowClick (object sender, RoutedEventArgs e)
        {
            this.Close ();
        }

        private void ButtonOpenROMClick (object sender, RoutedEventArgs e)
        {
            bool openFile = ROMInOut.OpenFile ();

            if (openFile == true)
                LoadWindowMain (sender, e);
            else
                return;
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
                StackPanel stackPanelMomma = new StackPanel ()
                {
                    Orientation = Orientation.Vertical,
                    Width = canvas.Width,
                    Height = canvas.Height
                };

                canvas.Children.Add (stackPanelMomma);

                    // WINDOW BUTTONS STACKPANEL
                    StackPanel stackPanelWindowButtons = new StackPanel ()
                    {
                        Orientation = Orientation.Horizontal,
                        Width = stackPanelMomma.Width,
                        FlowDirection = FlowDirection.RightToLeft
                    };

                    stackPanelMomma.Children.Add (stackPanelWindowButtons);

                        // CLOSE BUTTON
                        Button buttonCloseWindow = new Button ()
                        {
                            Width = 16,
                            Height = 16,
                            Style = (Style) FindResource ("buttonRed"),
                            Margin = new Thickness (10),
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        buttonCloseWindow.Click += ButtonCloseWindowClick;

                        stackPanelWindowButtons.Children.Add (buttonCloseWindow);

                        // MINIMISE BUTTON
                        Button buttonMinimiseWindow = new Button ()
                        {
                            Width = 16,
                            Height = 16,
                            Style = (Style) FindResource ("buttonGreen"),
                            Margin = new Thickness (10),
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        buttonMinimiseWindow.Click += ButtonMinimiseWindowClick;

                        stackPanelWindowButtons.Children.Add (buttonMinimiseWindow);

                    // ROM BUTTONS STACKPANEL
                    StackPanel stackPanelROMButtons = new StackPanel ()
                    {
                        Orientation = Orientation.Vertical,
                        Width = stackPanelMomma.Width
                    };

                    stackPanelMomma.Children.Add (stackPanelROMButtons);
                    
                        //ROM BUTTONS
                        Button buttonOpenROM = new Button ()
                        {
                            Width = stackPanelROMButtons.Width / 2,
                            Height = 30,
                            Content = "Load ROM"
                        };
                        
                        buttonOpenROM.Click += ButtonOpenROMClick;

                        stackPanelROMButtons.Children.Add (buttonOpenROM);
        }
    }
}