using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> FindById(Guid id);
        Task Add(T entity);
        Task AddWithoutSave(T entity);
        Task Delete(Guid id);
        Task Edit(T entity);
        Task Clear();
        Task Save();
    }
}
