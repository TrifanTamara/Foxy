﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public abstract class GenericRepo<T> : IGenericRepository<T> where T : class
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly DbSet<T> _entities;

        protected GenericRepo(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _entities = (databaseContext as DbContext)?.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<T> FindById(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual async Task Add(T entity)
        {
            //            (_databaseContext as DbContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            (_databaseContext as DatabaseContext).Attach(entity);
            await _entities.AddAsync(entity);
            await Save();
        }

        public virtual async Task AddWithoutSave(T entity)
        {
            //            (_databaseContext as DbContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            (_databaseContext as DatabaseContext).Attach(entity);
            await _entities.AddAsync(entity);
        }

        public virtual async Task Delete(Guid id)
        {
            var itemToRemove = await FindById(id);
            if (itemToRemove != null)
            {
                _entities.Remove(itemToRemove);
                await Save();
            }
        }

        public virtual async Task Edit(T entity)
        {
            _entities.Update(entity);
            await Save();
        }

        public virtual async Task Clear()
        {
            _entities.RemoveRange(_entities);
            await Save();
        }


        public virtual async Task Save()
        {
            await ((DbContext)_databaseContext).SaveChangesAsync();
        }
    }
}
