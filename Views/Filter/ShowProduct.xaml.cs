using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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



namespace Inventory.Views.Filter
{
    /// <summary>
    /// Interaction logic for ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public ShowProduct()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Process to Create Report on ShowProduct");
            if (datalist.Items.Count != 0)
            {
                List<Sale> lst = (List<Sale>)datalist.ItemsSource;
                SearchProduct rpt = new SearchProduct(fdate.Content.ToString()
                                                  ,ldate.Content.ToString(),
                                                  lst,
                                                  stockqty.Content.ToString(),
                                                  cname.Content.ToString());             

                rpt.ShowDialog();             
                Log.Info("After: Process to Create Report on ShowProduct");
            }
            else
            {
                MessageBox.Show("No Records Found");
            }
        }
    }
}
