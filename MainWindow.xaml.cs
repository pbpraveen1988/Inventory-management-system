using Inventory.Model;
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
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using log4net;
using Inventory.Views.Reports;


namespace Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

      
        public MainWindow()
        {
            Log.Info("Before Initialization");
            InitializeComponent();
           
            Log.Info("After Initialzation");
        }    

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            
        }
        public  void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //
            SetLanguageDictionary();
            //
        }
        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();

            dict.Source = new Uri("..\\Resources\\StringResources.fr-CA.xaml", UriKind.Relative);

            this.Resources.MergedDictionaries.Add(dict);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();

            dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);

            this.Resources.MergedDictionaries.Add(dict);
        }

     

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://stackinfotech.com");
        }             
        //private void settingmenu_Click(object sender, RoutedEventArgs e)
        //{
        //    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        //    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
        //    mainWindow.ToggleFlyout(19);
        //}

        //private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        //{
        //    LoadingWindow win = new LoadingWindow();
        //    win.Show();
        //    this.Close();
        //}

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
            Application.Current.MainWindow = this;
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
    }
}
