﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Armoniza.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public bool Add(T entity)
        {
            if (entity is null)
            {
               return false;
            }
            dbSet.Add(entity);
            save();
            return true;
        }

        public bool Any(Expression<Func<T, bool>> filter, string? includePropierties = null)
        {
            return dbSet.Any(filter);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return dbSet.Where(filter).ToList();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return dbSet.FirstOrDefault(filter);
        }

        public bool Update(T entity)
        {
            if (entity is null)
            {
                return false;
            }
            dbSet.Update(entity);
            save();
            return true;
        }

        public void save()
        {
            _db.SaveChanges();
        }


    }
}
