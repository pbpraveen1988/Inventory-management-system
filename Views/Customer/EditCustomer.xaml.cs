using DatabaseAndQueries;
using Inventory.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MahApps.Metro.Controls.Dialogs;
using log4net;
namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Flyout
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        public EditCustomer()
        {
            Log.Info("Before Initialization EditCustomer");
            InitializeComponent();
            Log.Info("After Initialization EditCustomer");
        }

        private void customername_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = (sender as ComboBox).SelectedItem as Customer;
            if (p != null)
            {
                Cname.Text = p.Cname;
                cid.Text = p.Cid.ToString();
                CAddress.Text = p.Address;
                CEmail.Text = p.Email;
                ContactNumar.Text = p.ContactNo.ToString();
             }
            else
            {
                Cname.Text = string.Empty;
                cid.Text = string.Empty;
                CAddress.Text = string.Empty;
                CEmail.Text = string.Empty;
                ContactNumar.Text = string.Empty;
             }
        }

        private void editcustomerFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (this.editcustomerFlyout.IsOpen)
            {
                Log.Info("Before: To set data context in editcustomerFlyout_IsOpenChanged EditCustomer");
                this.DataContext = new CustomerViewModel();
                Log.Info("After: To set data context in editcustomerFlyout_IsOpenChanged EditCustomer");
            }
        }

        private void Cname_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Cname.Text, "^[a-zA-Z ]+$"))
            {
                Cname.Text = string.Empty;
            }
        }

        private void ContactNumar_KeyUp(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(ContactNumar.Text, "^[0-9]*$"))
            {
                ContactNumar.Text = string.Empty;
            }       
        }     


    }


}

