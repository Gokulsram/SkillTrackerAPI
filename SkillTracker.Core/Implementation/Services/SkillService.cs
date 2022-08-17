using Microsoft.AspNetCore.Mvc;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        public async Task<IActionResult> DeleteSkillById(int skillId)
        {
            return await _skillRepository.DeleteSkillById(skillId);
        }
        public async Task<Skill> GetSkillById(int skillId)
        {
            return await _skillRepository.GetSkillById(skillId);
        }
        public async Task<List<Skill>> GetSkill()
        {
            return await _skillRepository.GetSkill();
        }
        public async Task<IActionResult> SaveSkill(Skill skill)
        {
            return await _skillRepository.SaveSkill(skill);
        }
        public async Task<IActionResult> UpdateSkill(Skill skill)
        {
            return await _skillRepository.UpdateSkill(skill);
        }
    }
}
