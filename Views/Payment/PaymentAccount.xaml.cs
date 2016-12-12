using Inventory.Model;
using log4net;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading.Tasks;

namespace Inventory.Views.Payment
{
    /// <summary>
    /// Interaction logic for PaymentAccount.xaml
    /// </summary>
    public partial class PaymentAccount : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public List<Customer> _customerlst;
        public List<Customer> _dealerlst;
        public List<Sale> _mlst; //_sales,_purchase,_lst;
        float totalrem = 0;
        public PaymentAccount()
        {
            InitializeComponent();
        }
        private void Payment_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            Paytxt.Text = string.Empty;
            Paymodetxt.Text = string.Empty;
            Log.Info("Before: To set data context in ItemsalesAdd_IsOpenChanged ItemSalesAdd");
            if (this.Payment.IsOpen)
            {
                SalesViewModel _s = new SalesViewModel();
                this.DataContext = _s;            
                List<Sale> _lst = _s.Purchase.ToList<Sale>();
                List<Sale> _lst1 = _s.Sales.ToList<Sale>();
                _mlst = _lst1.Concat(_lst).ToList<Sale>();
                _customerlst = _s.Customerlist.Where<Customer>(x => x.Type == 0 || x.Type == 2).ToList();
                _dealerlst = _s.Customerlist.Where<Customer>(x => x.Type == 1 || x.Type == 2).ToList();
                if (Recievedpicker.IsChecked == true)
                {
                    _lst = _s.Purchase.ToList<Sale>();
                    showCustomer.ItemsSource = _customerlst;
                }
            }
            else 
            {
                Recievedpicker.IsChecked = true;
                showCustomer.ItemsSource = _customerlst;
            }
        }

        private void Recievedpicker_Checked(object sender, RoutedEventArgs e)
        {
            Paytxt.Text = string.Empty; 
            Paymodetxt.Text = string.Empty;
            if (Recievedpicker.IsChecked == true)
            {
                showCustomer.ItemsSource = _customerlst;
            }
        }

        private void Paidpicker_Checked(object sender, RoutedEventArgs e)
        {
            Paytxt.Text = string.Empty;
            Paymodetxt.Text = string.Empty;
            if (Paidpicker.IsChecked == true)
            {              
                showCustomer.ItemsSource = _dealerlst;
            }
        }

        private void showCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (sender as ComboBox).SelectedItem as Customer;
            Remainingtxt.Text = string.Empty;
            
            if (p != null && _mlst.Count > 0)
            {
                List<Sale> _lst = _mlst.Where<Sale>(x => x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid).ToList<Sale>();
                cid.Text = p.Cid.ToString();

                List<Account> _acclst = DatabaseAndQueries.Queries.GetAllByCondition<Account>
                    (x => x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid);           
                
                Fox.Text = "Company - " + p.Company + ", Address - " + p.Address + ", Contact No - " + p.ContactNo;
                foreach (var item in _lst)
                {

                    var remain = item.RemainingBalance;
                    totalrem = totalrem + remain;
                 //   Remainingtxt.Text = totalrem.ToString();
                }
                if (Paidpicker.IsChecked == true)
                {
                    _acclst = _acclst.Where<Account>(x => x.Pay_or_Recieve == true).ToList<Account>();
                }
                else
                {
                    _acclst = _acclst.Where<Account>(x => x.Pay_or_Recieve == false).ToList<Account>();
                }
                if (_acclst.Count > 0)
                {
                    foreach (var accitem in _acclst)
                    {
                        float totalpayment = 0;
                        var pay = accitem.PayAmount;
                        totalpayment = totalpayment + pay;
                        totalrem = totalrem - totalpayment;                      
                    }
                }
                Remainingtxt.Text = totalrem.ToString();
                totalrem = 0;
                Paytxt.Text = string.Empty;
                Paymodetxt.Text = string.Empty;
            }
            else
            {
                Fox.Text = cid.Text = string.Empty;
            }         
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Process to Account");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(Paytxt.Text) && !string.IsNullOrEmpty(Paymodetxt.Text))
            {

                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Account _a = new Account();              
                _a.Customer.Cid = (int)showCustomer.SelectedValue;              
                _a.PayMode = Paymodetxt.Text;
                _a.PayAmount = float.Parse(Paytxt.Text);
                _a.Pay_or_Recieve = false;               // False,0   for Recieve
                if (Paidpicker.IsChecked == true)
                {
                    _a.PayAmount = -float.Parse(Paytxt.Text);
                    _a.Pay_or_Recieve = true;           // True,1  for Pay
                }
               
                DatabaseAndQueries.Queries.Add<Account>(_a);

                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Sales Record Inserted Successfully", "");
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                Log.Info("After:Add Account in DB");
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please Fill all Information", "");              
            }

        }

        private void Paytxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Paytxt.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                Paytxt.Text = string.Empty;
            }
        }

    }
}
