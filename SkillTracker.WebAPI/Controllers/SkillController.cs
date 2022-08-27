using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace SkillTracker.WebAPI.Controllers
{
    [Route("api/skill")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSkills()
        {
            try
            {
                var skill = await _skillService.GetSkill();
                if (skill == null) return NotFound();
                return Ok(skill.OrderBy(x=>x.SkillId));
                //return new BaseResponse { StatusDescription = "Success" };
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);
            }
        }
        [HttpGet("{skillId}")]
        public async Task<IActionResult> GetById(int skillId)
        {
            var skill = await _skillService.GetSkillById(skillId);
            if (skill == null) return NotFound();
            return Ok(skill);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSkill(Skill skill)
        {
            return await _skillService.SaveSkill(skill);
        }
        [HttpDelete("{skillId}")]
        public async Task<IActionResult> DeleteSkill(int skillId)
        {
            return await _skillService.DeleteSkillById(skillId);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSkill(Skill skillRequest)
        {
            return await _skillService.UpdateSkill(skillRequest);
        }
    }
}
