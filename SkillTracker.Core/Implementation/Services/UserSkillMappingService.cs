using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class UserSkillMappingService : IUserSkillMappingService
    {
        private readonly IUserSkillMappingRepository _userskillmappingRepository;
        public UserSkillMappingService(IUserSkillMappingRepository userskillmappingRepository)
        {
            _userskillmappingRepository = userskillmappingRepository;
        }
        public async Task<BaseResponse> DeleteUserSkillMappingById(int userskillmappingId)
        {
            return await _userskillmappingRepository.DeleteUserSkillMappingById(userskillmappingId);
        }
        public async Task<UserSkillMapping> GetUserSkillMappingById(int userskillmappingId)
        {
            return await _userskillmappingRepository.GetUserSkillMappingById(userskillmappingId);
        }
        public async Task<List<UserSkillMapping>> GetUserSkillMapping()
        {
            return await _userskillmappingRepository.GetUserSkillMapping();
        }
        public async Task<BaseResponse> SaveUserSkillMapping(UserSkillMapping userskillmapping)
        {
            return await _userskillmappingRepository.SaveUserSkillMapping(userskillmapping);
        }
        public async Task<BaseResponse> UpdateUserSkillMapping(UserSkillMapping userskillmapping)
        {
            return await _userskillmappingRepository.UpdateUserSkillMapping(userskillmapping);
        }
    }
}
