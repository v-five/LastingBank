namespace Lasting.Bank
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BankDbContext : DbContext
    {
        public BankDbContext()
            : base("name=BankDbContext")
        {
        }
        
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
    }
}