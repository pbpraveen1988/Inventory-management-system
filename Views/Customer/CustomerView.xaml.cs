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
using log4net;
namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
   
    public partial class CustomerView : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        public CustomerView()
        {
            Log.Info("Before InitializeComponent Customer main Flyout");
            InitializeComponent();
            Log.Info("After InitializeComponent Customer main Flyout");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before :Click on Add Customer");      
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var item = mainWindow.Flyouts;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(1);
            Log.Info("After :Click on Add Customer");  
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Click to Show All Customer");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(2);
            Log.Info("After :Click to Show All Customer");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Click to Edit Customer Record");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(12);
            Log.Info("After: Click to Edit Customer Record");
        }            
    }
}
