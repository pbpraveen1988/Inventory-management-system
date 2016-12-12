using DatabaseAndQueries;
using Inventory.Model;
using log4net;
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
using System.Data;

namespace Inventory.Views.Purchase
{
    /// <summary>
    /// Interaction logic for ItemPurchaseAdd.xaml
    /// </summary>
    public partial class ItemPurchaseAdd : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        List<Products> lst;
        public ItemPurchaseAdd()
        {
            InitializeComponent();
        }

        private void ItempurchaseAdd_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            Log.Info("Before:To set data context in ItempurchaseAdd_IsOpenChanged ItemPurchaseAdd");
            if (this.ItempurchaseAdd.IsOpen)
            {
                SalesViewModel _s = new SalesViewModel();
                this.DataContext = _s;
                lst = _s.Products.ToList<Products>();

                List<Customer> _dealerlst = _s.Customerlist.Where<Customer>(x => x.Type == 1 || x.Type == 2).ToList();
                showCustomer.ItemsSource = _dealerlst; 

            }
            Log.Info("After:To set data context in ItempurchaseAdd_IsOpenChanged ItemPurchaseAdd");
            showProduct.ItemsSource = lst;
            showCustomer.IsEnabled = true;
            billnotxt.IsEnabled = true;
            datalist.Items.Clear();
            mrplbl.Content = string.Empty;
            billnotxt.Text = string.Empty;
            vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
        }
        private void showCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Info("Before:To set data context in showCustomer_SelectionChanged ItemPurchaseAdd");
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
            Log.Info("After:To set data context in showCustomer_SelectionChanged ItemPurchaseAdd");
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
        private void showProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Info("Before:To set data context in showProduct_SelectionChanged ItemPurchaseAdd");
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
            } Log.Info("After:To set data context in showProduct_SelectionChanged ItemPurchaseAdd");
        }
        private void Advanced_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Advanced.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                Advanced.Text = string.Empty;
            }
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
        private void Delete(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To Delete item from Datagrid ItemPurchaseAdd");
            var s = ((DataView)this.datalist.SelectedItem)[0].Row.ItemArray;

            this.datalist.Items.Remove(this.datalist.SelectedItem);

            Products _Products = (from P in lst
                                  where P.Pid == Convert.ToInt32(s[0])
                                  select P).First<Products>();
            Log.Info("After: To Delete item from Datagrid ItemPurchaseAdd");
            Log.Info("Before: To Add item in combo which is deleted from datagrid in Delete ItemPurchaseAdd");
            List<Products> _St = (List<Products>)this.showProduct.ItemsSource;
            _St.Add(_Products);
            showProduct.ItemsSource = null;
            showProduct.ItemsSource = _St;
            vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
            Log.Info("After: To Add item in combo which is deleted from datagrid in Delete ItemPurchaseAdd");
        }
        private async void Qty_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(Qty.Text))
            {
                Products item = (Products)showProduct.SelectedItem;

                if (!string.IsNullOrEmpty(Price.Text))
                {
                    if (!string.IsNullOrEmpty(discounttxt.Text))
                    {
                        if (!string.IsNullOrEmpty(vattxt.Text))
                        {
                            Log.Info("Before: Process to calculate total amount in Qty_LostFocus Add Sales");
                            float price, Quantity, _Discount, _Vat, total;
                            price = float.Parse(Price.Text);  //1500
                            Quantity = float.Parse(Qty.Text);  //3
                            total = price * Quantity;       //4500

                            _Discount = (total * float.Parse(discounttxt.Text)) / 100;     //90
                            _Discount = total - _Discount;   // amount after discount   4410
                            _Vat = (_Discount * float.Parse(vattxt.Text)) / 100;     //617.4

                            TotalAmount.Text = Convert.ToString(_Discount + _Vat);// 5027.4

                            Log.Info("After: Process to calculate total amount in Qty_LostFocus Add Sales");
                        }
                    }
                    else
                    {
                        Log.Info("Before: Process to calculate total amount in Qty_LostFocus Add Sales");
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

                        Log.Info("After: Process to calculate total amount in Qty_LostFocus Add Sales");
                    }
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please Enter Unitprice", "");
                    Price.Focus();
                }

            }
            else
            {
                await mainWindow.ShowMessageAsync("Please Enter Quantity", "");
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
            float p = 0;
            if (string.IsNullOrEmpty(vattxt.Text))
            {
                vattxt.Text = p.ToString();
            }
            if (string.IsNullOrEmpty(discounttxt.Text))
            {
                discounttxt.Text = p.ToString();
            }
            Log.Info("Before: Process to calculate total amount in function named calculateTotal in ItemAddpurchase");
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
            Log.Info("After: Process to calculate total amount in function named calculateTotal in ItemAddpurchase");

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(TotalAmount.Text) && !string.IsNullOrEmpty(showProduct.Text)
                && float.Parse(Qty.Text) != 0)
            {
                Log.Info("Before: Process to set item in datagrid ,in ItemAddpurchase");
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

                IEnumerable<Products> _lsproduct = (IEnumerable<Products>)showProduct.ItemsSource;
                Products _p = (Products)showProduct.SelectedItem;

                var distinctValues = (from b in _lsproduct
                                      where b.Pid != _p.Pid
                                      select b).ToList<Products>();
                this.showProduct.ItemsSource = distinctValues;
                vattxt.Text = discounttxt.Text = TotalAmount.Text = Price.Text = Qty.Text = Advanced.Text = string.Empty;
                Log.Info("After: Process to set item in datagrid Successfully ,in ItemAddpurchase");
            }
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            submit:
            Log.Info("Before: Process to Insert data in Db ,in ItemAddpurchase");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            showCustomer.IsEnabled = false;
            List<Sale> lstsale = new List<Sale>();
            Sale _S = null;
            if (datalist.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(billnotxt.Text))
                {

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
                        _S.Type = false;
                        datalist.SelectedIndex++;

                        if (string.IsNullOrEmpty(Advanced.Text))
                        {
                            float p = 0;
                            Advanced.Text = p.ToString();
                            _S.Advance = (float.Parse(Advanced.Text));
                        }
                        if (datalist.SelectedIndex == datalist.Items.Count - 1)
                        {
                            _S.Advance = (float.Parse(Advanced.Text));
                        }

                        _S.RemainingBalance = -(_S.TotalAmount - _S.Advance);
                        Log.Info("Before: create Linq to get selected product record ,in ItemAddpurchase");
                        Products _chk = (from P in lst
                                         where P.Pid == Convert.ToInt32(kk[0])
                                         select P).First<Products>();
                        Log.Info("After: create Linq to get selected product record Successfully,in ItemAddpurchase");
                        float aq;
                        aq = _chk.StockQty + float.Parse(kk[4].ToString());
                        _chk.StockQty = aq;
                        _S.PurchaseBill = billnotxt.Text;
                        Log.Info("Before: Update Product stock in product table ,in ItemAddpurchase");
                        Queries.Update<Products>(_chk);
                        Log.Info("After: Update Product stock in product table successfully, in ItemAddpurchase");
                        Log.Info("Before: To add purchase record in sales Db, in ItemAddpurchase");
                        Queries.Add<Sale>(_S);
                        Log.Info("After: To add purchase record in sales Db successfully, in ItemAddpurchase");
                        _S = null;
                        Log.Info("After: Process to Insert data in Db successfully, in ItemAddpurchase");                    
                    }
                    await mainWindow.ShowMessageAsync("Purchase Record Added Successfully", "");
                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                }
                else
                {
                    var mySettings = new MetroDialogSettings()
                   {
                         AffirmativeButtonText = "Yes",
                         NegativeButtonText = "No",
                    };
                    MessageDialogResult result1 = await mainWindow.ShowMessageAsync("Do You Have Bill No:", "",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result1 == MessageDialogResult.Negative)
                    {
                        Log.Info("Before: To get list of Sales in ItemPurchaseAdd");                       
                        List<Sale> _blist = Queries.GetAllByCondition<Sale>(x=>x.Type == false);    // for all purchase record
                        Log.Info("After: To get list of Sales in ItemPurchaseAdd");
                        if (_blist.Count == 0)
                        {
                            billnotxt.Text = "1001"+ _blist.Count;                           
                        }
                        else
                        {
                            var bp = _blist.Last();                            
                            billnotxt.Text = bp.PurchaseBill + _blist.Count;                  
                        }
                        billnotxt.IsEnabled = false;
                        Log.Info("goTo submit label in ItemPurchaseAdd");     
                        goto submit;
                       // await mainWindow.ShowMessageAsync("Please Enter Bill No:", "");
                    }
                    billnotxt.Focus();
                }
            }
            else
            {
                await mainWindow.ShowMessageAsync("Item not Available in list ", "");
            }
        }


    }
}
