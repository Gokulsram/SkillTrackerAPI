using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class UpdateUserSkillCommandHandler : IRequestHandler<UpdateUserSkillCommand, BaseResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UpdateUserSkillCommandHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<BaseResponse> Handle(UpdateUserSkillCommand request, CancellationToken cancellationToken)
        {
            return await _userProfileRepository.EditUserProfile(request.UserId, request.Skills);
        }
    }
}
