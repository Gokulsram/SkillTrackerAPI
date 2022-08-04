using SkillTracker.Domain;

namespace SkillTracker.Core
{
    public interface IRepository<T> where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
