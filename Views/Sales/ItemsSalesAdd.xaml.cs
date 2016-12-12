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
using System.Data;
using DatabaseAndQueries;
using System.Collections.ObjectModel;
using Inventory.Views.Reports;

namespace Inventory.Views.Sales
{
    /// <summary>
    /// Interaction logic for ItemsSalesAdd.xaml
    /// </summary>
  
    public partial class ItemsSalesAdd : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        List<Products> lst;
        public int Billno;
        int billnumber = 0;
        public float totalsum = 0;
        public ItemsSalesAdd()
        {
            InitializeComponent();

        }

        private void ItemsalesAdd_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            totalcaltxt.Text = string.Empty;
            Log.Info("Before: To set data context in ItemsalesAdd_IsOpenChanged ItemSalesAdd");
            if (this.ItemsalesAdd.IsOpen)
            {
                SalesViewModel _s = new SalesViewModel();
                this.DataContext = _s;
                lst = _s.Products.ToList<Products>();

                List<Customer> _customerlst = _s.Customerlist.Where<Customer>(x=>x.Type == 0 || x.Type == 2).ToList();
                showCustomer.ItemsSource = _customerlst; 

                Log.Info("Before: To get list of Billing in ItemsalesAdd_IsOpenChanged ItemSalesAdd");
                List<Billing> _blist = Queries.GetAllData<Billing>();
                Log.Info("After: To get list of Billing in ItemsalesAdd_IsOpenChanged ItemSalesAdd");
                if (_blist.Count == 0)
                {
                    billnumber = 1001;
                }
                else
                {
                    var bp = _blist.Last();
                    billnumber = bp.BillNo + 1;
                }
                Log.Info("After: To set data context in ItemsalesAdd_IsOpenChanged ItemSalesAdd");
            }
            billlbl.Content ="Bill No: "+ billnumber;
            showProduct.ItemsSource = lst;
            showCustomer.IsEnabled = true;
            datalist.Items.Clear();
            mrplbl.Content = string.Empty;
            vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
        }
        private void showCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (sender as ComboBox).SelectedItem as Customer;
            if (p != null)
            {
                cid.Text = p.Cid.ToString();
                CustomerDetailtxt.Text = "Company - " + p.Company + ", Address - " + p.Address + ", Contact No - " + p.ContactNo;  
                vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
            }
            else
            {
                CustomerDetailtxt.Text = string.Empty;              
                cid.Text = string.Empty;
            }

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

        private async void Qty_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(Qty.Text))
            {
                Products item = (Products)showProduct.SelectedItem;
                if (!(float.Parse(Qty.Text) <= item.StockQty))
                {
                    // Qty.Text = string.Empty;
                    await mainWindow.ShowMessageAsync("You have Insufficient Stock : " + item.StockQty, "");
                    TotalAmount.Text = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Price.Text))
                    {
                        if (!string.IsNullOrEmpty(discounttxt.Text))
                        {
                            if (!string.IsNullOrEmpty(vattxt.Text))
                            {
                                Log.Info("Before: Process to calculate total amount in Qty_LostFocus ItemSalesAdd");
                                float price, Quantity, _Discount, _Vat, total;
                                price = float.Parse(Price.Text);  //1500
                                Quantity = float.Parse(Qty.Text);  //3
                                total = price * Quantity;       //4500

                                _Discount = (total * float.Parse(discounttxt.Text)) / 100;     //90
                                _Discount = total - _Discount;   // amount after discount   4410
                                _Vat = (_Discount * float.Parse(vattxt.Text)) / 100;     //617.4

                                TotalAmount.Text = Convert.ToString(_Discount + _Vat);// 5027.4

                                Log.Info("After: Process to calculate total amount in Qty_LostFocus ItemSalesAdd");
                            }
                        }
                        else
                        {
                            Log.Info("Before: Process to calculate total amount in Qty_LostFocus ItemSalesAdd");
                            float price, Quantity, _Discount, _Vat, total;
                            float p = 0;
                            price = float.Parse(Price.Text);      //1500
                            Quantity = float.Parse(Qty.Text);     //3
                            total = price * Quantity;           //4500
                            _Discount = (total * (p)) / 100;     //90
                            _Discount = total - _Discount;   // amount after discount   4410
                            if (string.IsNullOrEmpty(vattxt.Text))
                            {
                                _Vat = (_Discount * (p)) / 100;
                            }
                            else
                            {
                                _Vat = (_Discount * float.Parse(vattxt.Text)) / 100;
                            }
                            TotalAmount.Text = Convert.ToString(_Discount + _Vat);// 5027.4

                            Log.Info("After: Process to calculate total amount in Qty_LostFocus ItemSalesAdd");
                        }
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync("Please Enter Unitprice", "");
                        Price.Focus();
                    }
                }
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please Enter Quantity", "");
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

        }
        private void showProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Info("Before: To get list of Billing showProduct_SelectionChanged ItemSalesAdd");
            var p = (sender as ComboBox).SelectedItem as Products;
            if (p != null)
            {
                pid.Text = p.Pid.ToString();
                mrplbl.Content = "MRP :" + p.Mrp.ToString();
                vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
            }
            else
            {
                pid.Text = string.Empty;
            }
            Log.Info("After: To get list of Billing showProduct_SelectionChanged ItemSalesAdd");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(TotalAmount.Text) && !string.IsNullOrEmpty(showProduct.Text)
                && float.Parse(Qty.Text) != 0)
            {
                Log.Info("Before: To Add item into datagrid ItemSalesAdd");
                float p = 0;
                if (string.IsNullOrEmpty(vattxt.Text))
                {
                    vattxt.Text = p.ToString();
                }
                if (string.IsNullOrEmpty(discounttxt.Text))
                {
                    discounttxt.Text = p.ToString();
                }
                Products item = (Products)showProduct.SelectedItem;
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductCode");
                dt.Columns.Add("Product");
                dt.Columns.Add("Mrp");
                dt.Columns.Add("Price");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Discount");
                dt.Columns.Add("Vat");
                dt.Columns.Add("TotalAmount");

                DataRow dr = dt.NewRow();
                dr[0] = item.Pid;
                dr[1] = item.Pname;
                dr[2] = item.Mrp;
                dr[3] = Price.Text;
                dr[4] = Qty.Text;
                dr[5] = discounttxt.Text;
                dr[6] = vattxt.Text;
                dr[7] = TotalAmount.Text;
                dt.Rows.Add(dr);
                datalist.Items.Add(dt.DefaultView);

                if (!string.IsNullOrEmpty(TotalAmount.Text))
                {
                    totalsum = totalsum + float.Parse(TotalAmount.Text);
                    totalcaltxt.Text = totalsum.ToString();
                }
                IEnumerable<Products> _lsproduct = (IEnumerable<Products>)showProduct.ItemsSource;
                Products _p = (Products)showProduct.SelectedItem;

                var distinctValues = (from b in _lsproduct
                                      where b.Pid != _p.Pid
                                      select b).ToList<Products>();
                this.showProduct.ItemsSource = distinctValues;
                vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
                Log.Info("Before: To Add item into datagrid ItemSalesAdd");
            }         
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To delete item from datagrid in Delete ItemSalesAdd");
            var s = ((DataView)this.datalist.SelectedItem)[0].Row.ItemArray;

            this.datalist.Items.Remove(this.datalist.SelectedItem);
            Log.Info("Before: To delete item from datagrid in Delete ItemSalesAdd");
            Log.Info("Before: To ADD item into combo which deleted from datagrid ItemSalesAdd");
            Products _Products = (from P in lst
                                  where P.Pid == Convert.ToInt32(s[0])
                                  select P).First<Products>();
            List<Products> _St = (List<Products>)this.showProduct.ItemsSource;
            _St.Add(_Products);
            Log.Info("After: To ADD item into combo which deleted from datagrid ItemSalesAdd");
            showProduct.ItemsSource = null;
            showProduct.ItemsSource = _St;
            vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            showCustomer.IsEnabled = false;
            List<Sale> lstsale = new List<Sale>();
            Sale _S = null;
            if (datalist.Items.Count > 0)
            {
                Log.Info("Before: To insert data in sales ItemSalesAdd");
                foreach (DataView item in datalist.Items)
                {
                    DataRowView ssss = item[0];
                    var kk = ssss.Row.ItemArray;
                    _S = new Sale();

                    _S.Product.Pid = Convert.ToInt32(kk[0]);
                    _S.Customer = (Customer)showCustomer.SelectedItem;
                    _S.Price = float.Parse(kk[3].ToString());
                    _S.Quantity = float.Parse(kk[4].ToString());
                    _S.Discount = float.Parse(kk[5].ToString());
                    _S.Vat = float.Parse(kk[6].ToString());
                    _S.TotalAmount = float.Parse(kk[7].ToString());
                    _S.SaleDate = (DateTime)SalesDate.SelectedDate;
                    _S.Type = true;
                    datalist.SelectedIndex++;

                    if (string.IsNullOrEmpty(Advanced.Text))
                    {
                        float p = 0;
                        Advanced.Text = p.ToString();
                        _S.Advance = float.Parse(Advanced.Text);
                    }
                    if (datalist.SelectedIndex == datalist.Items.Count - 1)
                    {
                        _S.Advance = float.Parse(Advanced.Text);
                    }
                    Log.Info("Before: Create Linq to get record of selected product ItemSalesAdd");
                    _S.RemainingBalance = _S.TotalAmount - _S.Advance;
                    Products _chk = (from P in lst
                                     where P.Pid == Convert.ToInt32(kk[0])
                                     select P).First<Products>();
                    Log.Info("After: Create Linq to get record of selected product ItemSalesAdd");
                    if (_chk.StockQty >= float.Parse(kk[4].ToString()))
                    {
                        float aq;
                        aq = _chk.StockQty - float.Parse(kk[4].ToString());
                        _chk.StockQty = aq;
                        _S.Billing = billnumber;
                        Log.Info("Before: To update product stock in product Db ItemSalesAdd");
                        Queries.Update<Products>(_chk);
                        Log.Info("After: To update product stock in product Db, Successfully ItemSalesAdd");
                        Queries.Add<Sale>(_S);
                        Log.Info("After: To insert data in sales, Successfully ItemSalesAdd");
                    }
                    else
                    {
                        await mainWindow.ShowMessageAsync("You have Insufficient Stock", "");
                    }              
                    _S = null;
                }
                Billing obje = new Billing();
                obje.BillNo = billnumber;
                Queries.Add<Billing>(obje);

                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Generate Bill",
                    NegativeButtonText = "Cancel",
                };
                MessageDialogResult result1 = await mainWindow.ShowMessageAsync("Sales Record Inserted Successfully", "",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result1 != MessageDialogResult.Negative)
                {
                    Log.Info("Before: generate bill call from ItemSalesAdd");
                    Log.Info("Request to salesbill for report in ItemSalesAdd");
                    SalesBill rpt = new SalesBill(billnumber);                     
                    rpt.ShowDialog();
                    Log.Info("After: generate bill call, from ItemSalesAdd");
                }
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);              
            }
            else
            {
                await mainWindow.ShowMessageAsync("Item not Available in list ", "");
            }
        }
        private async void vattxt_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(Price.Text) && !string.IsNullOrEmpty(Qty.Text))
            {
                calculateTotal();
            }
            else
            {
                if (!string.IsNullOrEmpty(Price.Text))
                {
                    await mainWindow.ShowMessageAsync("Please Enter Qunatity", "");
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please Enter Unitprice", "");
                }
            }
        }

        private async void discounttxt_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (!string.IsNullOrEmpty(Price.Text) && !string.IsNullOrEmpty(Qty.Text))
            {
                calculateTotal();
            }
            else
            {
                if (string.IsNullOrEmpty(Qty.Text))
                {
                    await mainWindow.ShowMessageAsync("Please Enter Qunatity", "");
                    Qty.Focus();
                }
                else if (string.IsNullOrEmpty(Price.Text))
                {
                    await mainWindow.ShowMessageAsync("Please Enter Unitprice", "");
                    Price.Focus();
                }
            }
        }
        public void calculateTotal()
        {
            Products item = (Products)showProduct.SelectedItem;
            if (float.Parse(Qty.Text) <= item.StockQty)
            {
                float p = 0;
                if (string.IsNullOrEmpty(vattxt.Text))
                {                  
                    vattxt.Text = p.ToString();                   
                }
                if (string.IsNullOrEmpty(discounttxt.Text))
                {
                    discounttxt.Text = p.ToString();
                }
                Log.Info("Before: Process to calculate total amount in function named calculateTotal ItemSalesAdd");
                float price, Quantity, _Discount, _Vat, total;
                price = float.Parse(Price.Text);  //1500
                Quantity = float.Parse(Qty.Text);  //3
                total = price * Quantity;       //4500
                _Discount = (total * float.Parse(discounttxt.Text)) / 100;     //90
                _Discount = total - _Discount;   // amount after discount   4410
                _Vat = (_Discount * float.Parse(vattxt.Text)) / 100;     //617.4

                TotalAmount.Text = Convert.ToString(_Discount + _Vat);// 5027.4
                if (Convert.ToInt32(vattxt.Text) == 0)
                {
                    vattxt.Text = string.Empty;
                }
                if (Convert.ToInt32(discounttxt.Text) == 0)
                {
                    discounttxt.Text = string.Empty;
                }
                Log.Info("After: Process to calculate total amount in function named calculateTotal ItemSalesAdd");
            }
        }

        private void GenerateBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void vattxt_KeyUp(object sender, KeyEventArgs e)
        {            
            if (!System.Text.RegularExpressions.Regex.IsMatch(vattxt.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                vattxt.Text = string.Empty;
            }
        }

        private void discounttxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(discounttxt.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                discounttxt.Text = string.Empty;
            }
        }

    }
}
