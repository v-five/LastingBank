using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.CustomExceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        : base("Account with this Identity Number already exists.") { }

        public AccountNotFoundException(string message)
        : base(message) { }

        public AccountNotFoundException(string message, Exception inner)
        : base(message, inner) { }
    }
}