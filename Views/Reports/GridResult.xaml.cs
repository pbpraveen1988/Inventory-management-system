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

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for GridResult.xaml
    /// </summary>
    /// 
   
    public partial class GridResult : Flyout
    {
     private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

     public  List<Sale> lstsale;
     public String SendDate, Name;
        public GridResult()
        {
            InitializeComponent();
            
        }   

        private void GridResultFlyout_Loaded(object sender, RoutedEventArgs e)
        {
         // lstsale =  (List<Sale>)datalist.ItemsSource;
        }

        private void showCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (showCustomer.SelectedValue != null)
            {
                Log.Info("Before: Process to load item on selection of comboitem");
                if (showCustomer.SelectedValue.ToString().Contains("SALES"))
                {
                    datalist.ItemsSource = lstsale.Where(x => x.Type == true).ToList<Sale>();
                }
                else if (showCustomer.SelectedValue.ToString().Contains("PURCHASE"))
                {
                    datalist.ItemsSource = lstsale.Where(x => x.Type == false).ToList<Sale>();
                }
                else if (showCustomer.SelectedValue.ToString().Contains("BOTH"))
                {
                    datalist.ItemsSource = lstsale;
                }
                Log.Info("After: Process to load item on selection of comboitem");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            if (datalist.Items.Count != 0 )
            {
                Log.Info("Before: Process to load parameter to report ");
                Standard_Customer rpt = new Standard_Customer(SendDate, Name,balance.Content.ToString());
                rpt.saleslst = (List<Sale>)datalist.ItemsSource;            
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
