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
using Inventory.Model;
using System.IO;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using log4net;
using DatabaseAndQueries;
using Inventory.Views.Firstlook;
namespace Inventory
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : MetroWindow
    {
        DispatcherTimer dtClockTime = new DispatcherTimer();
        DispatcherTimer newslider = new DispatcherTimer();
        static int count = 0;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoadingWindow));

        public LoadingWindow()
        {
            InitializeComponent();
            newslider.Interval = new TimeSpan(0, 0, 2);
            newslider.Tick += Sldier_Tick;

        }

        private void dtClockTime_Tick(object sender, EventArgs e)
        {


        }

        private static void CopyDataBase()
        {
            Log.Info("Before: inside copy database LodingWindow");
            try
            {
                Log.Info("Before: inside try part in LodingWindow");
                string startUpDB = System.AppDomain.CurrentDomain.BaseDirectory + @"CTE_Data.db";
                //   string startUpDB = System.AppDomain.CurrentDomain.BaseDirectory + @"Database\CTE_Data.db";
                string localAppDataDB = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA\CTE_Data.db";

                string startUpDB64 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\CTE_Data.db";
                Log.Info("Before: To check file is exist in apppath 1st if not");
                if (!File.Exists(localAppDataDB))
                {
                    Log.Info("After: To check file is exist in apppath 1st if not, success");
                    Log.Info("Before: To check file is exist in startup path 2nd if in LodingWindow");
                    string p = startUpDB;
                    Log.Info(p);
                    ///Modified [19-10] to work with 64 Bit as well               
                    if (File.Exists(startUpDB))
                    {
                        Log.Info("Before: To check file is exist in startup path 2nd if i LodingWindow,success");
                        try
                        {
                            Log.Info("Before: copy Database to appdata in Loading (try)");
                            File.Copy(startUpDB, localAppDataDB);
                            Log.Info("After: copy Database to appdata in Loading (try), successful creation");
                        }
                        catch (Exception ex)
                        {
                            Log.Info("Info :before if copy Database to appdata in Loading : catch ");
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Log.Info("Before: inside if copy Database to appdata in Loading : (catch) ");
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA");
                                File.Copy(startUpDB, localAppDataDB);
                                Log.Info("After: inside if copy Database to appdata in Loading : (catch) ");
                                Log.Info("Before: inside copy database, successful creation");
                            } Log.Info("Info :After if copy Database to appdata in Loading : catch ");
                        }
                    }
                    else if (File.Exists(startUpDB64))   //for 64 Bit
                    {
                        try
                        {
                            Log.Info("Before: to copy database for 64bit try part LodingWindow");
                            File.Copy(startUpDB64, localAppDataDB);
                            Log.Info("After: to copy database for 64bit try part LodingWindow");
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path LodingWindow"))
                            {
                                Log.Info("Before: to copy database for 64bit catch part LodingWindow");
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA");
                                File.Copy(startUpDB64, localAppDataDB);
                                Log.Info("After: to copy database for 64bit catch part LodingWindow, successfully");
                            }
                        }
                    }
                }
                Log.Info("After: inside try part of copy database in LodingWindow");
            }
            catch { Log.Info("Info: inside catch part  Error ! LodingWindow"); };

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            #region Test
            //DatabaseAndQueries.SessionFactory.DatabaseType = DatabaseAndQueries.DBTYPE.SqlLite.ToString();
            //DatabaseAndQueries.SessionFactory.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA\CTE_Data.db";
            //DatabaseAndQueries.SessionFactory.AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().ToString();
            //CopyDataBase();
            //Inventory.Model.Setting _S = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>(x => x.Id == 1);
            //var rdiction = new ResourceDictionary();
            //rdiction.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _S.Accent));
            //Application.Current.Resources.MergedDictionaries.Add(rdiction);
            //var rdiction1 = new ResourceDictionary();
            //rdiction1.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _S.Theme));
            //Application.Current.Resources.MergedDictionaries.Add(rdiction1);
            //this.Close();
            #endregion
        }

        private void FlipView_Loaded(object sender, RoutedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            flipview.HideControlButtons();
            newslider.Start();
        }

        public void Sldier_Tick(object sender, EventArgs e)
        {
            if (count < (Slider.Items.Count - 1))
            {
                count++;
                Slider.SelectedIndex = count;
            }
            else
            {
                DatabaseAndQueries.SessionFactory.DatabaseType = DatabaseAndQueries.DBTYPE.SqlLite.ToString();
                DatabaseAndQueries.SessionFactory.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA\CTE_Data.db";
                DatabaseAndQueries.SessionFactory.AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().ToString();
                Log.Info("Before: call copy database in LodingWindow");
                CopyDataBase();
                Log.Info("After: call copy database,successfully in LodingWindow");
                Log.Info("Before: to get list of setting variable from DB in LoadingWindow");
                  IList<Setting> _Sp = Queries.GetAllByCondition<Setting>(x=>x.Id == 1);
                Log.Info("after: to get list of setting variable from DB in LoadingWindow");
                var rdiction = new ResourceDictionary();
                var rdiction1 = new ResourceDictionary();
                if (_Sp == null)
                {
                    Setting _setting = new Setting();
                    _setting.Accent = "Blue";
                    _setting.Theme = "Basedark";
                    Queries.Add<Setting>(_setting);
                    rdiction.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _setting.Accent));
                    Application.Current.Resources.MergedDictionaries.Add(rdiction);
                    rdiction1.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _setting.Theme));
                    Log.Info("After: to get list of setting variable from DB, Successfully in LoadingWindow");
                }
                else
                {
                    Log.Info("Before: to get list of setting variable from DB in LoadingWindow");
                    Inventory.Model.Setting _S = _Sp.First();//DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>(x => x.Id == 1);
                    Log.Info("After: to get list of setting variable from DB, Successfully in LoadingWindow");
                    rdiction.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _S.Accent));
                    Application.Current.Resources.MergedDictionaries.Add(rdiction);
                    rdiction1.Source = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", _S.Theme));
                    Log.Info("After: to get list of setting variable from DB, Successfully in LoadingWindow");
                }
                List<LoginUser> _lst = Queries.GetAllData<LoginUser>();
                if (_lst.Count == 0)
                {                  
                    new MasterPassword().Show();                 
                }
                else
                {
                    LoginUser _lu = _lst.First<LoginUser>();
                    if (_lu.Trial.Date <= DateTime.Now.Date)
                    {
                        new TrialExpireWindow().Show();           // send to expire window.
                        Log.Info("After: To show trail Expire window, in LoadingWindow");
                    }
                    else
                    {
                        Application.Current.Resources.MergedDictionaries.Add(rdiction);
                        Application.Current.Resources.MergedDictionaries.Add(rdiction1);
                        new Login().Show();
                    }
                }
                newslider.Stop();
                count = 0;
                this.Close();
            }
        }

        private void Slider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "Inventory Management";
                    break;
                case 1:
                    flipview.BannerText = "Service Provided By Stack Infotech";
                    break;
                case 2:
                    flipview.BannerText = "Inventory Management";
                    break;
            }
        }
    }
}
