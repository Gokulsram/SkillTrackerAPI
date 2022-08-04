using MediatR;
using SkillTracker.Domain;

namespace SkillTracker.Core
{
    public class GetUserSkillByUserIdQuery : IRequest<User>
    {
        public int UserId { get; set; }
    }
}
