using MediatR;
using Microsoft.Extensions.Options;
using SkillTracker.Domain;
using SkillTracker.RabbitMQ;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class CreateUserSkillCommandHandler : IRequestHandler<CreateUserSkillCommand, BaseResponse>
    {
        private readonly IUserProfileRepository _userprofileRepository;
        private readonly IMemCacheHelper _memCacheHelper;
        private readonly IUserSkillUpdateSender _skillUpdateSender;
        private readonly bool isInMemoryCache;

        public CreateUserSkillCommandHandler(
            IUserProfileRepository userprofileRepository,
            IMemCacheHelper memCacheHelper,
            IUserSkillUpdateSender skillUpdateSender,
            IOptions<CacheConfiguration> cacheConfiguration)
        {
            _userprofileRepository = userprofileRepository;
            _memCacheHelper = memCacheHelper;
            _skillUpdateSender = skillUpdateSender;
            isInMemoryCache = cacheConfiguration.Value.UseInMemoryCache;
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
                if (isInMemoryCache)
                    return await _userprofileRepository.AddUserProfile(request.UserSkill);
                else
                    return await _memCacheHelper.AddUserProfile(request.UserSkill);
            }
        }
    }
}
