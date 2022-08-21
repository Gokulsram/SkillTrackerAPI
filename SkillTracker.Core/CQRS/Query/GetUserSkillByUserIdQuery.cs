using MediatR;
using SkillTracker.Domain;

namespace SkillTracker.Core
{
    public class GetUserSkillByUserIdQuery : IRequest<UserSkill>
    {
        public int UserId { get; set; }
    }
}
