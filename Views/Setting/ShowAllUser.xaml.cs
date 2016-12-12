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
using DatabaseAndQueries;
using log4net;


namespace Inventory.Views.Setting
{
    /// <summary>
    /// Interaction logic for ShowAllUser.xaml
    /// </summary>
    public partial class ShowAllUser : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
     
        public ShowAllUser()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            controller.SetIndeterminate();
            var result = await mainWindow.ShowInputAsync("Hello!", "Please Enter Password !");
            Log.Info("Before: to Delete user, in ShowAllUser");

            if (result != null) //user pressed cancel
            {
                var selectedItem = datalist.SelectedItem;
                LoginUser _l = (LoginUser)datalist.SelectedItem;
                if (selectedItem != null)
                {
                    LoginUser _u = (LoginUser)Application.Current.Properties["User"];

                    if (_u.Password == result && _u.Role == "Admin" )
                    {
                     //   await mainWindow.ShowMessageAsync("Hello", "Hello " + _u.FirstName + "!");  
                        var mySettings = new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "Okay",
                            NegativeButtonText = "Cancel",
                        };                      
                        MessageDialogResult result1 = await mainWindow.ShowMessageAsync("Are you Sure want to Delete ?", "",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result1 != MessageDialogResult.Negative)
                        {
                            if (_l.Id != _u.Id)
                            {
                                if (_l.Id != 1)
                                {
                                    Log.Info("Before: to Delete user by admin ,in ShowAllUser");
                                    Queries.Delete<LoginUser>(_l);
                                    List<LoginUser> _user = Queries.GetAllData<LoginUser>();
                                    ManageUser manageflyout = mainWindow.Flyouts.Items[25] as ManageUser;
                                    datalist.ItemsSource = manageflyout.Userlistcmb.ItemsSource = _user;
                                    Log.Info("After: to Delete user by admin, Successfully in ShowAllUser");
                                }
                                else
                                {
                                    await TaskEx.Delay(2000);
                                    await controller.CloseAsync();
                                    await mainWindow.ShowMessageAsync("You Cannot Delete Super_Admin", "");
                                }
                            }
                            else
                            {
                                await TaskEx.Delay(2000);
                                await controller.CloseAsync();
                                await mainWindow.ShowMessageAsync("You Cannot Delete yourself", "");
                            }
                        }
                        else
                        {
                            await TaskEx.Delay(2000);
                            await controller.CloseAsync();
                            return;
                        }
                    }
                    else
                    {
                        await TaskEx.Delay(1000);
                        await controller.CloseAsync();
                        await mainWindow.ShowMessageAsync("Hello","You are not authorized to delete this... !"); 
                    }
                }              
            }
            else
            {
                await TaskEx.Delay(1000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please select User", "from list ... !");
                return;
            }
            Log.Info("After: to Delete user, in ShowAllUser");

        }
    }
}
