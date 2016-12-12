using Inventory.Model;
using log4net;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TilesExample.xaml
    /// </summary>
    public partial class TilesExample : UserControl
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Login));
        public TilesExample()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
              LoginUser _user = (LoginUser)Application.Current.Properties["User"];
              if (_user.Role == "Admin" || _user.Role == "Manager")
              {
                  MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                  Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                  mainWindow.ToggleFlyout(0);
              }            
        }

        private void Tile_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin" || _user.Role == "Manager")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(3);
            }
        }
        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin" || _user.Role == "Manager")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(6);
            }
        }
        private void Tile_Click_3(object sender, RoutedEventArgs e)
        {
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin" || _user.Role == "Manager")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(9);
            }
        }

        private void Tile_Click_4(object sender, RoutedEventArgs e)
        {
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin" || _user.Role == "Manager")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(13);
            }
        }

        private void Tile_Click_5(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To check role of user in tileexample");
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];       
            if (_user.Role == "Admin")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(19);
            }
        }

        private void Tile_Click_6(object sender, RoutedEventArgs e)
        {
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin" || _user.Role == "Manager" || _user.Role == "End_User")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(20);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Log.Info("Before: To check role of user in tileexample");
            //LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            //if (_user.Role == "End_User")
            //{
            //    customer.IsEnabled = false;
            //    product.IsEnabled = false;
            //    sales.IsEnabled = false;
            //    purchase.IsEnabled = false;
            //    settings.IsEnabled = false;
            //}
            //else if (_user.Role == "Manager")
            //{
            //    settings.IsEnabled = false;
            //}
            //Log.Info("After: To check role of user in tileexample,successfully");
        }         

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: To check role of user in tileexample");
            LoginUser _user = (LoginUser)Application.Current.Properties["User"];
            if (_user.Role == "Admin")
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                mainWindow.ToggleFlyout(29);
            }
        }
    }
}
