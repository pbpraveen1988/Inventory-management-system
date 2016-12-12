
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
namespace Inventory.Views.Setting
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Flyout
    {
        public List<LoginUser> _user;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));      
        public AdminView()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(22);
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Log.Info("Before: to get list of all User created by Super admin in AdminView");
            ShowAllUser flyout = mainWindow.Flyouts.Items[24] as ShowAllUser;
            flyout.datalist.ItemsSource = _user;
            Log.Info("After: to get list of all User created by Super admin in AdminView");
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(24);
          
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {  
             MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;          
             ManageUser manageflyout = mainWindow.Flyouts.Items[25] as ManageUser;
             manageflyout.Userlistcmb.ItemsSource = _user;
             Helpful.CloseAllFlyouts(mainWindow.Flyouts);
             mainWindow.ToggleFlyout(25);          
        }

        private void Flyout_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void Flyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: to get list of all user Successfully, in AdminView");
            _user = DatabaseAndQueries.Queries.GetAllData<LoginUser>();
            Log.Info("After: to get list of all user Successfully, in AdminView");
        }
    }
}
