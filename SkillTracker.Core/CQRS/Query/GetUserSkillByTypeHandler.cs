using MediatR;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class GetUserSkillByTypeHandler : IRequestHandler<GetUserSkillByTypeQuery, List<UserSkill>>
    {
        private readonly IMemCacheHelper _memCacheHelper;
        public GetUserSkillByTypeHandler(IMemCacheHelper memCacheHelper)
        {
            _memCacheHelper = memCacheHelper;
        }
        public async Task<List<UserSkill>> Handle(GetUserSkillByTypeQuery request, CancellationToken cancellationToken)
        {
            return _memCacheHelper.GetAllUserProfile(request.SearchCrteria);
        }
    }
}
