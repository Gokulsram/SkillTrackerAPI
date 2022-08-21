using Enyim.Caching;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class MemCacheRepository : IMemCacheHelper
    {
        const string _uerProfile = "UserProfile";
        private readonly IMemcachedClient _memCache;
        private readonly ISkillRepository _skillRepository;
        public MemCacheRepository(IMemcachedClient memCache, ISkillRepository skillRepository)
        {
            _memCache = memCache;
            _skillRepository = skillRepository;
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

        public async Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile)
        {
            var userProfileCache = _memCache.Get<List<UserSkill>>(_uerProfile);
            UserSkill userProfile = userProfileCache.Where(x => x.UserID == userId).FirstOrDefault();
            foreach (var skill in editUserProfile)
            {
                userProfile.SkillList.Add(skill);
            }
            userProfileCache.Remove(userProfile);
            userProfileCache.Add(userProfile);

            await _memCache.SetAsync(_uerProfile, userProfileCache, 24 * 40 * 60);
            return new BaseResponse { StatusCode = 200, StatusDescription = "Success" };
        }

        public List<UserProfileDetail> GetAllUserProfile(SearchCrteria searchCrteria)
        {
            try
            {

                List<UserSkill> lstUserProfile = _memCache.Get<List<UserSkill>>(_uerProfile);
                List<Skill> lstSkills = _skillRepository.GetSkill().Result;

                var userProfileDetails =
                    (from user in lstUserProfile.Where(x => x.Name.Contains(searchCrteria.Name) && x.AssociateID.Contains(searchCrteria.AssociateID))
                     select new UserProfileDetail
                     {
                         AssociateID = user.AssociateID,
                         Name = user.Name,
                         Email = user.Email,
                         Mobile = user.Mobile,
                         UserID = user.UserID,
                         UserTechnicalSkillDetails = (from skillmap in user.SkillList
                                                      join skill in lstSkills on skillmap.SkillName equals skill.SkillName
                                                      select new UserTechnicalSkill
                                                      {
                                                          SkillName = skill.SkillName,
                                                          ExpertiseLevel = skillmap.ExpertiseLevel,
                                                          IsTechnical = skill.IsTechnical
                                                      }).Where(x => x.SkillName.Contains(searchCrteria.SkillName) && x.ExpertiseLevel > 10).
                                                                                                                               OrderByDescending(x => x.ExpertiseLevel).ToList()

                     }).ToList();

                return userProfileDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserSkill GetUserProfileById(int UserId)
        {
            var userProfileCache = _memCache.Get<List<UserSkill>>(_uerProfile);
            return userProfileCache.Where(x => x.UserID == UserId).FirstOrDefault();
        }
    }

}
