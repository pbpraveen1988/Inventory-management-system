
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using log4net.Appender;
namespace Inventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.Repository.Hierarchy.Hierarchy h =
                (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            foreach (IAppender a in h.Root.Appenders)
            {
                if (a is FileAppender)
                {
                    FileAppender fa = (FileAppender)a;
                    // Programmatically set this to the desired location here
                    string logFileLocation =  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\InventoryDBA\log.txt";

                    // Uncomment the lines below if you want to retain the base file name
                    // and change the folder name...
                    //FileInfo fileInfo = new FileInfo(fa.File);
                    //logFileLocation = string.Format(@"C:\MySpecialFolder\{0}", fileInfo.Name);

                    fa.File = logFileLocation;
                    fa.ActivateOptions();
                    break;
                }
            }

        }
        public App()
        {
            Log.Info("Application start");
            Log.Info("After CopyDatabase Function");





        }

    

        public bool DoHandle { get; set; }
        private void Application_DispatcherUnhandledException(object sender,
                       System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Uncaught Exception");
            if (e.Exception.InnerException != null)
            {
                MessageBox.Show(e.Exception.InnerException.Message, "Inner Exception");
            }
            MessageBox.Show(e.Exception.StackTrace.ToString(), "Stack Trace Exception");

            if (this.DoHandle)
            {
                //Handling the exception within the UnhandledException handler.
                MessageBox.Show(e.Exception.Message, "Exception Caught",
                                        MessageBoxButton.OK, MessageBoxImage.Error);

                


                e.Handled = true;
            }
            else
            {
                //If you do not set e.Handled to true, the application will close due to crash.
                MessageBox.Show("Application is going to close! ", "Uncaught Exception");
                e.Handled = false;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();


            AppDomain.CurrentDomain.UnhandledException +=
                         new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Log.Error(ex.Message);
            MessageBox.Show(ex.Message, "Uncaught Thread Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show(ex.InnerException.Message, "Uncaught Thread Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show(ex.InnerException.ToString(), "Uncaught Thread Exception",
                           MessageBoxButton.OK, MessageBoxImage.Error);

        }

    }
}
