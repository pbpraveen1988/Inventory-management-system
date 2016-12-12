using DatabaseAndQueries;
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

namespace Inventory.Views.Setting
{
    /// <summary>
    /// Interaction logic for ManageUser.xaml
    /// </summary>
    public partial class ManageUser : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public ManageUser()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To change password in ManageUser");          
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            controller.SetIndeterminate();
            if(Userlistcmb.SelectedItem != null && !string.IsNullOrEmpty(oldpwdtxt.Password)
                 && !string.IsNullOrEmpty(Newpwdtxt.Password) && !string.IsNullOrEmpty(confirmpwdtxt.Password)
                 && !string.IsNullOrEmpty(Usertxt.Text))
            {
                Log.Info("Before: To check username with password is exist in ManageUser");  
              LoginUser _loguser =  Queries.GetDataByCondition<LoginUser>(x => x.FirstName == Userlistcmb.Text && x.Password == oldpwdtxt.Password);
              Log.Info("After: To check username with password is exist, Successfully in ManageUser");  
                if(_loguser != null)
               {
                   if(Newpwdtxt.Password == confirmpwdtxt.Password)
                   {
                       if (!Queries.IsExists<LoginUser>(x => x.FirstName == Usertxt.Text.ToLower()))
                       {
                           Log.Info("Before: To change user password in ManageUser");
                           _loguser.Password = Newpwdtxt.Password;
                           _loguser.FirstName = Usertxt.Text;
                           Queries.Update<LoginUser>(_loguser);

                           Application.Current.Properties["User"] = _loguser;

                           await TaskEx.Delay(2000);
                           await controller.CloseAsync();
                           await mainWindow.ShowMessageAsync("Record Updated Successfully", "");
                           Newpwdtxt.Password = oldpwdtxt.Password = confirmpwdtxt.Password = string.Empty;
                           Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                           Log.Info("After: To change user password,Successfully in ManageUser");
                       }
                       else
                       {
                           await TaskEx.Delay(2000);
                           await controller.CloseAsync();
                           await mainWindow.ShowMessageAsync("Please Select another Username:", "");
                           Usertxt.Text = string.Empty;
                           Usertxt.Focus();
                       }
                   }
                   else
                   {
                     await TaskEx.Delay(2000);
                     await controller.CloseAsync();
                     await mainWindow.ShowMessageAsync("New Password should be match with Confirm Password", "");
                     Newpwdtxt.Password = confirmpwdtxt.Password = string.Empty;
                     Newpwdtxt.Focus();
                   }
               }
               else
               {
                   // please enter valid password
                   await TaskEx.Delay(2000);
                   await controller.CloseAsync();
                   await mainWindow.ShowMessageAsync("Please enter valid password", "");
                   oldpwdtxt.Focus();
                   oldpwdtxt.Password = string.Empty;
               }
            }
            else
            {
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please Fill all the fields", "");
                //  please fill all the field.
            }
            Log.Info("After: To change password in ManageUser");  
        }

        private void Userlistcmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             var p = (sender as ComboBox).SelectedItem as LoginUser;
             if (p != null)
             {
                 Usertxt.Text = p.FirstName;               
             }
             else
             {
                 Usertxt.Text = string.Empty;
             }
        }
    }
}
