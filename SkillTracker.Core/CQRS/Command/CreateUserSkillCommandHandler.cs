using MediatR;
using SkillTracker.RabbitMQ;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class CreateUserSkillCommandHandler : IRequestHandler<CreateUserSkillCommand, BaseResponse>
    {
        private readonly IUserProfileRepository _userprofileRepository;
        private readonly IUserSkillUpdateSender _skillUpdateSender;
        public CreateUserSkillCommandHandler(IUserProfileRepository userprofileRepository, IUserSkillUpdateSender skillUpdateSender)
        {
            _userprofileRepository = userprofileRepository;
            _skillUpdateSender = skillUpdateSender;
        }
        public async Task<BaseResponse> Handle(CreateUserSkillCommand request, CancellationToken cancellationToken)
        {
            if (request.IsRabbitMqPublish)
            {
                _skillUpdateSender.SendUserSkill(request.UserSkill);
                return new BaseResponse
                {
                    StatusCode = HttpStatusDetail.SuccessCode,
                    StatusDescription = "Successfully posted data in RabbitMQ Queue"
                };
            }
            else
            {
                return await _userprofileRepository.AddUserProfile(request.UserSkill);
            }
        }
    }
}
