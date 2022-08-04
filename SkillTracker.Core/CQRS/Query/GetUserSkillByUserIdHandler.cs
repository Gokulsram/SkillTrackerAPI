using MediatR;
using SkillTracker.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class GetUserSkillByUserIdHandler : IRequestHandler<GetUserSkillByUserIdQuery, User>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public GetUserSkillByUserIdHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<User> Handle(GetUserSkillByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _userProfileRepository.GetUserProfileByUserId(request.UserId);
        }
    }
}
