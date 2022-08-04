using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class MemCacheRepository : IMemCacheHelper
    {
        const string _uerProfile = "SkillTracker.UserProfile";
        private readonly ICacheProvider _cacheProvider;
        public MemCacheRepository(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<BaseResponse> AddUserProfile(UserSkill userSkill)
        {
            try
            {
                var listProfiles = await _cacheProvider.GetItemAsync<List<UserSkill>>(_uerProfile);
                listProfiles.Add(userSkill);
                await _cacheProvider.AddItemAsync(_uerProfile, listProfiles);
                return new BaseResponse { StatusCode = 200, StatusDescription = "Success" };
            }
            catch
            {
                return new BaseResponse { StatusCode = 400, StatusDescription = "Failure" };
            }
        }

        public Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserSkill>> GetAllUserProfile(SearchCrteria searchCrteria)
        {
            try
            {
                return await _cacheProvider.GetItemAsync<List<UserSkill>>(_uerProfile);
            }
            catch
            {
                return new List<UserSkill> { };
            }
        }
    }
}
