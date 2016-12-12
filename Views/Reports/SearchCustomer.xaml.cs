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
    /// Interaction logic for SearchCustomer.xaml
    /// </summary>
    public partial class SearchCustomer : Window
    { 
        public string Ldate, Fdate, customername,advance,balance;
      List<Sale> productlst;
      private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));      
     
        public SearchCustomer()
        {
            InitializeComponent();
        }
        public SearchCustomer(string Firstdate, string Lastdata, List<Sale> ProductSales, string customerName,string Advance,string Balance)
        {
            InitializeComponent();
            Ldate = Lastdata;
            Fdate = Firstdate;
            productlst = ProductSales;
            customername = customerName;
            advance = Advance;
            balance = Balance;     
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Before : To generate bill in UserControl_Loaded SearchCustomer");
            DataSet Ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Product");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Price");
            dt.Columns.Add("TotalAmount");
            dt.Columns.Add("Advance");
            dt.Columns.Add("SaleDate");
            dt.Columns.Add("Type");
            Log.Info("Before :To get data from salelist using foreach UserControl_Loaded SearchCustomer");
            foreach (Sale item in productlst)
            {
                DataRow _r = dt.NewRow();
                _r["Product"] = item.Product.Pname;
                _r["Qty"] = item.Quantity;
                _r["Price"] = item.Price;
                _r["TotalAmount"] = item.TotalAmount;
                _r["Advance"] = item.Advance;
                _r["SaleDate"] = item.SaleDate.ToString("dd-MM-yyyy");
                if(item.Type == true)
                {  
                    _r["Type"] = "Sale";
                }
                else
                {
                    _r["Type"] = "Purchase";
                }
                dt.Rows.Add(_r);
            }
            Ds.Tables.Add(dt);
            Log.Info("After :To get data from salelist using foreach UserControl_Loaded SearchCustomer");
            string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Apppath.IndexOf("\\bin\\Debug") != -1)
            {
                Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
            }
            Log.Info("Before : To get setting record from db in SearchCustomer");
            Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                               (x => x.Id == 1);
            string[] address = _Setting.Address.Split('$');
            Log.Info("After : To get setting record from db in SearchCustomer");
            /*Report Parameters*/
            /*image parametere*/
        //    ReportParameter imageParam = new ReportParameter("imageParam", "file:///" + Apppath + "\\Images\\logo.png");

            ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());
            ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper());
            ReportParameter addressLine2 = new ReportParameter("addressLine2", address[1].ToUpper());
            ReportParameter addressLine3 = new ReportParameter("addressLine3", address[2].ToUpper());
            ReportParameter addressLine4 = new ReportParameter("addressLine4", address[3].ToUpper());
            ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());


            ReportParameter DateFromParam = new ReportParameter("DateFromParam", Fdate);
            ReportParameter DateToParam = new ReportParameter("DateToParam", Ldate);
            ReportParameter CustomerParam = new ReportParameter("CustomerParam", customername);
            ReportParameter AdvanceParam = new ReportParameter("AdvanceParam", advance);
            ReportParameter BalanceParam = new ReportParameter("BalanceParam", balance);
          
            ReportDataSource reportDataSource = new ReportDataSource("ProductDataSet", dt);

            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.SearchCustomer.rdlc";
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] {//imageParam,
                companyName,addressLine1,addressLine2,addressLine3, addressLine4,contact,DateFromParam
                ,DateToParam,CustomerParam, AdvanceParam,BalanceParam});
            this.reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.RefreshReport();
            Log.Info("After :To generate bill in UserControl_Loaded SearchCustomer");
        }
    }
}
