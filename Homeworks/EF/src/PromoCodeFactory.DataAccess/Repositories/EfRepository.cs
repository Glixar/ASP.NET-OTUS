using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.DataContext;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DataBaseContext _datacontext;
        protected readonly DbSet<T> Data;

        public EfRepository(DataBaseContext datacontext)
        {
            _datacontext = datacontext;
            Data = _datacontext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Customer))
            {
                var q = _datacontext.Set<Customer>()
                    .Include(c => c.Preferences);
                return (IEnumerable<T>)await q.ToListAsync();
            }

            if (typeof(T) == typeof(Employee))
            {
                var q = _datacontext.Set<Employee>()
                    .Include(e => e.Role);
                return (IEnumerable<T>)await q.ToListAsync();
            }

            if (typeof(T) == typeof(PromoCode))
            {
                var q = _datacontext.Set<PromoCode>()
                    .Include(p => p.Customer)
                    .Include(p => p.Preference);
                return (IEnumerable<T>)await q.ToListAsync();
            }

            return await Data.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            if (typeof(T) == typeof(Customer))
            {
                var r = await _datacontext.Set<Customer>()
                    .Include(c => c.Preferences)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return (T)(object)r!;
            }

            if (typeof(T) == typeof(Employee))
            {
                var r = await _datacontext.Set<Employee>()
                    .Include(e => e.Role)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return (T)(object)r!;
            }

            if (typeof(T) == typeof(PromoCode))
            {
                var r = await _datacontext.Set<PromoCode>()
                    .Include(p => p.Customer)
                    .Include(p => p.Preference)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return (T)(object)r!;
            }

            return await Data.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await Data.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await Data.AddAsync(entity);
            await _datacontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            Data.Update(entity);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return;

            Data.Remove(entity);
            await _datacontext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var toRemove = await Data.Where(e => ids.Contains(e.Id)).ToListAsync();
            if (toRemove.Count == 0) return;

            Data.RemoveRange(toRemove);
            await _datacontext.SaveChangesAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            if (typeof(T) == typeof(Customer))
            {
                var r = await _datacontext.Set<Customer>()
                    .Include(c => c.Preferences)
                    .FirstOrDefaultAsync(predicate as Expression<Func<Customer, bool>>);
                return (T)(object)r!;
            }

            if (typeof(T) == typeof(Employee))
            {
                var r = await _datacontext.Set<Employee>()
                    .Include(e => e.Role)
                    .FirstOrDefaultAsync(predicate as Expression<Func<Employee, bool>>);
                return (T)(object)r!;
            }

            return await Data.FirstOrDefaultAsync(predicate);
        }
    }
}