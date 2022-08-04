using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.API
{
    [Route("api/userskillmapping")]
    [ApiController]
    public class UserSkillMappingController : ControllerBase
    {
        private readonly IUserSkillMappingService _userskillmappingService;
        public UserSkillMappingController(IUserSkillMappingService userskillmappingService)
        {
            _userskillmappingService = userskillmappingService;
        }
        [HttpGet]
        public async Task<List<UserSkillMapping>> GetUserSkillMapping() => await _userskillmappingService.GetUserSkillMapping();
        [HttpPost]
        public async Task<BaseResponse> SaveUserSkillMapping(UserSkillMapping userskillmapping) => await _userskillmappingService.SaveUserSkillMapping(userskillmapping);
        [HttpPut]
        public async Task<BaseResponse> UpdateUserSkillMapping(UserSkillMapping userskillmapping) => await _userskillmappingService.UpdateUserSkillMapping(userskillmapping);
        [HttpGet("{userskillmappingid}")]
        public async Task<UserSkillMapping> GetUserSkillMappingById(int userskillmappingid) => await _userskillmappingService.GetUserSkillMappingById(userskillmappingid);
        [HttpDelete("{userskillmappingid}")]
        public async Task<BaseResponse> DeleteUserSkillMapping(int userskillmappingid) => await _userskillmappingService.DeleteUserSkillMappingById(userskillmappingid);
    }
}
