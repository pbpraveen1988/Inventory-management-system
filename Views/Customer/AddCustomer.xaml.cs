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
using MahApps.Metro.Controls.Dialogs;
using log4net;
namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public AddCustomer()
        {
            Log.Info("Before InitializeComponent Add Customer");

            InitializeComponent();
            Log.Info("After InitializeComponent Add Customer");
            //  this.DataContext = DatabaseAndQueries.Queries.GetDataByCondition<Customer>(x => x.Cid == 2);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before Add Customer");

            if (!string.IsNullOrEmpty(CEmail.Text) && !string.IsNullOrEmpty(CAddress.Text)
                && !string.IsNullOrEmpty(ContactNumar.Text) && !string.IsNullOrEmpty(Cname.Text))
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Customer _c = new Customer();
                _c.Address = CAddress.Text;
                _c.Cname = Cname.Text;
             //   _c.ContactNo = Convert.ToInt64(ContactNumar.Text);
                _c.Email = CEmail.Text;
                _c.Date = DateTime.Now;
                DatabaseAndQueries.Queries.Add<Customer>(_c);              
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                Log.Info("After Add Customer");

                await mainWindow.ShowMessageAsync("Customer Record Inserted Successfully", "");               
                CAddress.Text = string.Empty;
                Cname.Text = string.Empty;
                CEmail.Text = string.Empty;
                ContactNumar.Text = string.Empty;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);             
            }
            else
            {
                Log.Info("Before Add Customer to fill all Detail");
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Please Enter the Complete Details", "");
            }

        }

        private void addcustomerFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.addcustomerFlyout.IsOpen)
            {
                DataContext = new CustomerViewModel();
            }
         
        }

        private void addcustomerFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.addcustomerFlyout.IsOpen)
            {
                this.DataContext = new CustomerViewModel();
            }
            ContactNumar.Text = string.Empty;
        }
        private void Confirm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }
        private int _errors = 0;

        private void Confirm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            e.Handled = true;
           
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }

        private void ContactNumar_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(ContactNumar.Text, "^[0-9]*$"))
            {
              ContactNumar.Text = string.Empty;
            }          
        }

        private void Cname_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Cname.Text, "^[a-zA-Z ]+$"))
            {                
                Cname.Text = string.Empty;
            }
        }
            
    
        private async void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            if (!string.IsNullOrEmpty(CAddress.Text) && !string.IsNullOrEmpty(Cname.Text)
                && !string.IsNullOrEmpty(ContactNumar.Text))
            {

                if (string.IsNullOrEmpty(CEmail.Text))
                {
                    CEmail.Text = "not available";
                }
                Log.Info("Before: going to add new customer record");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Log.Info("Before: Check customer contact no exist or not ");
                if (!DatabaseAndQueries.Queries.IsExists<Customer>(x => x.ContactNo == (ContactNumar.Text)))
                {
                    Log.Info("After: Check customer contact no exist or not, successfully ");
                    Log.Info("Before: Going to Add customer record ");
                    Customer _customer = new Customer();
                    _customer.Cname = Cname.Text;
                    if (string.IsNullOrEmpty(Company.Text))
                    {
                        _customer.Company = "Not Available";
                    }
                    _customer.Company = Company.Text;
                    _customer.ContactNo = ContactNumar.Text;
                    _customer.Address = CAddress.Text;
                    _customer.Email = CEmail.Text;
                    if (customerchk.IsChecked == true && dealerchk.IsChecked == false)
                    {
                        _customer.Type = 0;    // Customer
                    }
                    if (dealerchk.IsChecked == true && customerchk.IsChecked == false)
                    {
                        _customer.Type = 1;      // Dealer
                    }
                    else if (customerchk.IsChecked == true && dealerchk.IsChecked == true)
                    {
                        _customer.Type = 2;      //  Both
                    }
                    DatabaseAndQueries.Queries.Add<Customer>(_customer);
                    Log.Info("After: to add new customer record, Successfully");
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Customer Record Added Successfully", "");
                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                }
                else
                {
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Customer Contact number already exist", "");
                }
            }
            else
            {
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
            }
        }

    }
}
