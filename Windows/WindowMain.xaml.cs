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

namespace GoldenSunEditor
{
    public partial class WindowMain : Window
    {
        public WindowMain ()
        {
            InitializeComponent ();
        }

        private void WindowLoaded (object sender, RoutedEventArgs e)
        {
            Panel panel = UIHelper.GetVisualChild <Panel>(this, "canvas1");

            ListView listView = new ListView ()
            {
                Width = 200,
                Margin = new Thickness(10)
            };

            Button button = new Button()
            {
                Height = 40,
                Width = 100,
                Margin = new Thickness (220,10,10,10)
            };

            listView.Items.Add (new ListViewItem ().Content = "Item1");
            
            panel.Children.Add (listView);

            panel.Children.Add (button);
        }

        private void WindowDrag (object sender, MouseButtonEventArgs e)
        {
            this.DragMove ();
        }
    }
}