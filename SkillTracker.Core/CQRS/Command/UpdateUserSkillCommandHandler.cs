using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class UpdateUserSkillCommandHandler : IRequestHandler<UpdateUserSkillCommand, BaseResponse>
    {
        private readonly IMemCacheHelper _memCacheHelper;
        public UpdateUserSkillCommandHandler(IMemCacheHelper memCacheHelper)
        {
            _memCacheHelper = memCacheHelper;
        }

        public async Task<BaseResponse> Handle(UpdateUserSkillCommand request, CancellationToken cancellationToken)
        {
            return await _memCacheHelper.EditUserProfile(request.UserId, request.Skills);
        }
    }
}
