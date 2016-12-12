using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using log4net;
using System.Windows.Input;

namespace Inventory.Model
{
    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<Sale> Sales { get; set; }
        public List<LanguageSpecificStringDictionary> Language { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Products> Product { get; set; }
        public List<Unit> Unit { get; set; }
        public List<Sale> purchase { get; set; }
        public Setting Setting { get; set; }
     
        public MainViewModel()
        {
           
            this.AccentColors = ThemeManager.Accents
                                             .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"]
                                                 as Brush }).ToList();

            this.AppThemes = ThemeManager.AppThemes
                                          .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"]
                                              as Brush,ColorBrush = a.Resources["WhiteColorBrush"] as Brush }).ToList();


         
         
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }


    


    }
}
