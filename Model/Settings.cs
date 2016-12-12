using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Setting
    {
        public virtual int Id { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string logo { get; set; }
        public virtual string Accent { get; set; }
        public virtual string Theme { get; set; }
        public virtual string Language { get; set; }
        public virtual DateTime date { get; set; }
        public virtual string Address { get; set; }
        public virtual long ContactNo { get; set; }
        public Setting()
        {
            date = DateTime.Now;
        }
    }
    public class SettingMap : ClassMap<Setting>
    {
        public SettingMap()
        {
            Table("Setting");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.CompanyName).Column("CompanyName");
            Map(x => x.logo).Column("CompanyLogo");
            Map(x => x.Accent).Column("Accent");
            Map(x => x.Theme).Column("Theme");
            Map(x => x.Language).Column("Language");
            Map(x => x.date).Column("Date");
            Map(x => x.ContactNo).Column("ContactNo");        
            Map(x => x.Address).Column("Address");            
            Not.LazyLoad();
        }
    }
}
