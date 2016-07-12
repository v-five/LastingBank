using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.CustomExceptions
{
    public class AccountNotEmptyException : Exception
    {
        public AccountNotEmptyException()
        : base("This action is forbiden because your account is not empty.") { }

        public AccountNotEmptyException(string message)
        : base(message) { }

        public AccountNotEmptyException(string message, Exception inner)
        : base(message, inner) { }
    }
}