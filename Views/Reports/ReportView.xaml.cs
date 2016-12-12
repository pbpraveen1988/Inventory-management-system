using DatabaseAndQueries;
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
using Inventory.Views.Filter;
using log4net;

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public List<Sale> lst;
        public List<Products> prodlst;
        public List<Sale> lstcustomer;
        public ReportView()
        {
            InitializeComponent();
        }

        private void showReportFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.showReportFlyout.IsOpen)
            {
                SalesViewModel _s = new SalesViewModel();
                this.DataContext = _s; 
                List<Customer> sb =  _s.Customerlist.ToList<Customer>();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            GridResult flyout = mainWindow.Flyouts.Items[21] as GridResult;
            Log.Info(": Find record in filter module goto page_ReportView");
            Log.Info("Before: get list of all sales record in ReportView");
            lstcustomer = Queries.GetAllData<Sale>();
            Log.Info("After: get list of all sales record in ReportView");
            if (enabledcustomerSwitch.IsChecked == true && enabledproductSwitch.IsChecked == true)
            {
                if (showCustomer.SelectedItem != null && showProduct.SelectedItem != null)
                {
                  
                //    float rem = lstcustomer.Sum(x => x.RemainingBalance);
                    if (DayDtpicker.IsChecked == true && DayPicker.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Customer,product on selected date in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                             && x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                             && x.SaleDate.Date == DayPicker.SelectedDate.Value.Date).ToList<Sale>();                                                       
                                                     

                   //     flyout.datalist.ItemsSource = lst;
                        flyout.SendDate = DayPicker.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + showProduct.Text;


                        flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Records");
                        flyout.balance.Content = DayPicker.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.adv_Copy.SetResourceReference(Label.ContentProperty, "Customer");
                        flyout.advance.Content = showCustomer.Text;

                        Log.Info("After: get list of Customer,product on selected date in ReportView");
                        //         goto report having all sales detail customer with product on selected date
                    }
                    else if (BetweenDtpicker.IsChecked == true && StartDate.SelectedDate.Value != null
                        && EndDate.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Customer,product B/w selected dates in ReportView");
                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                          && x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                               && x.SaleDate.Date >= StartDate.SelectedDate.Value.Date 
                               && x.SaleDate.Date <= EndDate.SelectedDate.Value.Date).ToList<Sale>();
                                        
                        flyout.SendDate = StartDate.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + EndDate.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.Name = showCustomer.Text + "$" + showProduct.Text;

                        flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Records From :");
                        flyout.balance.Content = StartDate.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.adv_Copy.Content = "To:";
                        flyout.advance.Content = EndDate.SelectedDate.Value.ToString("dd-MM-yyyy");

                        Log.Info("After: get list of Customer,product B/w selected dates in ReportView");
                        //       goto report having all sales detail customer with product on B/W date  
                    }
                    else if (MonthDtpicker.IsChecked == true && MonthPicker.SelectedDate.Value != null)
                    {
                        //         goto monthly report having all sales detail customer with product 
                        Log.Info("Before: get list of Customer,product of selected month/year in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                             && x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                                 && x.SaleDate.Month == MonthPicker.SelectedDate.Value.Month
                               && x.SaleDate.Year == MonthPicker.SelectedDate.Value.Year).ToList<Sale>();
                    
                  //      flyout.datalist.ItemsSource = lst;

                        flyout.SendDate = MonthPicker.SelectedDate.Value.Month + "/" + MonthPicker.SelectedDate.Value.Year + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + showProduct.Text;

                        flyout.bal_Copy.Content = "Record of:";
                        flyout.balance.Content = showCustomer.Text + " with product :" + showProduct.Text;
                        flyout.adv_Copy.Content = " Within";
                        flyout.advance.Content = MonthPicker.SelectedDate.Value.ToString("MMM-yyyy");

                        Log.Info("After: get list of Customer,product of selected month/year in ReportView");
                    }
                    else
                    {
                        Log.Info("Before: get all record list of Customer,product in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid 
                                 && x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid).ToList<Sale>();
                            
                        
                                            
                     //   flyout.datalist.ItemsSource = lst;
                        flyout.SendDate = "0" + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + showProduct.Text;

                        flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Customer");
                        flyout.balance.Content = showCustomer.Text;
                        flyout.adv_Copy.SetResourceReference(Label.ContentProperty, "Product");
                        flyout.advance.Content = showProduct.Text;

                        Log.Info("After: get all record list of Customer,product in ReportView");
                        //         goto report having all sales detail customer with product
                    }
                    flyout.datalist.ItemsSource = lst;
                    flyout.lstsale = lst;
                   
                    flyout.stockqty.Visibility = Visibility.Hidden;
                    flyout.stock.Visibility = Visibility.Hidden;

                    flyout.advance.Visibility = Visibility.Visible;
                    flyout.balance.Visibility = Visibility.Visible;
                    flyout.adv_Copy.Visibility = Visibility.Visible;
                    flyout.bal_Copy.Visibility = Visibility.Visible;

                    if (lst.Count != 0)
                    {
                        Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                        mainWindow.ToggleFlyout(21);
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync(" Record not found", "");
                    }
                }
                else
                {
                    if (showCustomer.SelectedItem == null && showProduct.SelectedItem != null)
                    {
                        await mainWindow.ShowMessageAsync("Please select customer from dropdown list", "");
                        showCustomer.Focus();
                    }
                    else if (showCustomer.SelectedItem != null && showProduct.SelectedItem == null)
                    {
                        await mainWindow.ShowMessageAsync("Please select product from dropdown list", "");
                        showProduct.Focus();
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync("Please select customer and product first", "");
                        showCustomer.Focus();
                    }
                }
            }
            else if (enabledcustomerSwitch.IsChecked == true)
            {
                if (showCustomer.SelectedItem != null)
                {
                    if (DayDtpicker.IsChecked == true && DayPicker.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Customer on selected date in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                               && x.SaleDate.Date == DayPicker.SelectedDate.Value.Date).ToList<Sale>();

                     
                        flyout.SendDate = DayPicker.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + "0";
                        Log.Info("After: get list of Customer on selected date in ReportView");
                        //         goto report having all sales detail customer on selected date
                    }
                    else if (BetweenDtpicker.IsChecked == true && StartDate.SelectedDate.Value != null
                       && EndDate.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Customer B/w selected dates in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                          &&(x.SaleDate.Date >= StartDate.SelectedDate.Value.Date) 
                          && (x.SaleDate.Date <= EndDate.SelectedDate.Value.Date)).ToList<Sale>();
                        
                        flyout.SendDate = StartDate.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + EndDate.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.Name = showCustomer.Text + "$" + "0";

                        Log.Info("After: get list of Customer B/w selected dates in ReportView");
                        //       goto report having all sales detail customer on B/W date  
                    }
                   else if (MonthDtpicker.IsChecked == true && MonthPicker.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Customer of selected month/year in ReportView");
                        lst = lstcustomer.Where<Sale>(x => x.Customer.Cid == ((Customer)showCustomer.SelectedItem).Cid
                              && x.SaleDate.Month == MonthPicker.SelectedDate.Value.Month
                              && x.SaleDate.Year == MonthPicker.SelectedDate.Value.Year).ToList<Sale>();
                                              
                
                        flyout.SendDate = MonthPicker.SelectedDate.Value.Month + "/" + MonthPicker.SelectedDate.Value.Year + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + "0";
                        Log.Info("After: get list of Customer of selected month/year in ReportView");
                        //         goto monthly report having all sales detail customer
                    }
                    else
                    {
                        Log.Info("Before: get all record list of Customer in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Customer.Cid ==
                            ((Customer)showCustomer.SelectedItem).Cid).ToList<Sale>();

                        flyout.SendDate = "0" + "$" + "0";
                        flyout.Name = showCustomer.Text + "$" + "0";
                        Log.Info("After: get all record list of Customer in ReportView");
                        //         goto report having all sales detail customer
                    }
                    flyout.lstsale = lst;
                    flyout.datalist.ItemsSource = lst;

                    flyout.stockqty.Content = showCustomer.Text;
                    flyout.stock.SetResourceReference(Label.ContentProperty, "Customer");

                    flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Remaining Balance :");
                    flyout.adv_Copy.SetResourceReference(Label.ContentProperty, "Total :");
                    Log.Info("Before: get all record list of Customer in ReportView in ReportView");
                    List<Sale> _salelst = Queries.GetAllByCondition<Sale>(x => x.Customer.Cid == (int)showCustomer.SelectedValue);
                    float remain = 0, total = 0;
                    Log.Info("After: get all record list of Customer in ReportView in ReportView");
                    float acccount = 0;
                    foreach (Sale item in _salelst)
                    {
                        acccount = item.Customer.Account.Where(x => x.Customer.Cid == item.Customer.Cid).Sum(x => x.PayAmount);                      
                        remain += item.RemainingBalance;
                        total += item.TotalAmount;
                    }
                    flyout.advance.Visibility = Visibility.Visible;
                    flyout.balance.Visibility = Visibility.Visible;
                    flyout.adv_Copy.Visibility = Visibility.Visible;
                    flyout.bal_Copy.Visibility = Visibility.Visible;
                    flyout.advance.Content = total;
                    flyout.balance.Content = " " + (remain - acccount).ToString();
                  
                    if (lst.Count != 0)
                    {
                        Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                        mainWindow.ToggleFlyout(21);
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync("Record not found", "");
                    }
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please select customer from dropdown list", "");
                    showCustomer.Focus();
                }
            }
            else if (enabledproductSwitch.IsChecked == true)
            {
                if (showProduct.SelectedItem != null)
                {
                    if (DayDtpicker.IsChecked == true && DayPicker.SelectedDate.Value != null )
                    {
                        Log.Info("Before: get list of Product on selected dates in ReportView");
                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                            && x.SaleDate.Date == DayPicker.SelectedDate.Value.Date).ToList<Sale>();
                                             
                //        flyout.datalist.ItemsSource = lst;

                        flyout.SendDate = DayPicker.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + "0";
                        flyout.Name = "0" + "$" + showProduct.Text;

                        flyout.bal_Copy.Content = string.Empty;
                        flyout.balance.Content = string.Empty;
                        flyout.adv_Copy.Content = string.Empty;
                        flyout.advance.Content = string.Empty;
                        Log.Info("After: get list of Product on selected dates in ReportView");
                        //         goto report having all sales detail product on selected date
                    }
                    else if (BetweenDtpicker.IsChecked == true && StartDate.SelectedDate.Value != null
                        && EndDate.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of Product B/w selected dates in ReportView");
                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                        && x.SaleDate.Date >= StartDate.SelectedDate.Value.Date
                        && x.SaleDate.Date <= EndDate.SelectedDate.Value.Date).ToList<Sale>();
                    

                        //lst = Queries.GetAllByCondition<Sale>(
                        //   x => x.Product == showProduct.SelectedItem &&
                        //       (x.SaleDate >= StartDate.SelectedDate) && (x.SaleDate <= EndDate.SelectedDate));
                  //      flyout.datalist.ItemsSource = lst;

                        flyout.SendDate = StartDate.SelectedDate.Value.ToString("dd-MM-yyyy") + "$" + EndDate.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.Name = "0" + "$" + showProduct.Text;

                        flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Records From :");
                        flyout.balance.Content = StartDate.SelectedDate.Value.ToString("dd-MM-yyyy");
                        flyout.adv_Copy.SetResourceReference(Label.ContentProperty, "To:");
                        flyout.advance.Content = EndDate.SelectedDate.Value.ToString("dd-MM-yyyy");

                        Log.Info("After: get list of Product B/w selected dates in ReportView");
                        //       goto report having all sales detail product on B/W date  
                    }
                    else if (MonthDtpicker.IsChecked == true && MonthPicker.SelectedDate.Value != null)
                    {
                        Log.Info("Before: get list of product of selected month/year in ReportView");
                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid == ((Products)showProduct.SelectedItem).Pid
                        && x.SaleDate.Month == MonthPicker.SelectedDate.Value.Month
                        && x.SaleDate.Year == MonthPicker.SelectedDate.Value.Year).ToList<Sale>();
                                         
                  //       flyout.datalist.ItemsSource = lst;
                        flyout.SendDate = MonthPicker.SelectedDate.Value.Month + "/" + MonthPicker.SelectedDate.Value.Year + "$" + "0";
                        flyout.Name = "0" + "$" + showProduct.Text;

                        flyout.bal_Copy.SetResourceReference(Label.ContentProperty, "Record_of");
                        flyout.balance.Content = MonthPicker.SelectedDate.Value.ToString("MMM-yyyy");
                        flyout.adv_Copy.Content = string.Empty;
                        flyout.advance.Content = string.Empty;
                        Log.Info("After: get list of product of selected month/year in ReportView");
                        //         goto monthly report having all sales detail product 
                    }
                    else
                    {
                        Log.Info("Before: get all record list of Product in ReportView");

                        lst = lstcustomer.Where<Sale>(x => x.Product.Pid ==
                            ((Products)showProduct.SelectedItem).Pid).ToList<Sale>();         

                        flyout.SendDate = "0" + "$" + "0";
                        flyout.Name = "0" + "$" + showProduct.Text;

                        flyout.bal_Copy.Content = string.Empty;
                        flyout.balance.Content = string.Empty;
                        flyout.adv_Copy.Content = string.Empty;
                        flyout.advance.Content = string.Empty;
                        Log.Info("After: get all record list of Product in ReportView");
                        //         goto report having all sales detail product
                    }
                    flyout.lstsale = lst;
                    flyout.datalist.ItemsSource = lst;
                    IEnumerable<Products> prodlst = (IEnumerable<Products>)showProduct.ItemsSource;
                    Products _chk = (from l in prodlst
                                     where l.Pid == (int)showProduct.SelectedValue
                                     select l).FirstOrDefault<Products>();
                    flyout.stockqty.Content = _chk.StockQty;
                    flyout.stock.Content = showProduct.Text + " " + "In_Stock :";

                    flyout.advance.Visibility = Visibility.Visible;
                    flyout.balance.Visibility = Visibility.Visible;
                    flyout.adv_Copy.Visibility = Visibility.Visible;
                    flyout.bal_Copy.Visibility = Visibility.Visible;
                    if (lst.Count != 0)
                    {
                        Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                        mainWindow.ToggleFlyout(21);
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync(" Record not found", "");
                    }
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please select Product from dropdown list", "");
                    showProduct.Focus();
                }
            }
            else if (enabledBillnoSwitch.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(billnotxt.Text))
                {
                    if (showSales.SelectedValue.ToString().Contains("SALES"))
                    {
                        if (Queries.IsExists<Sale>(x => x.Billing == Convert.ToInt32(billnotxt.Text)))
                        {
                            Log.Info("Before: generate bill call from ReportView");
                            Log.Info("Request to salesbill for report");
                            SalesBill rpt = new SalesBill(Convert.ToInt32(billnotxt.Text));
                            rpt.ShowDialog();
                            Log.Info("After: generate bill call, from ReportView");
                        }
                        else
                        {
                            await mainWindow.ShowMessageAsync("Bill not found, please enter valid Bill No", "");
                        }
                    }
                    else if (showSales.SelectedValue.ToString().Contains("PURCHASE"))
                    {
                        Log.Info("Before: generate bill call from ReportView");
                        Log.Info("Request to salesbill for report in ReportView");
                        PurchaseBill rpt = new PurchaseBill(billnotxt.Text);
                        rpt.ShowDialog();
                        Log.Info("After: generate bill call, from ReportView");
                    }
                  
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please Enter Bill no. Search", "");
                }
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please select option to Search", "");
                // please select first what do u want to know...... ? 
            }
        }

        private async void billnotxt_KeyUp(object sender, KeyEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (showSales.SelectedValue != null)
            {
                if (showSales.SelectedValue.ToString().Contains("SALES"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(billnotxt.Text, "^[0-9]*$"))
                    {
                        billnotxt.Text = string.Empty;
                    }
                }
                else if (showSales.SelectedValue.ToString().Contains("PURCHASE"))
                {
                } 
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please select from Combobox", "");
                showSales.Focus();
            }
        }

        private void enabledBillnoSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (enabledBillnoSwitch.IsChecked == true)
            {
                enabledcustomerSwitch.IsChecked = false;
                enabledproductSwitch.IsChecked = false;
                enabledcustomerSwitch.IsEnabled = false;
                enabledproductSwitch.IsEnabled = false;
                selectdategrid.IsEnabled = false;
            }
            else
            {
                enabledcustomerSwitch.IsEnabled = true;
                enabledproductSwitch.IsEnabled = true;
                selectdategrid.IsEnabled = true;
            }
        }

        private void showSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            billnotxt.Text = string.Empty;
        }

        private void enabledproductSwitch_Click(object sender, RoutedEventArgs e)
        {           
           // enabledBillnoSwitch.IsChecked == false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void accountbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(28);
        }
    }

}
