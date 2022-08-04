using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface ISkillDetailRepository
    {
        Task<List<SkillDetail>> GetSkillDetail();
        Task<BaseResponse> SaveSkillDetail(SkillDetail skilldetail);
        Task<BaseResponse> UpdateSkillDetail(SkillDetail skilldetail);
        Task<SkillDetail> GetSkillDetailById(int skilldetailId);
        Task<BaseResponse> DeleteSkillDetailById(int skilldetailId);
    }
}
