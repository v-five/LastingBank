using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.CustomExceptions
{
    public class SubscriptionNotFoundException : Exception
    {
        public SubscriptionNotFoundException()
        : base("You are not subscribed.") { }

        public SubscriptionNotFoundException(string message)
        : base(message) { }

        public SubscriptionNotFoundException(string message, Exception inner)
        : base(message, inner) { }
    }
}