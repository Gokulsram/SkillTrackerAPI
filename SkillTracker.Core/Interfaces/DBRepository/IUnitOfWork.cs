using System;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(int auditUserId, CancellationToken cancellationToken = default);
        int SaveChanges(int auditUserId);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync(int auditUserId);
        Task CommitTransactionAsync();
        void RollbackTransaction();
    }
}
