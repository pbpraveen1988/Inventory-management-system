using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Inventory.Views.Product
{
    /// <summary>
    /// Interaction logic for ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Flyout, INotifyPropertyChanged
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
     
        public ShowProduct()
        {
            InitializeComponent();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void showProductFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.showProductFlyout.IsOpen)
            {
                Log.Info("Before: To set data context in showProductFlyout_IsOpenChanged ShowProduct ");
                this.DataContext = new ProductViewModel();
                Log.Info("After:To set data context in showProductFlyout_IsOpenChanged ShowProduct ");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AllProduct rpt = new AllProduct();
            rpt.ShowDialog();    
        }

        private void Product_Name_KeyUp(object sender, KeyEventArgs e)
        {
            Log.Info("Before: Sort Product datalist according to search text box");
            IEnumerable<Products> _productlst = (IEnumerable<Products>)datalist.ItemsSource;
            datalist.ItemsSource = from cust in _productlst
                                   where cust.Pname.ToLower().StartsWith( Product_Name.Text.ToLower())                                  
                                   select cust;
            Log.Info("After: Sort Product datalist according to search text box, Successful");
        }
    }
}
