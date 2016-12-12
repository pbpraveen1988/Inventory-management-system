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
using Inventory.Model;
using log4net;

namespace Inventory.Views.Setting
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public CreateUser()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(Usertxt.Text) && !string.IsNullOrEmpty(passwordtxt.Password)
                && !string.IsNullOrEmpty(showUserCmb.Text))
            {
                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Log.Info("Before: to add user check existence of user in CreateUser");
                LoginUser _u = (LoginUser)Application.Current.Properties["User"];
                if (!Queries.IsExists<LoginUser>(x => x.FirstName == Usertxt.Text.ToLower()))
                {
                    Log.Info("Before: to add New user");
                    LoginUser _user = new LoginUser();
                    _user.FirstName = Usertxt.Text;
                    _user.Password = passwordtxt.Password;
                    _user.Role = showUserCmb.Text;
                    _user.Trial = _u.Trial;
                    Queries.Add<LoginUser>(_user);
                    Log.Info("After: to add New user,Successfully in CreateUser");
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("User Added Successfully", "");
                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                }
                else
                {
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Please Select another Username:", "");
                    Usertxt.Text = string.Empty;
                    Usertxt.Focus();
                }
                Log.Info("After: to add user check existence of user,Successfully in CreateUser");
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please Fill all the fields", "");
            }
        }

        private void Set_AppFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            Usertxt.Text = string.Empty;
            passwordtxt.Password = string.Empty;
        }
    }
}
