using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MahApps.Metro.Controls.Dialogs;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ShowCustomer.xaml
    /// </summary>
    public partial class ShowCustomer : Flyout
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public ShowCustomer()
        {
            Log.Info("Before InitializeComponent ShowCustomer");
            InitializeComponent();
            Log.Info("After InitializeComponent ShowCustomer");
        }

        private void showCustomerFlyout_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void showCustomerFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.showCustomerFlyout.IsOpen)
            {
                Log.Info("Before: To set data context in showCustomerFlyout_IsOpenChanged ShowCustomer");
                this.DataContext = new CustomerViewModel();
                Log.Info("After: To set data context in showCustomerFlyout_IsOpenChanged ShowCustomer");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datalist.Items.Count != 0)
            {
                AllCustomer rpt = new AllCustomer();
             //   rpt._lst = (List<Customer>)datalist.ItemsSource;  
                rpt.ShowDialog();
            }
            else {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Data not available to print", "");     
            }
        }

        private void Customer_Name_KeyUp(object sender, KeyEventArgs e)
        {
            Log.Info("Before: Sort customer datalist according to search text box in Customer_Name_KeyUp ShowCustomer");
            IEnumerable<Customer> _customerlst = (IEnumerable<Customer>)datalist.ItemsSource;
            datalist.ItemsSource = from cust in _customerlst
                                   where (cust.Cname.ToLower().StartsWith(Customer_Name.Text.ToLower())
                                   || cust.Email.ToLower().StartsWith(Customer_Name.Text.ToLower()))
                                   select cust;
            Log.Info("After: Sort customer datalist according to search text box in Customer_Name_KeyUp ShowCustomer, Successful");
        }
    }
}
