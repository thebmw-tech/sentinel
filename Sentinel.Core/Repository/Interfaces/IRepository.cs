﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IRepository<TType> where TType : class
    {
        IQueryable<TType> All();
        bool Exists(Expression<Func<TType, bool>> predicate);
        IQueryable<TType> Filter(Expression<Func<TType, bool>> predicate);
        TType Find(params object[] keys);
        TType Find(Expression<Func<TType, bool>> predicate);
        TType Create(TType entity);
        TType Delete(TType t);
        void Delete(Expression<Func<TType, bool>> predicate);
        TType Update(TType t);
        int SaveChanges();
    }
}