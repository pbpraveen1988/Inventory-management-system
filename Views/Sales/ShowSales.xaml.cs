using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
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
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Shapes;

namespace Inventory.Views.Sales
{
    /// <summary>
    /// Interaction logic for ShowSales.xaml
    /// </summary>
    public partial class ShowSales : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));       
        public ShowSales()
        {
            InitializeComponent();          
                                                                                                     // type = true for Sales.
        }

        public void GenerateBill(object sender, RoutedEventArgs e)
        {
          
            Button b = sender as Button;
            Sale sale = b.CommandParameter as Sale;                

                    Log.Info("Before: generate bill call from ShowSales");
                    Log.Info("Request to salesbill for report");
                    SalesBill rpt = new SalesBill(Convert.ToInt32(sale.Billing));
                    rpt.ShowDialog();
                    Log.Info("After: generate bill call, from ShowSales");
              
        }

        private void showSalesFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.showSalesFlyout.IsOpen)
            {
                Log.Info("Before: To set data context in showSalesFlyout_IsOpenChanged ShowSales");
                this.DataContext = new SalesViewModel();
                Log.Info("After: To set data context in showSalesFlyout_IsOpenChanged ShowSales");
            }
        }
    }
}
