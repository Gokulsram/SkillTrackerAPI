using MediatR;
using SkillTracker.RabbitMQ;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class CreateUserSkillCommandHandler : IRequestHandler<CreateUserSkillCommand, BaseResponse>
    {
        private readonly IMemCacheHelper _memCacheHelper;
        private readonly IUserSkillUpdateSender _skillUpdateSender;

        public CreateUserSkillCommandHandler(
            IMemCacheHelper memCacheHelper,
            IUserSkillUpdateSender skillUpdateSender
        )
        {
            _memCacheHelper = memCacheHelper;
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
                return await _memCacheHelper.AddUserProfile(request.UserSkill);
            }
        }
    }
}
