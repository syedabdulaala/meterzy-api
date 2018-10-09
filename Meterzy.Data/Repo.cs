using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Data
{
    public interface IRepo<T> where T : Table
    {
        #region Property(ies)
        IQueryable<T> DataSet { get; }
        #endregion

        #region Async Method(s)
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPaginatedAsync(int start, int length);
        Task AddAsync(T obj);
        Task UpdateAsync(T obj);
        Task SoftDeleteAsync(T obj);
        Task DeleteAsync(T obj);
        Task<int> SaveAsync();
        #endregion

        #region Sync Method(s)
        List<T> GetAll();
        List<T> GetPaginated(int start, int length);
        void Add(T obj);
        void Update(T obj);
        void SoftDelete(T obj);
        void Delete(T obj);
        int Save();
        #endregion
    }
    public sealed class Repo<T> : IRepo<T> where T : Table
    {
        #region Variable(s)
        private readonly DbContext dbContext;
        #endregion

        #region Property(ies)
        public IQueryable<T> DataSet => dbContext.Set<T>();
        #endregion

        #region Constructor(s)
        public Repo(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Async Method(s)
        public async Task<List<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetPaginatedAsync(int start, int length)
        {
            return await dbContext.Set<T>()
                                  .Take(length)
                                  .Skip(0).ToListAsync();
        }

        public async Task AddAsync(T obj)
        {
            obj.CreatedOn = DateTime.UtcNow;
            await dbContext.AddAsync(obj);
        }

        public async Task UpdateAsync(T obj)
        {
            obj.LastModifiedOn = DateTime.UtcNow;
            await Task.Run(() => { dbContext.Update(obj); });
        }

        public async Task SoftDeleteAsync(T obj)
        {
            obj.Deleted = true;
            obj.LastModifiedOn = DateTime.UtcNow;
            await UpdateAsync(obj);
        }

        public async Task DeleteAsync(T obj)
        {
            await Task.Run(() => { dbContext.Remove(obj); });
        }

        public async Task<int> SaveAsync()
        {
            InnerSave();
            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region Sync Method(s)
        public List<T> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }

        public List<T> GetPaginated(int start, int length)
        {
            return dbContext.Set<T>()
                            .Take(length)
                            .Skip(0).ToList();
        }

        public void Add(T obj)
        {
            obj.CreatedOn = DateTime.UtcNow;
            dbContext.Add(obj);
        }

        public void Update(T obj)
        {
            obj.LastModifiedOn = DateTime.UtcNow;
            dbContext.Update(obj);
        }

        public void SoftDelete(T obj)
        {
            obj.Deleted = true;
            obj.LastModifiedOn = DateTime.UtcNow;
            Update(obj);
        }

        public void Delete(T obj)
        {
            dbContext.Remove(obj);
        }

        public int Save()
        {
            InnerSave();
            return dbContext.SaveChanges();
        }
        #endregion

        #region General Method(s)
        private void InnerSave()
        {
            var changedEnteries = dbContext.ChangeTracker.Entries<Table>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var entry in changedEnteries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                if (entry.State == EntityState.Modified)
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
            }
        }
        #endregion
    }
}
