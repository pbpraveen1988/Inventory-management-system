using log4net;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));     
        public DataTable datatable = new DataTable();
        float totalrem = 0, totalsale = 0, totalpurchase = 0;
        public AccountView()
        {
            InitializeComponent();
        }
        public AccountView(float s,float p,float r)
        {          
            InitializeComponent();
        //    datatable = d;
            totalsale = s;
            totalpurchase = p;
            totalrem = r;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet Ds = new DataSet();
            Ds.Tables.Add(datatable);
            Log.Info("Before: getting setting data of standard report in AccountView");

            string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Apppath.IndexOf("\\bin\\Debug") != -1)
            {
                Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
            }
            Log.Info("Before : To get setting record from db in AccountView");
            Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                               (x => x.Id == 1);
            string[] address = _Setting.Address.Split('$');
            Log.Info("After: To get setting record from db in AccountView");
            /*Report Parameters*/
            /*image parametere*/
            float p = 0;
            ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());

            ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper() + ", " + address[1].ToUpper());
            ReportParameter addressLine2 = new ReportParameter("addressLine2", address[2].ToUpper() + ", " + address[3].ToUpper());
            ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());
            ReportParameter salesumparam = new ReportParameter("salesumparam", totalsale.ToString());
            ReportParameter purchasesumparam = new ReportParameter("purchasesumparam", totalpurchase.ToString());
            ReportParameter remainsumparam = new ReportParameter("remainsumparam", totalrem.ToString());
                      
            Log.Info("After: getting setting data of standard report, successfully in AccountView");
            Log.Info("Before: set parameter of standard report in AccountView");
            
            ReportDataSource reportDataSource = new ReportDataSource("accountDS", datatable);


            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.AccountReport.rdlc";
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] { companyName,addressLine1,addressLine2,
                                                                                contact,salesumparam
                                                                                ,purchasesumparam,remainsumparam});
            Log.Info("After: set parameter of standard report in AccountView");               
            this.reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);   
            reportViewer.RefreshReport();
            datatable = null;
            Log.Info("After:successful load datatable of standard report in AccountView");
        }
    }
}
