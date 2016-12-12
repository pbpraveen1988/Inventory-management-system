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
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using DatabaseAndQueries;
using log4net;

namespace Inventory.Views.Product
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));       
        public AddProduct()
        {
            InitializeComponent();          
        }
        private async  void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: Process to add Product record in AddProduct");
            if (!string.IsNullOrEmpty(pname.Text) && !string.IsNullOrEmpty(description.Text))
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);

                Products _p = new Inventory.Model.Products();
                _p.Pname = pname.Text;
                _p.Description = description.Text;
                _p.Date = DateTime.Now;
               
                Log.Info("Before: check to add Product record in AddProduct");

                if (Queries.IsExists<Products>(x => x.Pname == (pname.Text)))
                {
                    await mainWindow.ShowMessageAsync("Product Record Already Exist", "");
                }
                else
                {
                    DatabaseAndQueries.Queries.Add<Products>(_p);
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();

                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                    var flyout = mainWindow.Flyouts.Items[8] as ShowProduct;


                    if (flyout == null)
                    {
                        return;
                    }
                    Log.Info("Before: To add Product record in AddProduct");

                    flyout.datalist.ItemsSource = DatabaseAndQueries.Queries.GetAllData<Products>();
                    pname.Text = string.Empty;
                    description.Text = string.Empty;
                    await mainWindow.ShowMessageAsync("Product Record Inserted Successfully", "");
                    Log.Info("After: To add Product record in AddProduct");

                }
                Log.Info("After: check to add Product record in AddProduct");

            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                await mainWindow.ShowMessageAsync("Please Enter the Complete Details", "");
            }
         
        }

        private void addproductFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.addproductFlyout.IsOpen)
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

        private void addproductFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            mrptxt.Text = string.Empty;
            pname.Text = string.Empty;
            description.Text = string.Empty;                 
        }
    }
}
