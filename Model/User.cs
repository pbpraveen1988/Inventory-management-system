using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class User : INotifyPropertyChanged
    {
        private int _id;
        private string _firstName;
      

        public User()
        {
        }

        public virtual int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyOfPropertyChange("ID");
            }
        }
        public virtual string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange("FirstName");
            }
        }


        #region INotifyPropertyChanged Members
        public virtual event PropertyChangedEventHandler PropertyChanged;

        private void NotifyOfPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }


    public class ViewModelUser
    {

        PersonnelBusinessObject personnel;
        ObservableCollection<User> _Employee;
        public ViewModelUser()
        {
            personnel = new PersonnelBusinessObject();
        }
        public ObservableCollection<User> Employee
        {
            get
            {
                _Employee = new ObservableCollection<User>(personnel.GetEmployees());
                return _Employee;
            }
        }
    }

    public class PersonnelBusinessObject
    {
        public List<User> Employee { get; set; }
        public PersonnelBusinessObject()
        {
            Employee = DatabaseAndQueries.Queries.GetAllData<User>();
        }

        public List<User> GetEmployees()
        {
            return Employee = DatabaseAndQueries.Queries.GetAllData<User>();
        }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();
            Map(x => x.FirstName).Column("FirstName");        
            Table("User");
        }

    }
}
