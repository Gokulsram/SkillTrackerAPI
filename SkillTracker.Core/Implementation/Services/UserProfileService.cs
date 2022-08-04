using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userprofileRepository;
        public UserProfileService(IUserProfileRepository userprofileRepository)
        {
            _userprofileRepository = userprofileRepository;
        }
        public async Task<BaseResponse> DeleteUserProfileById(int userprofileId)
        {
            return await _userprofileRepository.DeleteUserProfileById(userprofileId);
        }
        public async Task<User> GetUserProfileByUserId(int userId)
        {
            return await _userprofileRepository.GetUserProfileByUserId(userId);
        }
        public async Task<List<User>> GetUserProfile()
        {
            return await _userprofileRepository.GetUserProfile();
        }
        public async Task<BaseResponse> SaveUserProfile(User userprofile)
        {
            return await _userprofileRepository.SaveUserProfile(userprofile);
        }
        public async Task<BaseResponse> UpdateUserProfile(User userprofile)
        {
            return await _userprofileRepository.UpdateUserProfile(userprofile);
        }

        public async Task<BaseResponse> EditUserProfile(int userid, List<Skills> editUserProfile)
        {
            return await _userprofileRepository.EditUserProfile(userid, editUserProfile);
        }

        public async Task<List<UserProfileDetail>> GetAllUserProfile(SearchCrteria searchCrteria)
        {
            return await _userprofileRepository.GetAllUserProfile(searchCrteria);
        }

        public async Task<BaseResponse> AddUserProfile(UserSkill userSkill)
        {
            return await _userprofileRepository.AddUserProfile(userSkill);
        }
    }
}
