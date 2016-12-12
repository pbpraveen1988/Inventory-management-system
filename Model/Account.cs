using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Model
{
  public class Account
    {
      public int id { get; set; }
        public Customer Customer { get; set; }
        public float PayAmount { get; set; }
        public string PayMode { get; set; }
        public DateTime PayDate { get; set; }
        public bool Pay_or_Recieve { get; set; }
        public Account()
        {
            Customer = new Customer();
            PayDate = DateTime.Now;
        }
    }
  public class AccountMap : ClassMap<Account>
  {
      public AccountMap()
      {
          Table("Account");
          Not.LazyLoad();
          Id(x => x.id).GeneratedBy.Increment();
          Map(x => x.PayAmount).Column("PaidAmount");
          Map(x => x.PayDate).Column("PayDate");
          Map(x => x.Pay_or_Recieve).Column("Pay_or_Recieve");
          Map(x => x.PayMode).Column("PayMode");
          References(x => x.Customer).Column("Customer").Not.LazyLoad();       
      }
  }
}
