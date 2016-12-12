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
using Inventory.Views.Reports;

namespace Inventory.Views.Filter
{
    /// <summary>
    /// Interaction logic for ProductSearch.xaml
    /// </summary>
    public partial class ProductSearch : Flyout
    {
        List<Products> lst;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

       
        public ProductSearch()
        {
            Log.Info("Before InitializeComponent Product Search");
            InitializeComponent();
            Log.Info("After InitializeComponent Product Search");
          
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(showProduct.Text)
          && !string.IsNullOrEmpty(FirstDate.Text) && !string.IsNullOrEmpty(SecondDate.Text))
            {
                Log.Info("Before: Clicking to Product Search");
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                ShowProduct flyout = mainWindow.Flyouts.Items[17] as ShowProduct;

                lst = (List<Products>)showProduct.ItemsSource; 

                Products _chk = (from l in lst
                                 where l.Pid == (int)showProduct.SelectedValue
                                 select l).FirstOrDefault<Products>();
                Log.Info("Before: Set to Datagrid Product Search Record");  


               flyout.datalist.ItemsSource =                   
                   Queries.GetAllByCondition<Sale>(x => x.Product.Pid == (int)showProduct.SelectedValue
                  && (x.SaleDate >= FirstDate.SelectedDate) && (x.SaleDate <= SecondDate.SelectedDate));            

                    flyout.cname.Content = showProduct.Text;
                    flyout.fdate.Content = FirstDate.SelectedDate.Value.ToString("dd-MMM-yyyy");
                    flyout.ldate.Content = SecondDate.SelectedDate.Value.ToString("dd-MMM-yyyy");
                    flyout.stockqty.Content = _chk.StockQty;
                    Log.Info("After: Set to Datagrid Product Search Record");
                                 
                    //rpt.productid = (int)showProduct.SelectedValue;

                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                    mainWindow.ToggleFlyout(17);
                    showProduct.Text = string.Empty;
                    FirstDate.Text = string.Empty;
                    SecondDate.Text = string.Empty;
                    Log.Info("After: Clicking to Product Search");
                    await controller.CloseAsync();
                    if (flyout == null)
                    {
                        return;
                    }
                }                         
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Please Enter the Complete Details", "");
            }
        }
    }
}
