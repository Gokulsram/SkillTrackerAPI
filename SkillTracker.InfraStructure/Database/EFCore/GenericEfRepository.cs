using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SkillTracker.Core;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class GenericEfRepository<T> : IGenericEfRepository<T> where T : BaseEntity
    {
        protected readonly SkillTrackerDBContext _dbContext;

        public GenericEfRepository(SkillTrackerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _dbContext;
            }
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T GetSingleBySpec(ISpecification<T> spec)
        {
            return List(spec).FirstOrDefault();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public List<T> ListAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public List<T> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
        .Aggregate(_dbContext.Set<T>().AsQueryable(),
          (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
        .Aggregate(queryableResultWithIncludes,
          (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult
              .Where(spec.Criteria)
              .ToList();
        }
        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
        .Aggregate(_dbContext.Set<T>().AsQueryable(),
          (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
        .Aggregate(queryableResultWithIncludes,
          (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
              .Where(spec.Criteria)
              .ToListAsync();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (_dbContext.Entry(entity) != null)
                _dbContext.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public T SaveEntity(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<T> UpdateAsync(T entity)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<T> SaveEntityAsync(T entity)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Bulk Insert Extension method
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IList<T> entities)
        {
            await _dbContext.BulkInsertAsync<T>(entities, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 4000 });
        }

        /// <summary>
        /// Bulk Upsert Extension method
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddOrUpdateRangeAsync(IList<T> entities)
        {
            await _dbContext.BulkInsertOrUpdateAsync<T>(entities, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 4000 });
        }

    }
}
