using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface IUserProfileRepository
    {
        Task<List<User>> GetUserProfile();
        Task<List<UserProfileDetail>> GetAllUserProfile(SearchCrteria searchCrteria);
        Task<BaseResponse> AddUserProfile(UserSkill userSkill);
        Task<BaseResponse> SaveUserProfile(User userprofile);
        Task<BaseResponse> UpdateUserProfile(User userprofile);
        Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile);
        Task<User> GetUserProfileByUserId(int userId);
        Task<BaseResponse> DeleteUserProfileById(int userprofileId);
    }
}
