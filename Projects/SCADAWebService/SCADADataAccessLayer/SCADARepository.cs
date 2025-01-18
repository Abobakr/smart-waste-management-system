using DomainDataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace SCADADataAccessLayer
{
    public class SCADARepository<T> : ISCADARepository<T> where T : class,IEntityObjectState
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            using (var context = new SCADADbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new SCADADbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = new SCADADbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual void Add(params T[] items)
        {
            Update(items);
        }

        public virtual void Update(params T[] items)
        {
            using (var context = new SCADADbContext())
            {
                DbSet<T> dbSet = context.Set<T>();
                foreach (T item in items)
                {
                    dbSet.Add(item);
                    foreach (DbEntityEntry<IEntityObjectState> entry in context.ChangeTracker.Entries<IEntityObjectState>())
                    {
                        IEntityObjectState entity = entry.Entity;
                        entry.State = GetEntityState(entity.ObjectState);
                    }
                }
                context.SaveChanges();
            }
        }

        public virtual void Remove(params T[] items)
        {
            Update(items);
        }

        protected static EntityState GetEntityState(EntityObjectState entityObjectState)
        {
            switch (entityObjectState)
            {
                case EntityObjectState.Unchanged: 
                    return EntityState.Unchanged;
                
                case EntityObjectState.Added:
                    return EntityState.Added;
                
                case EntityObjectState.Modified:
                    return EntityState.Modified;
                
                case EntityObjectState.Deleted:
                    return EntityState.Deleted;
                
                default:
                    return EntityState.Detached;
            }
        }
    }
}