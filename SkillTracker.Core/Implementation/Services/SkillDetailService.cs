using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class SkillDetailService : ISkillDetailService
    {
        private readonly ISkillDetailRepository _skilldetailRepository;
        private readonly ICacheHelper _cacheHelper;
        public SkillDetailService(ISkillDetailRepository skilldetailRepository, ICacheHelper cacheHelper)
        {
            _skilldetailRepository = skilldetailRepository;
            _cacheHelper = cacheHelper;
        }
        public async Task<BaseResponse> DeleteSkillDetailById(int skilldetailId)
        {
            return await _skilldetailRepository.DeleteSkillDetailById(skilldetailId);
        }
        public async Task<SkillDetail> GetSkillDetailById(int skilldetailId)
        {
            return await _skilldetailRepository.GetSkillDetailById(skilldetailId);
        }
        public async Task<List<SkillDetail>> GetSkillDetail()
        {
            return await _skilldetailRepository.GetSkillDetail();
        }
        public async Task<BaseResponse> SaveSkillDetail(SkillDetail skilldetail)
        {
            return await _skilldetailRepository.SaveSkillDetail(skilldetail);
        }
        public async Task<BaseResponse> UpdateSkillDetail(SkillDetail skilldetail)
        {
            return await _skilldetailRepository.UpdateSkillDetail(skilldetail);
        }

        public List<SkillDetail> GetSkillDetailByCache()
        {
            return _cacheHelper.SkillDetail;
        }
    }
}
