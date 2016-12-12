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
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using log4net;
namespace Inventory
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Login));
        DispatcherTimer dtClockTime = new DispatcherTimer();
        public Login()
        {
            InitializeComponent();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UserName.Text))
            {
                if (!string.IsNullOrEmpty(Password.Password))
                {
                    var controller = await this.ShowProgressAsync("Please wait...", "We are baking some cupcakes!");
                    controller.SetIndeterminate();
                    Log.Info("Before: To check username & pwd from DB");
                    await TaskEx.Delay(3000);
                    LoginUser _use = DatabaseAndQueries.Queries.GetDataByCondition<LoginUser>(x => x.FirstName == UserName.Text && x.Password == Password.Password);
                    if (_use != null && _use.Trial.Date != DateTime.Now.Date )
                    {
                        Application.Current.Properties["User"] = _use;                      
                        await controller.CloseAsync();
                        isClosingConfirmed = true;
                        new MainWindow().Show();
                        this.Close();
                    }
                    else
                    {
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Invalid Credentails!", "Please Check the Details");
                    }
                    Log.Info("After: To check username & pwd from DB,Successful");
                }
                else
                {
                    await this.ShowMessageAsync("Missing!", "Please Enter the Password");
                }

            }
            else
            {
                await this.ShowMessageAsync("Missing!", "Please Enter the Username");
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.isClosingConfirmed)
            {
                // window will close, if e.Cancel is passed in as "false"
                return;
            }
            this.ShowConfirmationDialog();
            e.Cancel = true;
        }

        private bool isClosingConfirmed;

        private async void ShowConfirmationDialog()
        {

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Okay",
                NegativeButtonText = "Cancel",
                ColorScheme = MetroDialogOptions.ColorScheme
            };
            MessageDialogResult result = await this.ShowMessageAsync("Are you Sure want to Close ?", "",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result != MessageDialogResult.Negative)
            {
                this.isClosingConfirmed = true;
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
