using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.Models
{
    public class Subscriber
    {
        public int ID { get; set; }
        public int IdentityNumber { get; set; }
        public string CallbackUri { get; set; }
    }
}