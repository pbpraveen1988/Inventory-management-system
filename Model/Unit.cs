using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Unit
    {

        public virtual int Id { get; set; }
        public virtual string UnitName { get; set; }

    }


    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.UnitName).Column("UnitName");
            Not.LazyLoad();
            Table("Unit");
        }
    }

}
