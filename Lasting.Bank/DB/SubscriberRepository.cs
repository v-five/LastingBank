using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lasting.Bank.CustomExceptions;

namespace Lasting.Bank.DB
{
    public class SubscriberRepository : BaseRepository<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository(BankDbContext dbContext) : base(dbContext) { }


        public override void Delete(int identityNumber)
        {
            var subscriber = this.Find(s => s.IdentityNumber.Equals(identityNumber)).FirstOrDefault();
            if (subscriber == null)
                throw new SubscriptionNotFoundException();

            base.Delete(subscriber.ID);
            this.Save();
        }
    }
}