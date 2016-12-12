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
    /// Interaction logic for AllProduct.xaml
    /// </summary>
    public partial class AllProduct : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));      
     
        public AllProduct()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Before : To Generate report of all product in AllProduct");
         
            DataSet Ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Pid");
            dt.Columns.Add("Pname");
            dt.Columns.Add("Discription");
            dt.Columns.Add("Stock");

            Log.Info("Before : To get list of all product in AllProduct");         
            List<Products> _lst = Queries.GetAllData<Products>();
            Log.Info("After : To get list of all product in AllProduct");       
            foreach (Products item in _lst)
            {
                DataRow _r = dt.NewRow();
                _r["Pid"] = item.Pid;
                _r["Pname"] = item.Pname;
                _r["Discription"] = item.Description;             
                _r["Stock"] = item.StockQty;
                dt.Rows.Add(_r);
            }
            Ds.Tables.Add(dt);


            string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Apppath.IndexOf("\\bin\\Debug") != -1)
            {
                Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
            }
            Log.Info("Before : To get setting record from db in AllProduct");
            Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                               (x => x.Id == 1);
            Log.Info("After : To get setting record from db in AllProduct");
            string[] address = _Setting.Address.Split('$');
            /*Report Parameters*/
            /*image parametere*/
       //     ReportParameter imageParam = new ReportParameter("imageParam", "file:///" + Apppath + "\\Images\\logo.png");

            ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());

            ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper());
            ReportParameter addressLine2 = new ReportParameter("addressLine2", address[1].ToUpper());
            ReportParameter addressLine3 = new ReportParameter("addressLine3", address[2].ToUpper());
            ReportParameter addressLine4 = new ReportParameter("addressLine4", address[3].ToUpper());
            ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());

            ReportDataSource reportDataSource = new ReportDataSource("Product", dt);


            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.SearchAllProduct.rdlc";
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] {// imageParam,
                companyName,addressLine1,addressLine2,addressLine3,addressLine4,contact});
            this.reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.RefreshReport();
            Log.Info("After : To Generate report of all product in AllProduct");
         
        }
    }
}
