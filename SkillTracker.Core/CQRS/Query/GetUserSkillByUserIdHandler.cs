using MediatR;
using SkillTracker.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class GetUserSkillByUserIdHandler : IRequestHandler<GetUserSkillByUserIdQuery, UserSkill>
    {
        private readonly IMemCacheHelper _memCacheHelper;
        public GetUserSkillByUserIdHandler(IMemCacheHelper memCacheHelper)
        {
            _memCacheHelper = memCacheHelper;
        }
        public async Task<UserSkill> Handle(GetUserSkillByUserIdQuery request, CancellationToken cancellationToken)
        {
            return _memCacheHelper.GetUserProfileById(request.UserId);
        }
    }
}
