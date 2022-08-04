using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface ISkillDetailService
    {
        Task<List<SkillDetail>> GetSkillDetail();
        List<SkillDetail> GetSkillDetailByCache();
        Task<BaseResponse> SaveSkillDetail(SkillDetail skilldetail);
        Task<BaseResponse> UpdateSkillDetail(SkillDetail skilldetail);
        Task<SkillDetail> GetSkillDetailById(int skilldetailId);
        Task<BaseResponse> DeleteSkillDetailById(int skilldetailId);
    }
}
