using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.API
{
    [Route("api/skilldetail")]
    [ApiController]
    public class SkillDetailController : ControllerBase
    {
        private readonly ISkillDetailService _skilldetailService;
        public SkillDetailController(ISkillDetailService skilldetailService)
        {
            _skilldetailService = skilldetailService;
        }
        [HttpGet]
        public List<SkillDetail> GetSkillDetail() => _skilldetailService.GetSkillDetailByCache();
        [HttpPost]
        public async Task<BaseResponse> SaveSkillDetail(SkillDetail skilldetail) => await _skilldetailService.SaveSkillDetail(skilldetail);
        [HttpPut]
        public async Task<BaseResponse> UpdateSkillDetail(SkillDetail skilldetail) => await _skilldetailService.UpdateSkillDetail(skilldetail);
        [HttpGet("{skilldetailid}")]
        public async Task<SkillDetail> GetSkillDetailById(int skilldetailid) => await _skilldetailService.GetSkillDetailById(skilldetailid);
        [HttpDelete("{skilldetailid}")]
        public async Task<BaseResponse> DeleteSkillDetail(int skilldetailid) => await _skilldetailService.DeleteSkillDetailById(skilldetailid);
    }
}
