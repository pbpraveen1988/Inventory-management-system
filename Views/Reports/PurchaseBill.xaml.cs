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
using log4net;
using DatabaseAndQueries;

namespace Inventory.Views.Reports
{
    /// <summary>
    /// Interaction logic for PurchaseBill.xaml
    /// </summary>
    public partial class PurchaseBill : Window
    {
        public String billno;
        public string customer, add, contactno, sdate;
        public List<Sale> saleslst;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public PurchaseBill()
        {
            InitializeComponent();
        }
        public PurchaseBill(string bpno)
        {
            billno = bpno;
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Before: generate PurchaseBill report");
            Log.Info("Before: to get list of purchase record using billno. PurchaseBill report");

            List<Sale> saleslst = Queries.GetAllByCondition<Sale>(x => x.PurchaseBill == billno);
            Log.Info("After: to get list of purchase record using billno. PurchaseBill report, Successfully");
            float total = 0, remainingbalance = 0;
            if (saleslst.Count != 0)
            {
                Log.Info("Before: load datatable of PurchaseBill report");
                DataSet Ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("Product");
                dt.Columns.Add("Price");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Discount");
                dt.Columns.Add("Vat");
                dt.Columns.Add("TotalAmount");
                dt.Columns.Add("Advance");
                dt.Columns.Add("Type");
                dt.Columns.Add("RemainingBalance");
                foreach (Sale item in saleslst)
                {
                    DataRow _r = dt.NewRow();
                    _r["Product"] = item.Product.Pname;
                    _r["Price"] = item.Price;
                    _r["Qty"] = item.Quantity;
                    _r["Discount"] = item.Discount;
                    _r["Vat"] = item.Vat;
                    _r["TotalAmount"] = item.TotalAmount;
                    _r["Advance"] = item.Advance;
                    if (item.Type == true)
                    {
                        _r["Type"] = "Sale";
                    }
                    else
                    {
                        _r["Type"] = "Purchase";
                    }
                    total = total + item.TotalAmount;
                    remainingbalance = remainingbalance + item.RemainingBalance;
                    _r["RemainingBalance"] = item.RemainingBalance;
                    customer = item.Customer.Cname;
                    contactno = item.Customer.ContactNo.ToString();
                    add = item.Customer.Address.ToString();
                    sdate = item.SaleDate.ToString("dd-MM-yyyy");
                    dt.Rows.Add(_r);
                }
                Ds.Tables.Add(dt);
                Log.Info("Before: getting setting data of PurchaseBill report");


                string Apppath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                if (Apppath.IndexOf("\\bin\\Debug") != -1)
                {
                    Apppath = Apppath.Remove(Apppath.IndexOf("\\bin\\Debug"));
                }
                Inventory.Model.Setting _Setting = DatabaseAndQueries.Queries.GetDataByCondition<Inventory.Model.Setting>
                                                   (x => x.Id == 1);
                string[] address = _Setting.Address.Split('$');
                /*Report Parameters*/
                /*image parametere*/
          //      ReportParameter imageParam = new ReportParameter("imageParam", "file:///" + Apppath + "\\Images\\logo.png");
                 Log.Info("After: getting setting data of PurchaseBill report");

                ReportParameter companyName = new ReportParameter("comanyName", _Setting.CompanyName.ToUpper());

                ReportParameter addressLine1 = new ReportParameter("addressLine1", address[0].ToUpper());
                ReportParameter addressLine2 = new ReportParameter("addressLine2", address[1].ToUpper());
                ReportParameter addressLine3 = new ReportParameter("addressLine3", address[2].ToUpper());
                ReportParameter addressLine4 = new ReportParameter("addressLine4", address[3].ToUpper());
                ReportParameter contact = new ReportParameter("contact", _Setting.ContactNo.ToString());

                ReportParameter cusotmerName = new ReportParameter("customerName", customer.ToUpper());
                ReportParameter customerAddress = new ReportParameter("customerAddress", add.ToUpper());
                ReportParameter Customercontact = new ReportParameter("Customercontact", contactno.ToString());
                ReportParameter purchasedate = new ReportParameter("purchasedate", sdate);
                ReportParameter Billno = new ReportParameter("Billno", billno.ToString());
                ReportParameter Totalsumparam = new ReportParameter("Totalsumparam", total.ToString());

                ReportParameter Remainsumparam = new ReportParameter("Remainsumparam", remainingbalance.ToString());

                ReportDataSource reportDataSource = new ReportDataSource("BillDataset", dt);
                //    reportDataSource.Name = "CustomerReport";            // Name of the DataSet we set in .rdlc        

                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.ReportEmbeddedResource = "Inventory.Reports.SearchByCustomer.rdlc";
                this.reportViewer.LocalReport.SetParameters(new ReportParameter[] {// imageParam,
                    companyName,addressLine1,addressLine2,addressLine3,addressLine4,contact,cusotmerName,customerAddress,Customercontact,purchasedate,Billno
                                                                           ,Totalsumparam,Remainsumparam });
                this.reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.RefreshReport();
                Log.Info("After: generate PurchaseBill report");
            }
            else
            {
                this.Close();
                MessageBox.Show("Record not found");
            }
        }
   

    }
}
