using Inventory.Model;
using Inventory.Views.Sales;
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
using MahApps.Metro.Controls.Dialogs;
using DatabaseAndQueries;
using log4net;

namespace Inventory.Views.Filter
{
    /// <summary>
    /// Interaction logic for FindCustomer.xaml
    /// </summary>
    public partial class FindCustomer : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public FindCustomer()
        {
            Log.Info("Before InitializeComponent FindCustomer");
            InitializeComponent();
            Log.Info("After InitializeComponent FindCustomer");
                 
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Clicking to Find Customer Record in FindCustomer");
            if (!string.IsNullOrEmpty(showCustomer.Text)
            && !string.IsNullOrEmpty(FirstDate.Text) && !string.IsNullOrEmpty(SecondDate.Text))
            {   
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                ShowRecord flyout = mainWindow.Flyouts.Items[16] as ShowRecord;

                Log.Info("Before: Set to Datagrid Customer Record in FindCustomer");  
                    flyout.datalist.ItemsSource = Queries.GetAllByCondition<Sale>(x => x.Customer.Cid == (int)showCustomer.SelectedValue
                    && (x.SaleDate >= FirstDate.SelectedDate) && (x.SaleDate <= SecondDate.SelectedDate));

                flyout.cname.Content = showCustomer.Text ;
                flyout.fdate.Content = FirstDate.SelectedDate.Value.ToString("dd-MMM-yyyy"); 
                flyout.ldate.Content = SecondDate.SelectedDate.Value.ToString("dd-MMM-yyyy");
                Log.Info("After: Set to Datagrid Customer Record in FindCustomer");
               
                //  Type == true  for sales.

                //lst = (List<Products>)showProduct.ItemsSource;

                //Products _chk = (from l in lst
                //                 where l.Pid == (int)showProduct.SelectedValue
                //                 select l).FirstOrDefault<Products>();
                Log.Info("Before: Get list of customer in FindCustomer");
              
                List<Sale> _lst = Queries.GetAllByCondition<Sale>(x => x.Customer.Cid == (int)showCustomer.SelectedValue);
                Log.Info("After: Get list of customer in FindCustomer");
            
                float b = 0, total = 0;
                foreach (Sale item in _lst)
                {
                    b += item.RemainingBalance;
                    total += item.TotalAmount;
                }

                flyout.advance.Content = total;
                flyout.balance.Content = b;       

                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(16);
                showCustomer.Text = string.Empty;
                FirstDate.Text = string.Empty;
                SecondDate.Text = string.Empty;

                await controller.CloseAsync();

                if (flyout == null)
                {
                    return;
                }
                Log.Info("After: Clicking to Find Customer Record");
            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Please Enter the Complete Details", "");
            }
        }

        private void findCustomerFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.findCustomerFlyout.IsOpen)
            {
                Log.Info("Before: to set data context in findCustomerFlyout_Loaded FindCustomer");
                this.DataContext = new CustomerViewModel();
                Log.Info("After: to set data context in findCustomerFlyout_Loaded FindCustomer");
            }
        }

        private void findCustomerFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.findCustomerFlyout.IsOpen)
            {
                Log.Info("Before: to set data context in findCustomerFlyout_IsOpenChanged FindCustomer");
                this.DataContext = new CustomerViewModel();
                Log.Info("After: to set data context in findCustomerFlyout_IsOpenChanged FindCustomer");
            }
            
        }
    }
}
