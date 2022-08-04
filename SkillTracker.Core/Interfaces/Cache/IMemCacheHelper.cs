using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface IMemCacheHelper
    {
        Task<BaseResponse> AddUserProfile(UserSkill userSkill);
        Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile);
        Task<List<UserSkill>> GetAllUserProfile(SearchCrteria searchCrteria);
    }
}
