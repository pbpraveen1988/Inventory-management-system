using Inventory.Model;
using MahApps.Metro.Controls;
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

namespace Inventory.Views.Filter
{
    /// <summary>
    /// Interaction logic for ShowFilter.xaml
    /// </summary>
    public partial class ShowFilter : Flyout
    {
        public ShowFilter()
        {
            InitializeComponent();
        }
        
       private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var item = mainWindow.Flyouts;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(14);
        }

       private void Button_Click_1(object sender, RoutedEventArgs e)
       {
           MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
           var item = mainWindow.Flyouts;
           Helpful.CloseAllFlyouts(mainWindow.Flyouts);
           mainWindow.ToggleFlyout(15);


           //FindCustomer flyout = mainWindow.Flyouts.Items[14] as FindCustomer;
           //flyout.Header = "FIND PRODUCT";
           //flyout.showCustomer.ItemsSource = DatabaseAndQueries.Queries.GetAllData<Products>();
           //flyout.showCustomer.DisplayMemberPath = "Pname";
           //flyout.showCustomer.SelectedValuePath = "Pid";

       }
    }
}
