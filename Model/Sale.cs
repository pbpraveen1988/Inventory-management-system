using FluentNHibernate.Mapping;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

using MahApps.Metro.Controls.Dialogs;
using DatabaseAndQueries;
using System.Windows;

namespace Inventory.Model
{                                                  //  Type = true;               // for sale record............
    public class Sale
    {
        public int Id { get; set; }
        public Products Product { get; set; }
        public Customer Customer { get; set; }
        public float Price { get; set; }
        public float TotalAmount { get; set; }
        public float Advance { get; set; }
        public DateTime SaleDate { get; set; }
        public float Quantity { get; set; }
        public bool Type { get; set; }
        public float RemainingBalance { get; set; }
        public int Billing { get; set; }
        public float Discount { get; set; }
        public float Vat { get; set; }
        public string PurchaseBill { get; set; }

        public Sale()
        {
            Product = new Products();
            SaleDate = DateTime.Now;
            Customer = new Customer();
        }

    }
    public class SaleMap : ClassMap<Sale>
    {
        public SaleMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            References(x => x.Product).Column("Product").Cascade.Delete();
            References(x => x.Customer).Column("CustomerId").Not.LazyLoad();
            Map(x => x.Billing).Column("bill");
            Map(x => x.Price).Column("Price");
            Map(x => x.SaleDate).Column("SaleDate");
            Map(x => x.TotalAmount).Column("TotalAmount");
            Map(x => x.Advance).Column("Advance");
            Map(x => x.Quantity).Column("Qty");
            Map(x => x.Type).Column("Type");
            Map(x => x.PurchaseBill).Column("Purchasebill");
            Map(x => x.Discount).Column("Discount");
            Map(x => x.Vat).Column("Vat");
            Map(x => x.RemainingBalance).Column("RemainingBalance");
            Not.LazyLoad();
            Table("Sales");
        }
    }
    public class SalesViewModel : ViewModelBase
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        ObservableCollection<Sale> sales;
        ObservableCollection<Sale> purchase;    
        private ObservableCollection<Customer> _customerlist;
    
        private ObservableCollection<Products> products;

        public ObservableCollection<Products> Products
        {
            get { return products; }
            set
            {
                products = value;
                NotifyPropertyChanged("Products");
            }
        }
        public ObservableCollection<Customer> Customerlist
        {
            get { return _customerlist; }
            set
            {
                _customerlist = value;
                NotifyPropertyChanged("Customers");
            }
        }

        private ICommand _SubmitPurchaseCommand;
        private ICommand _SubmitSalesCommand;

        private Sale _sale;
        public Sale Sale
        {
            get { return _sale; }
            set
            {
                _sale = value;
                //    NotifyPropertyChanged("Sale");
            }
        }
        public ICommand SubmitSalesCommand
        {
            get
            {
                if (_SubmitSalesCommand == null)
                {
                    _SubmitSalesCommand = new RelayCommand(param => this.SubmitAddSales(),
                        null);
                }
                return _SubmitSalesCommand;
            }
        }
        private async void SubmitAddSales()
        {
            Log.Info("Before: Process to add new sales record");

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            controller.SetIndeterminate();
            await TaskEx.Delay(1000);
            _sale.Type = true;              // for sale record............        


            if (!string.IsNullOrEmpty(_sale.Product.Pid.ToString()) && !string.IsNullOrEmpty(_sale.Customer.Cid.ToString())
               && !string.IsNullOrEmpty(_sale.Advance.ToString()) && !string.IsNullOrEmpty(_sale.Price.ToString())
                && !string.IsNullOrEmpty(_sale.Quantity.ToString()) && !string.IsNullOrEmpty(_sale.SaleDate.ToString()))
            {
                _sale.RemainingBalance = _sale.TotalAmount - _sale.Advance;

                Log.Info("Before: get list of available stock");
                Products _chk = Queries.GetDataByCondition<Products>(x => x.Pid == _sale.Product.Pid);
                Log.Info("After: get list of available stock");
                if (_chk.StockQty >= _sale.Quantity)
                {
                    _chk.StockQty = _chk.StockQty - _sale.Quantity;
                    DatabaseAndQueries.Queries.Add<Sale>(_sale);

                    Sales.Add(_sale);
                    Sale = new Sale();
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Sales Record Added Successfully", "");
                }
                else
                {
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("You have Insufficient Stock : " + _chk.StockQty, "");
                }
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            }
            else
            {
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please Fill all the fields", "");
            }
            Log.Info("After: Process to add new sales record, successfully");

        }
        public ICommand SubmitPurchaseCommand
        {
            get
            {
                if (_SubmitPurchaseCommand == null)
                {
                    _SubmitPurchaseCommand = new RelayCommand(param => this.SubmitAddPurchase(),
                        null);
                }

                return _SubmitPurchaseCommand;
            }

        }
        private async void SubmitAddPurchase()
        {
            Log.Info("Before: Process to add new Purchase record");
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (!string.IsNullOrEmpty(_sale.Product.Pid.ToString()) && !string.IsNullOrEmpty(_sale.Customer.Cid.ToString())
            && !string.IsNullOrEmpty(_sale.Advance.ToString()) && !string.IsNullOrEmpty(_sale.Price.ToString())
                && !string.IsNullOrEmpty(_sale.Quantity.ToString()) && !string.IsNullOrEmpty(_sale.SaleDate.ToString()))
            {

                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                if (_sale != null)
                {
                    _sale.RemainingBalance = _sale.TotalAmount - _sale.Advance;
                    DatabaseAndQueries.Queries.Add<Sale>(_sale);

                    Log.Info("Before: get list of available stock");
                    Products _chk = Queries.GetDataByCondition<Products>(x => x.Pid == _sale.Product.Pid);
                    _chk.StockQty = _chk.StockQty + _sale.Quantity;
                    Log.Info("After: get list of available stock");
                    Queries.Update<Products>(_chk);
                    Log.Info("After: updation of product stock, successfully");
                    Sales.Add(_sale);
                    Sale = new Sale();
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Sales Record Added Successfully", "");
                    Log.Info("After: Process to add new Purchase record");
                }

                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            }
            else
            {
                await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
            }

        }
        public ObservableCollection<Sale> Purchase
        {
            get { return purchase; }
            set
            {
                purchase = value;
                NotifyPropertyChanged("Sales");
            }
        }

        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
            set
            {
                sales = value;
                NotifyPropertyChanged("Sales");
            }
        }
      
        public SalesViewModel()
        {
            Log.Info("Before: Salesviewmodel ");

            Sale = new Model.Sale();
            Log.Info("Before:In Salesviewmodel, get all data of sales ");
            Sales = new ObservableCollection<Model.Sale>(DatabaseAndQueries.Queries.GetAllByConditionWithOrder<Sale, DateTime>(x => x.Type == true, x => x.SaleDate, true));
            Log.Info("After:In Salesviewmodel, get all data of sales,Successfully ");
            Log.Info("Before:In Salesviewmodel, get all data of purchase");
            Purchase = new ObservableCollection<Model.Sale>(DatabaseAndQueries.Queries.GetAllByConditionWithOrder<Sale, DateTime>(x => x.Type == false, x => x.SaleDate, true));
            Log.Info("After:In Salesviewmodel, get all data of purchase, Successfully");
            Log.Info("Before:In Salesviewmodel, get all data of Customer and Product");
       
            Customerlist = new AsyncObservableCollection<Model.Customer>(DatabaseAndQueries.Queries.GetAllDataWithOrder<Customer, int>(x => x.Cid, true));
            Products = new ObservableCollection<Model.Products>(DatabaseAndQueries.Queries.GetAllData<Products>());
            Log.Info("After:In Salesviewmodel, get all data of Customer and Product, Successfully");
            Log.Info("After: Salesviewmodel");
            Sales.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Sales_CollectionChanged);
        }
        void Sales_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Sales");
        }
    }
}
