using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.API
{
    [Route("api/v1/engineer/")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userprofileService;
        public UserProfileController(IUserProfileService userprofileService)
        {
            _userprofileService = userprofileService;
        }
        //[HttpGet]
        //public async Task<List<User>> GetUserProfile() => await _userprofileService.GetUserProfile();
        //[HttpGet("admin/{criteria}/{criteriavalue}")]
        //public async Task<List<UserProfileDetail>> GetAllUserProfile(string criteria, string searchvalue)
        //{
        //    SearchCrteria searchCrteria = new SearchCrteria
        //    {
        //        AssociateID = string.Empty,
        //        Name = string.Empty,
        //        SkillName = string.Empty
        //    };
        //    switch (criteria.ToLower())
        //    {
        //        case "name":
        //            searchCrteria.Name = searchvalue;
        //            break;
        //        case "associateid":
        //            searchCrteria.AssociateID = searchvalue;
        //            break;
        //        default:
        //            searchCrteria.SkillName = searchvalue;
        //            break;
        //    }
        //    return await _userprofileService.GetAllUserProfile(searchCrteria);
        //}
        [HttpPost("add-profile")]
        public async Task<BaseResponse> SaveUserProfile(User userprofile) => await _userprofileService.SaveUserProfile(userprofile);
        [HttpPut("update-profile/{userid}")]
        public async Task<IActionResult> EditUserProfile(int userid, [FromBody] UserProfileDTO editUserProfile) =>
            await _userprofileService.EditUserProfile(userid, editUserProfile);
        //[HttpPut]
        //public async Task<BaseResponse> UpdateUserProfile(User userprofile) => await _userprofileService.UpdateUserProfile(userprofile);
        //[HttpGet("{userprofileid}")]
        //public async Task<User> GetUserProfileById(int userprofileid) => await _userprofileService.GetUserProfileById(userprofileid);
        //[HttpDelete("{userprofileid}")]
        //public async Task<BaseResponse> DeleteUserProfile(int userprofileid) => await _userprofileService.DeleteUserProfileById(userprofileid);
    }
}
