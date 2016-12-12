using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
            {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }







        public string Email;
        public string Name;
        public string ContactNumber;
        public string Address;
        public string EmailValidation
        {
            get { return this.Email; }
            set
            {
                if (Equals(value, Email))
                {
                    return;
                }

                Email = value;
                NotifyPropertyChanged("EmailValidation");
            }
        }
        public string AddressValidation
        {
            get { return this.Address; }
            set
            {
                if (Equals(value, Address))
                {
                    return;
                }

                Address = value;
                NotifyPropertyChanged("AddressValidation");
            }
        }

        public string IntegerValidation
        {
            get { return this.ContactNumber; }
            set
            {
                if (Equals(value, ContactNumber))
                {
                    return;
                }
                ContactNumber = value;
                NotifyPropertyChanged("IntegerValidation");
            }
        }

        public string NameValidation
        {
            get { return this.Name; }
            set
            {
                if (Equals(value, Name))
                {
                    return;
                }

                Name = value;
                NotifyPropertyChanged("NameValidation");
            }
        }






        public string this[string columnName]
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EmailValidation))
                {

                    if (columnName == "EmailValidation")
                    {
                        if (!string.IsNullOrEmpty(this.EmailValidation))
                        {
                            if (!Regex.IsMatch(this.EmailValidation, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                return "Email Is not in valid format";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.NameValidation))
                {
                    if (columnName == "NameValidation")
                    {
                        if (!string.IsNullOrEmpty(this.NameValidation))
                        {
                            if (!Regex.IsMatch(this.NameValidation, "^[a-zA-Z ]+$", RegexOptions.IgnoreCase))
                            {
                                return "Only Alphabets Required";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.IntegerValidation))
                {

                    if (columnName == "IntegerValidation")
                    {
                        if (!string.IsNullOrEmpty(this.ContactNumber))
                        {
                            if (!Regex.IsMatch(this.ContactNumber, "^[0-9]*$", RegexOptions.IgnoreCase))
                            {
                                return "Only Numbers Required";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.AddressValidation))
                {
                    if (columnName == "AddressValidation")
                    {
                        if (string.IsNullOrEmpty(this.AddressValidation))
                        {
                            return "Please Enter ";
                        }
                    }
                }

                return null;
            }
        }



      











    }
}
