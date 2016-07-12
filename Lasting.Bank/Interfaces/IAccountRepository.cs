using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lasting.Bank.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        int Deposit(int accountId, int amount);
        int Withdraw(int accountId, int amount);
    }
}
