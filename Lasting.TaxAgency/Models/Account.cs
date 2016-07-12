using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.TaxAgency.Models
{
    public class Account
    {
        public int ID { get; set; }
        public Currency Currency { get; set; }
        public int Balance { get; set; }
    }
}