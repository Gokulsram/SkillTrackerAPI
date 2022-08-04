using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface IGenericEfRepository<T> where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        List<T> ListAll();
        Task<List<T>> ListAllAsync();
        List<T> List(ISpecification<T> spec);
        Task<List<T>> ListAsync(ISpecification<T> spec);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IList<T> entity);
        Task AddOrUpdateRangeAsync(IList<T> entities);
        T Update(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        T SaveEntity(T entity);
        Task<T> SaveEntityAsync(T entity);
    }
}
