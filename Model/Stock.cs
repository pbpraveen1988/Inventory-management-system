using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Stock
    {
        public virtual int Sid { get; set; }
        public virtual Products Product { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual float Qty { get; set; }
        public Stock()
        {
            Product = new Products();
            Date = DateTime.Now;
        }
    }

    public class StockMap : ClassMap<Stock>
    {
        public StockMap()
        {
            Table("Stock");
            Id(x => x.Sid).GeneratedBy.Increment();
            Map(x => x.Qty).Column("Quantity");
            References(x => x.Product).Column("Product").Cascade.Delete();       
         //   Map(x => x.Description).Column("Discription");
            Map(x => x.Date).Column("Date");
            Not.LazyLoad();
        }
    }
}
