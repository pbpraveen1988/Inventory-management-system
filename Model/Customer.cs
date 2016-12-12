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
using MahApps.Metro.Controls;
using log4net;

namespace Inventory.Model
{
    public class Customer : IDataErrorInfo
    {
        public int Cid { get; set; }
        public string Cname { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Company {get; set;}       
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public virtual IList<Sale> Sales { get; set; }
        public virtual IList<Account> Account { get; set; }

        public Customer()
        {
            Sales = new List<Sale>();
            Account = new List<Account>();
            Date = DateTime.Now;
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {

                string result = string.Empty;
                switch (columnName)
                {
                    case "Cname": if (string.IsNullOrEmpty(Cname))
                        {
                            result = "Name is required!";
                        }
                        else
                        {
                            if (!Regex.IsMatch(this.Cname, "^[a-zA-Z ]+$", RegexOptions.IgnoreCase))
                            {
                                result = "Only Alphabets Required";
                                this.Cname = string.Empty;
                            }
                        } break;
                    case "Address": if (string.IsNullOrEmpty(Address)) result = "Please Enter the Address"; break;
                    case "Email": if (string.IsNullOrEmpty(Email)) { result = "Please Enter the Email Address"; }
                        else
                        {
                            if (!Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                return "Email Is not in valid format";
                            }
                        } break;
                    case "ContactNo": if (!string.IsNullOrEmpty(ContactNo))
                        {
                            if (!Regex.IsMatch(this.ContactNo.ToString(), "^[0-9]+$", RegexOptions.IgnoreCase))
                            {
                                result = "Only Number Required";
                            }
                        }
                        else
                        {
                            result = "Please Enter The Contact No.";
                        }
                        break;
                };
                return result;


            }
        }
    }
    /*Mapping Class */
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customer");
            Not.LazyLoad();
            Id(x => x.Cid).GeneratedBy.Increment();
            Map(x => x.Cname).Column("Cname");
            Map(x => x.Address).Column("Address");
            Map(x => x.ContactNo).Column("ContactNo");
            Map(x => x.Email).Column("Email");
            Map(x => x.Company).Column("Company");
            Map(x => x.Type).Column("Type");
            Map(x => x.Date).Column("Date");
            //   HasMany(x => x.Sales).KeyColumn("CustomerId").Cascade.Delete().NotFound.Ignore().Not.LazyLoad();
            HasMany(x => x.Account).KeyColumn("Customer").Cascade.All().Inverse().Not.LazyLoad()
              .NotFound.Ignore();

            HasMany(x => x.Sales).KeyColumn("CustomerId").Cascade.All().Inverse().Not.LazyLoad()
                .NotFound.Ignore();
        }
    }

    public class CustomerViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        private Customer _customer;
        private ObservableCollection<Customer> _CustomerList;
        private ObservableCollection<Account> _accounts;

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { _accounts = value;
            NotifyPropertyChanged("Accounts");
            }
        }

     
        private ICommand _SubmitCommand;
        private ICommand _UpdateCommand;
        private ICommand _DeleteCommand;


        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
                NotifyPropertyChanged("Customer");

            }
        }
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _CustomerList;
            }
            set
            {
                _CustomerList = value;
                NotifyPropertyChanged("Customers");
            }
        }

        public ICommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {
                    _SubmitCommand = new RelayCommand(param => this.Submit(),
                        null);
                }
                return _SubmitCommand;
            }
        }
        private async void Submit()
        {
            #region add
            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
            //if (!string.IsNullOrEmpty(_customer.Address) && !string.IsNullOrEmpty(_customer.Cname)
            //    && !string.IsNullOrEmpty(_customer.ContactNo))
            //{

            //    if (string.IsNullOrEmpty(_customer.Email))
            //    {
            //        _customer.Email = "not available";
            //    }
            //    Log.Info("Before: going to add new customer record");
            //    controller.SetIndeterminate();
            //    await TaskEx.Delay(1000);
            //    Log.Info("Before: Check customer contact no exist or not ");
            //    if (!DatabaseAndQueries.Queries.IsExists<Customer>(x => x.ContactNo == _customer.ContactNo))
            //    {
            //        Log.Info("After: Check customer contact no exist or not, successfully ");
            //        Log.Info("Before: Going to Add customer record ");
            //        DatabaseAndQueries.Queries.Add<Customer>(_customer);
            //        Log.Info("Before: Going to Add customer record, Successfully");
            //        Customers.Add(_customer);
            //        Log.Info("After: to add new customer record, Successfully");
            //        Customer = new Customer();
            //        await TaskEx.Delay(2000);
            //        await controller.CloseAsync();
            //        await mainWindow.ShowMessageAsync("Customer Record Added Successfully", "");
            //        Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            //    }
            //    else
            //    {
            //        await TaskEx.Delay(2000);
            //        await controller.CloseAsync();
            //        await mainWindow.ShowMessageAsync("Customer Contact number already exist", "");
            //    }
            //}
            //else
            //{
            //    await TaskEx.Delay(2000);
            //    await controller.CloseAsync();
            //    await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
            //}
            #endregion
        }
        public ICommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                {
                    _UpdateCommand = new RelayCommand(param => this.updatesubmit(),
                          null);
                }
                return _UpdateCommand;
            }
        }
        private async void updatesubmit()
        {
            var mySettings = new MetroDialogSettings()
         {
             AffirmativeButtonText = "Update",
             NegativeButtonText = "Cancel"
         };
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            MessageDialogResult result = await mainWindow.ShowMessageAsync("Are you Sure want to Update ?", "",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                if (!string.IsNullOrEmpty(_customer.Address) && !string.IsNullOrEmpty(_customer.Cname)
                                  && !string.IsNullOrEmpty(_customer.ContactNo))
                {
                    if (string.IsNullOrEmpty(_customer.Email))
                    {
                        _customer.Email = "not available";
                    }
                    else
                    {
                        var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                        if (!DatabaseAndQueries.Queries.IsExists<Customer>(x => x.ContactNo == _customer.ContactNo && x.Cid != _customer.Cid))
                        {
                            Log.Info("Before: going to update customer record");                          
                            controller.SetIndeterminate();
                            await TaskEx.Delay(1000);
                            DatabaseAndQueries.Queries.Update<Customer>(_customer);
                            Log.Info("After: going to update customer record");
                            await TaskEx.Delay(2000);
                            await controller.CloseAsync();
                            await mainWindow.ShowMessageAsync("Customer Record Updated Successfully", "");
                            Helpful.CloseAllFlyouts(mainWindow.Flyouts);
                        }
                        else
                        {
                            await TaskEx.Delay(2000);
                            await controller.CloseAsync();
                            await mainWindow.ShowMessageAsync("Customer Contact number already exist", "");
                        }
                    }
                }
                else
                {                 
                    await mainWindow.ShowMessageAsync("Please Fill the Complete Details", "");
                }
            }

        }
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new RelayCommand(param => this.deletesubmit(),
                          null);
                }
                return _DeleteCommand;
            }
        }
        private async void deletesubmit()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Delete",
                NegativeButtonText = "Cancel"

            };
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            MessageDialogResult result = await mainWindow.ShowMessageAsync("Are you Sure want to Delete ?", "",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                Log.Info("Before: going to Delete customer record");
                var controller = await mainWindow.ShowProgressAsync("Please wait...", "Process is Going on..!");
                controller.SetIndeterminate();
                await TaskEx.Delay(1000);
                DatabaseAndQueries.Queries.Delete<Customer>(Customer);
                Log.Info("After: to Delete customer record,Successfully");
                await TaskEx.Delay(2000);
                await controller.CloseAsync();
                await mainWindow.ShowMessageAsync("Customer Record Deleted Successfully", "");
                Helpful.CloseAllFlyouts(mainWindow.Flyouts);
            }
        }
        public CustomerViewModel()
        {
            Customer = new Customer();
            Accounts = new AsyncObservableCollection<Account>(DatabaseAndQueries.Queries.GetAllDataWithOrder<Account, int>(x => x.Customer.Cid, true));
            Customers = new AsyncObservableCollection<Model.Customer>(DatabaseAndQueries.Queries.GetAllDataWithOrder<Customer, int>(x => x.Cid, true));
            Customers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Customers_CollectionChanged);
        }
        void Customers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Customers");
        }
    }
}
