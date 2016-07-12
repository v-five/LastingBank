using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lasting.Bank.CustomExceptions;

namespace Lasting.Bank.DB
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankDbContext dbContext) : base(dbContext) { }

        public int Deposit(int accountId, int amount)
        {
            var account = base.Get(accountId);
            if (account == null)
                throw new AccountNotFoundException();

            account.Balance += amount;

            base.Save();
            return account.Balance;
        }

        public int Withdraw(int accountId, int amount)
        {
            var account = base.Get(accountId);
            if (account == null)
                throw new AccountNotFoundException();

            if (amount > account.Balance)
            {
                throw new Exception("The amount is too large.");
            }
            account.Balance -= amount;

            base.Save();
            return account.Balance;
        }
    }
}