using Lasting.Bank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace Lasting.Bank.DB
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        protected BankDbContext DbContext { get; private set; }

        protected BaseRepository(BankDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual TEntity Add(TEntity entity)
        {
            var result = DbContext.Set<TEntity>().Add(entity);
            this.Save();
            return result;
        }

        public virtual TEntity Get(int id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual void Delete(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                DbContext.Set<TEntity>().Remove(entity);
            }
        }

        public virtual void Save()
        {
            DbContext.SaveChanges();
        }
    }
}