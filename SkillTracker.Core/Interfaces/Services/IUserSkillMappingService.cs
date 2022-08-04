using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface IUserSkillMappingService
    {
        Task<List<UserSkillMapping>> GetUserSkillMapping();
        Task<BaseResponse> SaveUserSkillMapping(UserSkillMapping userskillmapping);
        Task<BaseResponse> UpdateUserSkillMapping(UserSkillMapping userskillmapping);
        Task<UserSkillMapping> GetUserSkillMappingById(int userskillmappingId);
        Task<BaseResponse> DeleteUserSkillMappingById(int userskillmappingId);
    }
}
