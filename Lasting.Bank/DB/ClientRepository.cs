using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;
using Lasting.Bank.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Lasting.Bank.DB
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private readonly IAccountRepository _accountRepository;

        public ClientRepository(BankDbContext dbContext) : base(dbContext)
        {
            _accountRepository = new AccountRepository(dbContext);
        }

        public override Client Add(Client client)
        {
            if (base.GetAll().Any(c => c.IdentityNumber.Equals(client.IdentityNumber)))
            {
                throw new ClientAlreadyExistsException();
            }

            this.AddAccount(client, new Account { Currency = Currency.EUR });
            this.AddAccount(client, new Account { Currency = Currency.RON });

            return base.Add(client);
        }

        public override Client Get(int id)
        {
            return DbContext.Set<Client>().Include(c => c.Accounts).FirstOrDefault(c => c.ID.Equals(id));
        }

        public override IEnumerable<Client> GetAll()
        {
            return DbContext.Set<Client>().Include(c => c.Accounts).ToList();
        }

        public override void Delete(int id)
        {
            var client = this.Get(id);
            if (client == null)
                throw new ClientNotFoundException();

            foreach(var account in client.Accounts)
            {
                this.RemoveAccount(client, account);
            }

            base.Delete(id);
            this.Save();
        }

        public Account AddAccount(int clientId, Account account)
        {
            return this.AddAccount(this.Get(clientId), account);
        }

        private Account AddAccount(Client client, Account account)
        {
            if (client == null)
                throw new ClientNotFoundException();

            client.Accounts = client.Accounts ?? new List<Account>();
            client.Accounts.Add(account);
            
            this.Save();
            return client.Accounts.Last();
        }

        public void RemoveAccount(int clientId, int accountId)
        {
            this.RemoveAccount(this.Get(clientId), _accountRepository.Get(accountId));
        }

        private void RemoveAccount(Client client, Account account)
        {
            if (client == null)
                throw new ClientNotFoundException();

            if (account == null)
                throw new AccountNotFoundException();

            if (account.Balance != 0)
            {
                throw new AccountNotEmptyException(
                    $"Account {account.ID} is not empty. Actual balance is {account.Balance} but should be 0.");
            }

            client.Accounts = client.Accounts.Where(a => a.ID != account.ID).ToList();
            base.Save();
            _accountRepository.Delete(account.ID);
            _accountRepository.Save();
        }
    }
}