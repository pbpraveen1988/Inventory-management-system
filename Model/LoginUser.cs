using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class LoginUser
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual DateTime Trial { get; set; }
        public virtual DateTime LoginDate { get; set; }
        public LoginUser()
        {
            LoginDate = DateTime.Now;
        }
    }
    public class LoginUserMap : ClassMap<LoginUser>
    {
        public LoginUserMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.FirstName).Column("FirstName");
            Map(x => x.Password).Column("Password");
            Map(x => x.Role).Column("Role");
            Map(x => x.LoginDate).Column("LoginDate");
            Map(x => x.Trial).Column("Trial");
            Table("User");
            Not.LazyLoad();
        }
    }
}
