using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Billing
    {
        public int Id { get; set; }

        public int BillNo { get; set; }
        public virtual DateTime Dateofgenerate { get; set; }
        public Billing()
        {
            Dateofgenerate = DateTime.Now;
        }
    }
    public class BillingMap : ClassMap<Billing>
    {
        public BillingMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Dateofgenerate).Column("Date");
            Map(x => x.BillNo).Column("BillNo");
            Table("Billing");
            Not.LazyLoad();


        }
    }
}
