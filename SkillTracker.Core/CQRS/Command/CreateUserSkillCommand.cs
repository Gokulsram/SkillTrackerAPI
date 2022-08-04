using MediatR;
using SkillTracker.Domain;

namespace SkillTracker.Core
{
    public class CreateUserSkillCommand : IRequest<BaseResponse>
    {
        public UserSkill UserSkill { get; set; }
        public bool IsRabbitMqPublish { get; set; }
    }
}
