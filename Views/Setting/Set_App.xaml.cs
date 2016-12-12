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
using Inventory.Model;
using DatabaseAndQueries;
using System.IO;
using System.Diagnostics;

namespace Inventory.Views.Setting
{
    /// <summary>
    /// Interaction logic for Set_App.xaml
    /// </summary>
    public partial class Set_App : Flyout
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));
        private static string dest_path;
        public Inventory.Model.Setting _s;
        public Set_App()
        {
            InitializeComponent();
        }

        private void SetPath_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                FilePathtext.Text = filename;

                string name = "logo.png";             // System.IO.Path.GetFileName(filename);

                string destinationPath = GetDestinationPath(name, "Images");
                dest_path = destinationPath;

                Uri uri = new Uri(filename);
                ImageSource imgSource = new BitmapImage(uri);
                this.setImage.Source = imgSource;
                // set image here
                //  File.Copy(filename, destinationPath, true);
            }

        }
        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            if (appStartPath.IndexOf("\\bin\\Debug") != -1)
            {
                appStartPath = appStartPath.Remove(appStartPath.IndexOf("\\bin\\Debug"));
            }
            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            if (!string.IsNullOrEmpty(company.Text) && !string.IsNullOrEmpty(addressline1.Text)
              && !string.IsNullOrEmpty(addressline2.Text) && !string.IsNullOrEmpty(addressline3.Text)
              && !string.IsNullOrEmpty(addressline4.Text) && !string.IsNullOrEmpty(contactno.Text))
            {
                Log.Info("Before: Setting record updated successfully");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                _s.CompanyName = company.Text;
                _s.ContactNo = Convert.ToInt64(contactno.Text);
                // set path of saved file.
                _s.Address = addressline1.Text + "$" + addressline2.Text + "$" + addressline3.Text + "$" + addressline4.Text;
                _s.Id = 1;

                Queries.Update<Inventory.Model.Setting>(_s);
                Log.Info("After: Setting record updated successfully in Set_App");
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Setting Saved Successfully", "");
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            }
            else
            {
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please Enter Complete Details", "");
            }
            #region OldLogoCode
            //{
            //    if (!string.IsNullOrEmpty(company.Text) && !string.IsNullOrEmpty(addressline1.Text)
            //      && !string.IsNullOrEmpty(addressline2.Text) && !string.IsNullOrEmpty(addressline3.Text)
            //      && !string.IsNullOrEmpty(addressline4.Text) && !string.IsNullOrEmpty(contactno.Text))
            //    {
            //        Log.Info("Before: Setting record updated successfully");
            //        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //        var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            //        controller.SetIndeterminate();
            //        await TaskEx.Delay(1000);

            //        if (!string.IsNullOrEmpty(FilePathtext.Text))
            //        {
            //            var bitmapFrame = BitmapFrame.Create(new Uri(FilePathtext.Text),        // get image path and Dimension.
            //            BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            //            var width = bitmapFrame.PixelWidth;
            //            var height = bitmapFrame.PixelHeight;
            //            if (width == 72 && height == 72)
            //            {

            //                File.Copy(FilePathtext.Text, dest_path, true);                      // save image in specified folder.
            //            }
            //            else
            //            {
            //                await TaskEx.Delay(2000);
            //                await controller.CloseAsync();
            //                await mainWindow.ShowMessageAsync("Logo not found : Please Select Image in '72 x 72' ", "");
            //            }
            //            _s.CompanyName = company.Text;
            //            _s.ContactNo = Convert.ToInt64(contactno.Text);
            //            // set path of saved file.
            //            _s.Address = addressline1.Text + "$" + addressline2.Text + "$" + addressline3.Text + "$" + addressline4.Text;
            //            _s.Id = 1;

            //            Queries.Update<Inventory.Model.Setting>(_s);
            //            Log.Info("After: Setting record updated successfully in Set_App");
            //            await TaskEx.Delay(2000);
            //            await controller.CloseAsync();
            //            await mainWindow.ShowMessageAsync("Setting Saved Successfully", "");
            //            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            //        }
            //        else
            //        {
            //            var mySettings = new MetroDialogSettings()
            //            {
            //                AffirmativeButtonText = "Okay",
            //                NegativeButtonText = "Cancel",
            //            };
            //            MessageDialogResult result1 = await mainWindow.ShowMessageAsync("Are you save the information without Logo?", "",
            //            MessageDialogStyle.AffirmativeAndNegative, mySettings);
            //            if (result1 != MessageDialogResult.Negative)
            //            {
            //                _s.CompanyName = company.Text;
            //                _s.ContactNo = Convert.ToInt64(contactno.Text);
            //                // set path of saved file.
            //                _s.Address = addressline1.Text + "$" + addressline2.Text + "$" + addressline3.Text + "$" + addressline4.Text;
            //                _s.Id = 1;

            //                Queries.Update<Inventory.Model.Setting>(_s);
            //                Log.Info("After: Setting record updated successfully in Set_App");
            //                await TaskEx.Delay(2000);
            //                await controller.CloseAsync();
            //                await mainWindow.ShowMessageAsync("Setting Saved Successfully", "");
            //                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            //            }
            //            else
            //            {
            //                await TaskEx.Delay(2000);
            //                await controller.CloseAsync();
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //        await mainWindow.ShowMessageAsync("Please Enter Complete Details", "");
            //    }
            //}
            # endregion
        }
        private void Set_AppFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Info(" Before: To get from setting ,in Set_App");
                Inventory.Model.Setting set = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>(x => x.Id == 1);
                Log.Info(" After: To get from setting ,in Set_App");
                _s = set;
                if (set != null)
                {
                    DataContext = set;
                    string[] add = set.Address.Split('$');
                    try
                    {
                        addressline1.Text = add[0];
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        addressline2.Text = add[1];
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        addressline3.Text = add[2];
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        addressline4.Text = add[3];
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    Log.Info("setting record not available: ,in Set_App");
                }
            }

            catch (Exception ex)
            {
                Log.Error("Settings is Empty ,in Set_App");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            mainWindow.ToggleFlyout(23);
        }

        private void Set_AppFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
