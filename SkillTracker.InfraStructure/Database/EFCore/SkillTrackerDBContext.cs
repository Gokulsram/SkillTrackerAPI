using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class SkillTrackerDBContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public SkillTrackerDBContext() : base()
        {

        }

        public SkillTrackerDBContext(DbContextOptions<SkillTrackerDBContext> options) : base(options)
        {

        }

        public virtual DbSet<SkillDetail> SkillDetail { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserSkillMapping> UserSkillMapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public int SaveChanges(int auditUserId)
        {

            var addedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
             .Where(p => p.State == EntityState.Added)
             .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
             .Where(p => p.State == EntityState.Modified)
             .Select(p => p.Entity);

            var now = DateTime.Now;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedTime = now;
                added.CreatedBy = added.CreatedBy > 0 ? added.CreatedBy : auditUserId;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.ModifiedTime = now;
                modified.ModifiedBy = auditUserId;
            }

            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(int auditUserId, CancellationToken cancellationToken = default)
        {
            var addedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
             .Where(p => p.State == EntityState.Added)
             .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
             .Where(p => p.State == EntityState.Modified)
             .Select(p => p.Entity);

            var now = DateTime.Now;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedTime = now;
                added.CreatedBy = added.CreatedBy > 0 ? added.CreatedBy : auditUserId;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.ModifiedTime = now;
                modified.ModifiedBy = auditUserId;
            }

            return (await base.SaveChangesAsync(true, cancellationToken));
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task BeginTransactionAsync()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadUncommitted);//.ConfigureAwait(false);  This should be synchronous 
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task CommitTransactionAsync(int auditUserId)
        {
            try
            {
                await SaveChangesAsync(auditUserId).ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                    _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadUncommitted);//.ConfigureAwait(false);  This should be synchronous 
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
