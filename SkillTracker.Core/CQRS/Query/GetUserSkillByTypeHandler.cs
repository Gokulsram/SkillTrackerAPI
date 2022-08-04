using MediatR;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class GetUserSkillByTypeHandler : IRequestHandler<GetUserSkillByTypeQuery, List<UserProfileDetail>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public GetUserSkillByTypeHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<List<UserProfileDetail>> Handle(GetUserSkillByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _userProfileRepository.GetAllUserProfile(request.SearchCrteria);
        }
    }
}
