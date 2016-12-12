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

using System.Data;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using Inventory.Model;
using DatabaseAndQueries;
using log4net;

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for AllCustomer.xaml
    /// </summary>
    public partial class AllCustomer : Window
    {
      //  public List<Customer> _lst;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));      
        public AllCustomer()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Before : To generate report in AllCustomer");
            DataSet Ds = new DataSet();           
            DataTable dt = new DataTable();
            dt.Columns.Add("Cid");
            dt.Columns.Add("Cname");
            dt.Columns.Add("Address");
            dt.Columns.Add("ContactNo");
            dt.Columns.Add("Email");
            dt.Columns.Add("Date");
            Log.Info("Before : To get List of all customer in AllCustomer");
          List<Customer> _lst = Queries.GetAllData<Customer>();
          Log.Info("After : To get List of all customer in AllCustomer");
          foreach(Customer  item in _lst )
          {
              DataRow _r = dt.NewRow();
              _r["Cid"] = item.Cid;
              _r["Cname"] = item.Cname;
              _r["Address"] = item.Address;
              _r["ContactNo"] = item.ContactNo;
              _r["Email"] = item.Email;
              _r["Date"] = item.Date;
              dt.Rows.Add(_r);
          }
            Ds.Tables.Add(dt);                               
            
            string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Apppath.IndexOf("\\bin\\Debug") != -1)
            {
                Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
            }
            Log.Info("Before : To get setting record from db in AllCustomer");
            Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                               (x => x.Id == 1);
            Log.Info("After : To get setting record from db in AllCustomer");
            string[] address = _Setting.Address.Split('$');
            /*Report Parameters*/
            /*image parametere*/
         //   ReportParameter imageParam = new ReportParameter("imageParam", "file:///" + Apppath + "\\Images\\logo.png");

            ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());

            ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper());
            ReportParameter addressLine2 = new ReportParameter("addressLine2", address[1].ToUpper());
            ReportParameter addressLine3 = new ReportParameter("addressLine3", address[2].ToUpper());
            ReportParameter addressLine4 = new ReportParameter("addressLine4", address[3].ToUpper());
            ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());

            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);                    
         
            
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.SearchAllCustomer.rdlc";
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] {// imageParam,
                companyName,addressLine1,addressLine2,addressLine3,addressLine4,contact});
            this.reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.RefreshReport();
            Log.Info("After : To generate report in AllCustomer");
        }
    }
}
