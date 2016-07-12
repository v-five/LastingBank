using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lasting.Bank.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Account AddAccount(int clientId, Account account);
        void RemoveAccount(int clientId, int accountId);
    }
}