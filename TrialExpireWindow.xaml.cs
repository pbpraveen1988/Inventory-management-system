using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for TrialExpireWindow.xaml
    /// </summary>
    public partial class TrialExpireWindow : MetroWindow
    {
        public TrialExpireWindow()
        {
            InitializeComponent();
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
            System.Diagnostics.Process.Start("http://stackinfotech.com");
        } 
    }
}
