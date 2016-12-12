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
using MahApps.Metro.Controls.Dialogs;
using Inventory.Views.Sales;
using DatabaseAndQueries;
using log4net;

namespace Inventory.Views.Purchase
{
    /// <summary>
    /// Interaction logic for AddPurchase.xaml
    /// </summary>
    public partial class AddPurchase : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public AddPurchase()
        {
            InitializeComponent();        
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Process to Add Purchase");

            if (!string.IsNullOrEmpty(Price.Text) && !string.IsNullOrEmpty(Qty.Text)
            && !string.IsNullOrEmpty(Advanced.Text))
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Sale _s = new Sale();
                _s.Product.Pid = (int)showProduct.SelectedValue;
                _s.Customer.Cid = (int)showCustomer.SelectedValue;
                _s.Advance = float.Parse(Advanced.Text);
                _s.Price = float.Parse(Price.Text);
                _s.Quantity = float.Parse(Qty.Text);            
                _s.TotalAmount = _s.Price * _s.Quantity;
                TotalAmount.Text = _s.TotalAmount.ToString();
                _s.RemainingBalance = _s.TotalAmount - _s.Advance;
                _s.Type = false;


                Products _chk = (from l in (List<Products>)showProduct.ItemsSource
                                 where l.Pid == (int)showProduct.SelectedValue
                                 select l).FirstOrDefault<Products>();

                //    Products _chk = Queries.GetDataByCondition<Products>(x => x.Pid == ((int)showProduct.SelectedValue));                
                _chk.StockQty = _chk.StockQty + float.Parse(Qty.Text);
                Queries.Update<Products>(_chk);
                Log.Info("Before:Add Purchase in DB");

                DatabaseAndQueries.Queries.Add<Sale>(_s);
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                Log.Info("After:Add Purchase in DB");

                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                var flyout = mainWindow.Flyouts.Items[10] as ShowPurchase;
                if (flyout == null)
                {
                    return;
                }
                flyout.datalist.ItemsSource = DatabaseAndQueries.Queries.GetAllByCondition<Sale>(x => x.Type == false);
                Price.Text = string.Empty;
                Advanced.Text = string.Empty;
                TotalAmount.Text = string.Empty;
                Qty.Text = string.Empty;
                await mainWindow.ShowMessageAsync("Sales Record Inserted Successfully", "");
                Log.Info("After: Process to Add Purchase");

            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Please Enter the Complete Details", "");
            }


        }

        private async void Qty_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Price.Text) && !string.IsNullOrEmpty(Qty.Text))
            {
                Log.Info("Before: Process to calculate total amount in Qty_LostFocus Add Purchase");
                float price, Quantity;
                price = float.Parse(Price.Text);
                Quantity = float.Parse(Qty.Text);
                TotalAmount.Text = Convert.ToString(price * Quantity);
                Log.Info("Before: Process to calculate total amount in Qty_LostFocus Add Purchase");
            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (!string.IsNullOrEmpty(Price.Text))
                {                  
                    await mainWindow.ShowMessageAsync("Please Enter Qunatity", "");
                    Qty.Focus();
                }
                else {                   
                    await mainWindow.ShowMessageAsync("Please Enter Unitprice", "");
                    Price.Focus();
                }             
              //  MessageBox.Show("Please enter Unitprice and Qunatity");
            }
        }
        private void showCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (sender as ComboBox).SelectedItem as Customer;
            if (p != null)
            {
                cid.Text = p.Cid.ToString();
            }
            else
            {
                cid.Text = string.Empty;
            }

        }
        private void showProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (sender as ComboBox).SelectedItem as Products;
            if (p != null)
            {
                pid.Text = p.Pid.ToString();
            }
            else
            {
                pid.Text = string.Empty;
            }
        }

        private void addPurchaseFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.addPurchaseFlyout.IsOpen)
            {
                this.DataContext = new SalesViewModel();
            }          
        }

        private void addPurchaseFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.addPurchaseFlyout.IsOpen)
            {
                this.DataContext = new SalesViewModel();
            }
            Price.Text = string.Empty;
            Qty.Text = string.Empty;
            Advanced.Text = string.Empty;
            TotalAmount.Text = string.Empty;
        }

        private void Price_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Price.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                Price.Text = string.Empty;
            }
        }

        private void Qty_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Qty.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                Qty.Text = string.Empty;
            }
        }

        private void Advanced_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Advanced.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                Advanced.Text = string.Empty;
            }
        }

        private void Price_LostFocus(object sender, RoutedEventArgs e)
        {
           
                //  MessageBox.Show("Please enter Unitprice and Qunatity");
          
        }     

    }
}
