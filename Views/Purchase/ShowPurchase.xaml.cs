using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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


namespace Inventory.Views.Purchase
{
    /// <summary>
    /// Interaction logic for ShowPurchase.xaml
    /// </summary>
    public partial class ShowPurchase : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));       
        public ShowPurchase()
        {
            InitializeComponent();
                                                     // type= false for Purchase
        }

        private void showPurchaseFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.showPurchaseFlyout.IsOpen)
            {
                this.DataContext = new SalesViewModel();
            }
        }
        public void GenerateBill(object sender, RoutedEventArgs e)
        {
         //  MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Button b = sender as Button;
            Sale sale = b.CommandParameter as Sale;         

                Log.Info("Before: generate bill call from ShowPurchase");
                Log.Info("Request to salesbill for report");
                PurchaseBill rpt = new PurchaseBill(sale.PurchaseBill);
                rpt.ShowDialog();
                Log.Info("After: generate bill call, from ShowPurchase");
            
        }
    }
}
