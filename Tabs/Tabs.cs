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
    public class Tabs
    {
        public List <TabItem> tabsList = new List <TabItem> ();

        public Tabs ()                                              // CLASS CONSTRUCTOR, Runs whenever this class is instanciated
        {
            tabsList.Add (TabClasses ());
            tabsList.Add (TabItems ());
        }

        public TabItem TabClasses ()
        {
            TabItem tabItem = new TabItem ();

            tabItem.Header = "Classes";

            Canvas canvas = new Canvas ();

            tabItem.Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            return tabItem;
        }

        public TabItem TabItems ()
        {
            TabItem tabItem = new TabItem ();

            tabItem.Header = "Items";

            Canvas canvas = new Canvas ();

            tabItem.Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            return tabItem;
        }
    }
}














// ALTERNATE WAY OFF DOING THINGS
// WORSE THOUGH, I THINK

/*
namespace GoldenSunEditor
{
    public class TabClasses : TabItem
    {
        public TabClasses ()
        {
            Header = "Classes";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }

    public class TabItems : TabItem
    {
        public TabItems ()
        {
            Header = "Items";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }

    public class TabForge : TabItem
    {
        public TabForge ()
        {
            Header = "Forge";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }

    public class TabElemental : TabItem
    {
        public TabElemental ()
        {
            Header = "Elemental";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }

    public class TabShops : TabItem
    {
        public TabShops ()
        {
            Header = "Shops";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }
}
*/