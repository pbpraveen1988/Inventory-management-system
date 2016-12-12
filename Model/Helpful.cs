using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
namespace Inventory.Model
{
   public class Helpful
    {

       public static void CloseAllWindows()
       {
           for (int intCounter = App.Current.Windows.Count - 2; intCounter >= 0; intCounter--)
               App.Current.Windows[intCounter].Close();
       }

       public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
       {
           if (depObj != null)
           {
               for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
               {
                   DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                   if (child != null && child is T)
                   {
                       yield return (T)child;
                   }

                   foreach (T childOfChild in FindVisualChildren<T>(child))
                   {
                       yield return childOfChild;
                   }
               }
           }
       }

       public static void ClearAll<T,K>(DependencyObject objectC,K newobjectc)
       {
           //foreach (K tb in FindVisualChildren<K>(objectC))
           //{
           //    // do something with tb here
           //}
       }


       public static void CloseAllFlyouts(FlyoutsControl _list)
       {
           foreach (Flyout item in _list.Items)
           {
               item.IsOpen = false;
           }
       }
       public class BoolToValueConverter<T> : IValueConverter
       {
           public T FalseValue { get; set; }
           public T TrueValue { get; set; }

           public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
           {
               if (value == null)
                   return FalseValue;
               else
                   return (bool)value ? TrueValue : FalseValue;
           }

           public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
           {
               return value != null ? value.Equals(TrueValue) : false;
           }
       }

    

       
    }
}
