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
    public static class UIHelper
    {
        //
        // PARENT
        //
        public static T GetVisualParent <T> (DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);     // Try to get parent of child

            if (parentObject == null)                                               // If parent is null
                return null;                                                        // return parent as null

            T parent = (T)parentObject;                                            // check if the parents type matches the type we’re looking for

            if (parent != null)                                                     // If parent is not null
                return parent;                                                      // Successful return of the parent
            else                                                                    // If parent is null
                return GetVisualParent<T>(parentObject);                         // use recursion to proceed with next level
        }

        //
        // CHILD
        //
        public static T GetVisualChild <T> (DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)                                                     // If parent is null
                return null;                                                        // Return child as null

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = GetVisualChild<T>(child, childName);                    // recursively drill down the tree

                    if (foundChild != null)                                                 // If the child is found, break so we do not overwrite the found child. 
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = (FrameworkElement)child;                        // as FrameworkElement;

                    if (frameworkElement != null && frameworkElement.Name == childName)     // If the child's name is set for search
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;

                        break;
                    }
                }
                else
                {
                    foundChild = (T)child; // child element found.

                    break;
                }
            }

            return foundChild;
        }
    }
}