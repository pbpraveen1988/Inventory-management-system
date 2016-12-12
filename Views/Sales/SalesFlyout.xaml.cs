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
using System.Windows.Shapes;

namespace Inventory.Views.Sales
{
    /// <summary>
    /// Interaction logic for SalesFlyout.xaml
    /// </summary>
    public partial class SalesFlyout : Flyout
    {
        public SalesFlyout()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(26);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(5);
        }
    }
}
