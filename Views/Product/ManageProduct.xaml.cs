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
using DatabaseAndQueries;

namespace Inventory.Views.Product
{
    /// <summary>
    /// Interaction logic for ManageProduct.xaml
    /// </summary>
    public partial class ManageProduct : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public ManageProduct()
        {
            InitializeComponent();        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   Log.Info("Clicking to find  Edit Customer");
            Products _Cdb = (Products)productname.SelectedValue;
            pid.Text = _Cdb.Pid.ToString();
            Pname.Text = _Cdb.Pname;
            Description.Text = _Cdb.Description;
         //   showUnit.SelectedValue = _Cdb.Unit.Id ;
            datastack.IsEnabled = true;
        //    Log.Info("After find  Edit Customer Successfully");
        }

        private async void CreateUpdate_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: In CreateUpdate_Click ManageProduct");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            controller.SetIndeterminate();
            await TaskEx.Delay(1000);
            Products _Cdb = new Products();
            _Cdb.Pid = int.Parse(pid.Text);
            _Cdb.Pname = Pname.Text;
            _Cdb.Description = Description.Text;
        //    _Cdb.Unit.Id = (int)showUnit.SelectedValue;
            _Cdb.Date = DateTime.Now;

            Queries.Update<Products>(_Cdb);

            await TaskEx.Delay(2000);
            await controller.CloseAsync();
            await mainWindow.ShowMessageAsync("Product Record Updated Successfully", "");

            var flyout = mainWindow.Flyouts.Items[8] as ShowProduct;
            if (flyout == null)
            {
                return;
            }
            Log.Info("Before:To Get List of all product in CreateUpdate_Click ManageProduct ");
            flyout.DataContext = Queries.GetAllData<Products>();
            Log.Info("After:To Get List of all product in CreateUpdate_Click ManageProduct ");
            Description.Text = string.Empty;
            Pname.Text = string.Empty;
            pid.Text = string.Empty;
            datastack.IsEnabled = false;
            Log.Info("After:  In CreateUpdate_Click ManageProduct Edit Product record successfully");
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
        }
        private void productname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           var p =  (sender as ComboBox).SelectedItem as Products;
           if (p != null)
           {
               Pname.Text = p.Pname;
               Description.Text = p.Description;
               mrptxt.Text = p.Mrp.ToString();
               pid.Text = p.Pid.ToString();
           }
           else
           {
               Pname.Text = string.Empty;
               mrptxt.Text = string.Empty;
               Description.Text = string.Empty;
               pid.Text = String.Empty;
           }
        }
        private void manageproductFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.manageproductFlyout.IsOpen)
            {
                this.DataContext = new ProductViewModel();
            }
        }
        private void mrptxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(mrptxt.Text, @"^[0-9]*(?:\.[0-9]*)?$"))
            {
                mrptxt.Text = string.Empty;
            }
        }
    }
}
