using Inventory.Model;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace Inventory.Views.Firstlook
{
    /// <summary>
    /// Interaction logic for MasterPassword.xaml
    /// </summary>
    public partial class MasterPassword : MetroWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
  //      public int _ucount = 0;
        public MasterPassword()
        {
            InitializeComponent();
        }
            

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To Enter master key first time installation in MasterPassword ");
        //    if (_ucount == 0)
          //  {
                if (Masterkey.Password == GetMacAddress() + "STACK" && !string.IsNullOrEmpty(trialtxt.Text))
                {
                    LoginUser _User = new LoginUser();
                    _User.FirstName = "admin";
                    _User.Password = "admin";
                    _User.Role = "Admin";
                    DateTime endDate = DateTime.Now;
                    _User.Trial = endDate.AddDays(Convert.ToInt32(trialtxt.Text));
                    Log.Info("Before: To insert first login detail in MasterPassword");
                    DatabaseAndQueries.Queries.Add<LoginUser>(_User);
               //     _ucount = 1;
               //     new Login().Show();
                    Masterkey.Password = string.Empty;
                    trialtxt.Text = string.Empty;
                //    this.isClosingConfirmed = true;
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Your Application is Successfully Install, Click Yes to Restart Application", "Success", System.Windows.MessageBoxButton.YesNo);
                    Log.Info("Before: To first start of application after MasterPassword");
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {                      
                        new LoadingWindow().Show();
                        this.Close();
                        Log.Info("After: To first start of application after MasterPassword, successfully");
                    }
                    else 
                        this.Close();           
                    Log.Info("After: To insert first login detail in MasterPassword, successfully");
                }
                else
                {                   
                    Masterkey.Password = string.Empty;
                    trialtxt.Text = string.Empty;
                    MessageBox.Show("Given Information is not Valid......", "");
                    Masterkey.Focus();
                }
      
           //     this.Close();
                Log.Info("After: To Enter master key first time installation in MasterPassword, Successfully ");
        }

        static string GetMacAddress()
        {
            Log.Info("Before: inside GetMacAddress method: in MasterPassword");
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    //continue;
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            } Log.Info("After: inside GetMacAddress method in MasterPassword, Successfully");
            return macAddresses;
        }

        private void trialtxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(trialtxt.Text, "^[0-9]*$"))
            {
                trialtxt.Text = string.Empty;
                MessageBox.Show("Only Number Allowed");
            }     
        }
    }
}
