using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.CustomExceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException()
        : base("Client with specified id doesn't exist.") { }

        public ClientNotFoundException(string message)
        : base(message) { }

        public ClientNotFoundException(string message, Exception inner)
        : base(message, inner) { }
    }
}