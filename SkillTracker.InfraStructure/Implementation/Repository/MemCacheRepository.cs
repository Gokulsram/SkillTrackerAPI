using Enyim.Caching;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class MemCacheRepository : IMemCacheHelper
    {
        const string _uerProfile = "UserProfile";
        private readonly IMemcachedClient _memCache;
        public MemCacheRepository(IMemcachedClient memCache)
        {
            _memCache = memCache;
        }

        public async Task<BaseResponse> AddUserProfile(UserSkill userSkill)
        {
            try
            {
                var listProfiles = _memCache.Get<List<UserSkill>>(_uerProfile);
                if (listProfiles == null)
                    listProfiles = new List<UserSkill>();
                listProfiles.Add(userSkill);
                await _memCache.SetAsync(_uerProfile, listProfiles, 24 * 40 * 60);
                return new BaseResponse { StatusCode = 200, StatusDescription = "Success" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile)
        {
            throw new NotImplementedException();
        }

        public List<UserSkill> GetAllUserProfile(SearchCrteria searchCrteria)
        {
            try
            {
                return _memCache.Get<List<UserSkill>>(_uerProfile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

}
