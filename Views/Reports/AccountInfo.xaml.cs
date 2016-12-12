using Inventory.Model;
using MahApps.Metro.Controls;
using System.Data;
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
using log4net;
using MahApps.Metro.Controls.Dialogs;

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for AccountInfo.xaml
    /// </summary>
    public partial class AccountInfo : Flyout
    {
        DataTable dt;
        float totalrem = 0, totalsale = 0, totalpurchase = 0;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public AccountInfo()
        {
            InitializeComponent();
        }

        private void accountinfoFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.accountinfoFlyout.IsOpen)
            {     
                datelbl.Content = DateTime.Now.Date.ToString("dd/MMM/yyy");
                CustomerViewModel _s = new CustomerViewModel();
                this.DataContext = _s;
                List<Account> _aclst = _s.Accounts.ToList<Account>();

                List<Customer> lst = _s.Customers.ToList<Customer>();         
                dt = new DataTable();      
                dt.Columns.Add("Customer");
                dt.Columns.Add("Sales");
                dt.Columns.Add("Purchase");
                dt.Columns.Add("Remaining");

                foreach (var item in lst)
                {
                    var acccount = item.Account.Where(x => x.Customer.Cid == item.Cid).Sum(x => x.PayAmount);

                    var custimername = "";
                    if (item.Type == 1 || item.Type == 3)
                    {
                        custimername = item.Cname + " [Dealer]";
                    }
                    else
                    custimername = item.Cname;
                    if (item.Sales != null && item.Sales.Count > 0)
                    {
                        var saletotal = item.Sales.Where(x => x.Type == true).Sum(x => x.TotalAmount);
                        var purchasetoto = item.Sales.Where(x => x.Type == false).Sum(x => x.TotalAmount);
                        var remain = item.Sales.Sum(x => x.RemainingBalance);
                        var p = remain - acccount;

                        DataRow dr = dt.NewRow();
                        dr[0] = custimername;
                        dr[1] = saletotal;
                        dr[2] = purchasetoto;
                        dr[3] = p;     // remain;                     
                        dt.Rows.Add(dr);
                        totalsale = totalsale + saletotal;
                        totalpurchase = totalpurchase + purchasetoto;
                        totalrem = totalrem - remain;
                    }                
                }            
                datalist.DataContext = dt.DefaultView;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datalist.Items.Count != 0)
            {       
                Log.Info("Before: Process to load parameter to report ");
                AccountView rpt = new AccountView(totalsale,totalpurchase,totalrem);   
                rpt.datatable = dt;
                rpt.ShowDialog();
                Log.Info("After: Process to load parameter to report successfully");
            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Data not available to print", "");
            }
        }
    }
}
