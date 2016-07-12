using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.CustomExceptions
{
    public class ClientAlreadyExistsException : Exception
    {
        public ClientAlreadyExistsException()
        : base("Client with this Identity Number already exists.") { }

        public ClientAlreadyExistsException(string message)
        : base(message) { }

        public ClientAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }
    }
}