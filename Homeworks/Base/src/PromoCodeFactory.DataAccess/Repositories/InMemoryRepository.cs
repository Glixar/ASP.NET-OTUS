using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected List<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data?.ToList() ?? new List<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(Data.ToList());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id)!);
        }

        public Task<T> CreateAsync(T entity)
        {
            Data.Add(entity);
            return Task.FromResult(entity);
        }

        public void Update(T entity)
        {
            var indexById = Data.FindIndex(t => t.Id == entity.Id);
            if (indexById == -1)
            {
                throw new KeyNotFoundException($"Сущность {typeof(T).Name} с Id={entity.Id} не найдена для обновления.");
            }

            Data[indexById] = entity;
        }

        public void Delete(Guid id)
        {
            Data.RemoveAll(t => t.Id == id);
        }
    }
}