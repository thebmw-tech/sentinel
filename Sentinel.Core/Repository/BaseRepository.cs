using Sentinel.Core.Repository.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository
{
    public class BaseRepository<TType> : IRepository<TType> where TType : BaseEntity<TType>, new()
    {
        protected readonly SentinelDatabaseContext dbContext;

        public BaseRepository(SentinelDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<TType> DbSet => dbContext.Set<TType>();

        public IQueryable<TType> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual TType Create(TType entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            DbSet.Add(entity);
            return entity;
        }

        public TType Delete(TType entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Remove(entity);

            return entity;
        }

        public void Delete(Expression<Func<TType, bool>> predicate)
        {
            var itemsToDelete = DbSet.Where(predicate).ToList();
            if (itemsToDelete.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(predicate)} did not match any records");
            }
            DbSet.RemoveRange(itemsToDelete);
        }

        public bool Exists(Expression<Func<TType, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public IQueryable<TType> Filter(Expression<Func<TType, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public TType Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TType Find(Expression<Func<TType, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public TType Update(TType entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Update(entity);

            return entity;
        }
    }
}