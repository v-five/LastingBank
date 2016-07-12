using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.TaxAgency.Models
{
    public class Client
    {
        public int ID { get; set; }
        public int IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public IList<Account> Accounts { get; set; }
    }
}