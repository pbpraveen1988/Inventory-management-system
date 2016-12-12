using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using DatabaseAndQueries;
using log4net;

namespace Inventory.Model
{
    public class Products : ViewModelBase
    {
        int pid;
        string pname;
        DateTime date;
        float stockQty;
        string description;
        float mrp;

        public float Mrp
        {
            get { return mrp; }
            set
            {
                mrp = value;
                NotifyPropertyChanged("Mrp");
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged("Date");
            }
        }

        public int Pid
        {
            get { return pid; }
            set
            {
                pid = value;
                NotifyPropertyChanged("Pid");
            }
        }


        public string Pname
        {
            get { return pname; }
            set
            {
                pname = value;
                NotifyPropertyChanged("Pname");
            }
        }



        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public float StockQty
        {
            get { return stockQty; }
            set
            {
                stockQty = value;
                NotifyPropertyChanged("StockQty");
            }
        }


        public Products()
        {
            Date = DateTime.Now;

        }

    }
    public class ProductMap : ClassMap<Products>
    {
        public ProductMap()
        {
            Table("Product");
            Id(x => x.Pid).GeneratedBy.Increment();

            Map(x => x.Pname).Column("Pname");
            Map(x => x.Description).Column("Discription");
            Map(x => x.Date).Column("Date");
            Map(x => x.StockQty).Column("Stock");
            Map(x => x.Mrp).Column("Mrp");
            //   References(x => x.Unit).Column("Unit");
            Not.LazyLoad();
        }
    }

    public class ProductViewModel : ViewModelBase
    {
        ObservableCollection<Products> products;
        Products product;
        private ICommand _cmdupdate;
        private ICommand _cmd;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        public ObservableCollection<Products> Products
        {
            get { return products; }
            set
            {
                products = value;
                NotifyPropertyChanged("Products");
            }
        }

        public Products Product
        {
            get { return product; }
            set
            {
                product = value;
                NotifyPropertyChanged("Product");
            }
        }
        public ICommand Cmd
        {
            get
            {
                if (_cmd == null)
                {
                    _cmd = new RelayCommand(param => this.Submit(),
                        null);
                }
                return _cmd;
            }
        }
        private async void Submit()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            if (!string.IsNullOrEmpty(product.Pname) && !string.IsNullOrEmpty(product.Description))
            {
                Log.Info("Before: going to add new product into stock");

                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                Log.Info("Before: check product is exist or not in DB");
                if (Queries.IsExists<Products>(x => x.Pname.Contains(Product.Pname)))
                {
                    Log.Info("After: check product is exist or not in DB, successfully");
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Product Record Already Exist", "");
                }
                else
                {
                    Log.Info("Before: To add new Product record");
                    DatabaseAndQueries.Queries.Add<Products>(product);
                    Products.Add(product);
                    Product = new Products();
                    Log.Info("After: To add new Product record ,successfully");
                    await TaskEx.Delay(2000);
                    await controller.CloseAsync();
                    await mainWindow.ShowMessageAsync("Product Record Added Successfully", "");
                    Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                    Log.Info("After: going to add new product into stock");
                }
            }
            else
            {
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
            }
        }

        public ICommand Cmdupdate
        {
            get
            {
                if (_cmdupdate == null)
                {
                    _cmdupdate = new RelayCommand(param => this.SubmitUpdate(),
                        null);
                }
                return _cmdupdate;
            }
        }
        private async void SubmitUpdate()
        {
            Log.Info("Before: EditProduct record");

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Submit",
                NegativeButtonText = "Cancel"

            };
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            MessageDialogResult result = await mainWindow.ShowMessageAsync("Are you Sure want to Update ?", "",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                if (!string.IsNullOrEmpty(product.Pname) && !string.IsNullOrEmpty(product.Description) && !string.IsNullOrEmpty(product.Mrp.ToString()))
                {
                    var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                    controller.SetIndeterminate();
                    await TaskEx.Delay(1000);
                    Log.Info("Before: check product is exist or not in DB");
                    if (Queries.IsExists<Products>(x => x.Pname.Contains(Product.Pname)))
                    {
                        Log.Info("After: check product is exist or not in DB, successfully");
                        await TaskEx.Delay(2000);
                        await controller.CloseAsync();
                        await mainWindow.ShowMessageAsync("Product Record Already Exist", "");
                    }
                    else
                    {
                        Queries.Update<Products>(product);
                        await TaskEx.Delay(2000);
                        await controller.CloseAsync();
                        await mainWindow.ShowMessageAsync("Product Record Updated Successfully", "");
                        Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                        Log.Info("After: Edit Product record successfully inside the model name: Product");
                    }

                }
                else
                {
                    await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
                }
            }
        }
        public ProductViewModel()
        {
            Product = new Model.Products();
            Log.Info("Before: get list of all product in Productviewmodel");
            Products = new ObservableCollection<Model.Products>(DatabaseAndQueries.Queries.GetAllData<Products>());
            Products.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Products_CollectionChanged);
            Log.Info("Before: get list of all product in Productviewmodel,successfully");
        }
        void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Products");
        }
    }
}
