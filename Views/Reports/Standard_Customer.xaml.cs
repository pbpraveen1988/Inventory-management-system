using DatabaseAndQueries;
using Inventory.Model;
using log4net;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for Standard_Customer.xaml
    /// </summary>
    public partial class Standard_Customer : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));
        public List<Sale> saleslst;
        public string Ldate, Fdate, customername, productname;
        public string firststring, secondstring, firstdatestring, seconddatestring;
        public Standard_Customer()
        {
            InitializeComponent();
        }
        public Standard_Customer(string Sdate, string CPname,string Remaining)
        {
            InitializeComponent();
            string[] DateString = Sdate.Split('$');
            string[] NameString = CPname.Split('$');
            if (DateString[0].ToUpper().Contains("/") && DateString[1].ToUpper() == "0")
            {
                firstdatestring = "Record From:";
                Fdate = DateString[0].ToUpper();
                seconddatestring = string.Empty;
                Ldate = string.Empty;
            }
            else if (DateString[0].ToUpper() == "0" && DateString[1].ToUpper() == "0")
            {
                firstdatestring = "Record upto:";
                Fdate = DateTime.Now.ToString("dd-MM-yyyy");
                seconddatestring = string.Empty;
                Ldate = string.Empty;
            }
            else
            {
                firstdatestring = "Record From:";
                seconddatestring = "To :";
                Fdate = DateString[0].ToUpper();
                Ldate = DateString[1].ToUpper();
            }

            if (NameString[1].ToUpper() == "0")
            {
                firststring = "Customer Name :";
                customername = NameString[0].ToUpper();
                secondstring = "Remaining_Balance:";
                productname = Remaining;

            }
            else if (NameString[0].ToUpper() == "0")
            {
                secondstring = string.Empty;
                productname = string.Empty;
                firststring = "Product :";
                customername = NameString[1].ToUpper();
            }
            else
            {
                firststring = "Customer Name :";
                secondstring = "Product :";
                customername = NameString[0].ToUpper();
                productname = NameString[1].ToUpper();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: load datatable of standard report in Standard_Customer");
            DataSet Ds = new DataSet();
            DataTable dt = new DataTable();
            float sale = 0, purcahse = 0, remainbalance = 0;
            dt.Columns.Add("BillNo");
            dt.Columns.Add("SaleDate");
            dt.Columns.Add("Product");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Price");
            //dt.Columns.Add("TotalAmount");
            //dt.Columns.Add("Advance");
            dt.Columns.Add("Sale");
            dt.Columns.Add("Purchase");

            foreach (Sale item in saleslst)
            {
                DataRow _r = dt.NewRow();
                if (item.Type)
                {
                    _r["BillNo"] = item.Billing;
                    _r["Sale"] = item.TotalAmount;
                    _r["Purchase"] = string.Empty;
                    sale = sale + item.TotalAmount;
                }
                else
                {
                    _r["BillNo"] = item.PurchaseBill;
                    _r["Sale"] = string.Empty;
                    _r["Purchase"] = item.TotalAmount;
                    purcahse = purcahse + item.TotalAmount;
                }
                //if (productname == "Hello")
                //{
                //    List<Sale> _lst = Queries.GetAllByCondition<Sale>(x => x.Customer.Cname.ToUpper() == customername);
                //    foreach (Sale items in _lst)
                //    {
                //        remainbalance = remainbalance + items.RemainingBalance;
                //    }
                //}
                _r["SaleDate"] = item.SaleDate.ToString("dd-MM-yyyy");
                _r["Product"] = item.Product.Pname;
                _r["Qty"] = item.Quantity;
                _r["Price"] = item.Price;


                // _r["TotalAmount"] = item.TotalAmount;
                //   _r["Advance"] = item.Advance;

                dt.Rows.Add(_r);
            }
            var p = remainbalance;
            Ds.Tables.Add(dt);
            Log.Info("Before: getting setting data of standard report in Standard_Customer");

            string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Apppath.IndexOf("\\bin\\Debug") != -1)
            {
                Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
            }
            Log.Info("Before : To get setting record from db in Standard_Customer");
            Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                               (x => x.Id == 1);
            string[] address = _Setting.Address.Split('$');
            Log.Info("After: To get setting record from db in Standard_Customer");
            /*Report Parameters*/
            /*image parametere*/

            ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());

            ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper() + ", " + address[1].ToUpper());
            ReportParameter addressLine2 = new ReportParameter("addressLine2", address[2].ToUpper() + ", " + address[3].ToUpper());
            ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());
            Log.Info("After: getting setting data of standard report, successfully in Standard_Customer");
            Log.Info("Before: set parameter of standard report in Standard_Customer");

            ReportParameter Nameparam = new ReportParameter("Nameparam", customername);
            ReportParameter Dateparam = new ReportParameter("Dateparam", Fdate);
            ReportParameter SecondDateparam = new ReportParameter("SecondDateparam", Ldate);
            //if (productname == "Hello")
            //{
            //    productname = remainbalance.ToString();
            //}

            ReportParameter prodparam = new ReportParameter("prodparam", productname);

            ReportParameter secondstringparam = new ReportParameter("secondstringparam", secondstring);
            ReportParameter firststringparam = new ReportParameter("firststringparam", firststring);
            ReportParameter datestringparam = new ReportParameter("datestringparam", firstdatestring);
            ReportParameter seconddatestringparam = new ReportParameter("seconddatestringparam", seconddatestring);
            ReportParameter salesumparam = new ReportParameter("salesumparam", sale.ToString());
            ReportParameter Purchasesumparam = new ReportParameter("Purchasesumparam", purcahse.ToString());


            ReportDataSource reportDataSource = new ReportDataSource("GridDataSet", dt);


            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.StandardReport.rdlc";
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] { companyName,addressLine1,addressLine2,
                                                                                contact,Nameparam,
                                                                               Dateparam,SecondDateparam,prodparam
                                                                               ,secondstringparam,firststringparam,
                                                                               datestringparam,seconddatestringparam,salesumparam,Purchasesumparam});
            Log.Info("After: set parameter of standard report in Standard_Customer");
            this.reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.RefreshReport();
            Log.Info("After:successful load datatable of standard report in Standard_Customer");
        }
    }
}
