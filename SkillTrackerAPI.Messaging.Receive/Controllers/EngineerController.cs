using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System.Threading.Tasks;

namespace SkillTrackerAPI.Messaging.Receive.Controllers
{
    [Route("api/v1/engineer")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IUserProfileService _userprofileService;
        public EngineerController(IUserProfileService userprofileService)
        {
            _userprofileService = userprofileService;
        }
        [HttpPost("add-profile")]
        public async Task<BaseResponse> AddUserProfile(UserSkill userSkill) => await _userprofileService.AddUserProfile(userSkill);
    }
}
