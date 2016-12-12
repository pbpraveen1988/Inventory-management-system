using Inventory.Model;
using Inventory.Views.Reports;
using log4net;
using MahApps.Metro.Controls;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Inventory.Views.Filter
{
    /// <summary>
    /// Interaction logic for ShowRecord.xaml
    /// </summary>
    public partial class ShowRecord : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public ShowRecord()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Process to Create Excel Sheet on ShowRecord of Customer");
            if (datalist.Items.Count != 0)
            {
                List<Sale> lst = (List<Sale>)datalist.ItemsSource;
                SearchCustomer rpt = new SearchCustomer(fdate.Content.ToString()
                                                  , ldate.Content.ToString(),
                                                  lst, cname.Content.ToString(),
                                                  advance.Content.ToString(),
                                                  balance.Content.ToString());
                rpt.ShowDialog();
                Log.Info("After: Process to Create Excel Sheet on ShowRecord of Customer");
            }
            else
            {
                MessageBox.Show("No Records Found");
            }
        }
    }
}
