using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.API.Controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserProfileService _userprofileService;
        public AdminController(IUserProfileService userprofileService)
        {
            _userprofileService = userprofileService;
        }
        [HttpGet("{criteria}/{criteriavalue}")]
        public async Task<List<UserProfileDetail>> GetAllUserProfile([FromRoute] string criteria, [FromRoute] string criteriavalue)
        {
            SearchCrteria searchCrteria = new SearchCrteria
            {
                AssociateID = string.Empty,
                Name = string.Empty,
                SkillName = string.Empty
            };
            string searchType = string.IsNullOrEmpty(criteria) ? string.Empty : criteria.Trim();
            switch (searchType)
            {
                case "name":
                    searchCrteria.Name = criteriavalue.Trim();
                    break;
                case "associateid":
                    searchCrteria.AssociateID = criteriavalue.Trim();
                    break;
                case "skillname":
                    searchCrteria.SkillName = criteriavalue.Trim();
                    break;
                default:
                    break;
            }
            return await _userprofileService.GetAllUserProfile(searchCrteria);
        }
    }
}
