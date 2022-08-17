using Microsoft.AspNetCore.Mvc;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface ISkillService
    {
        Task<List<Skill>> GetSkill();
        Task<IActionResult> SaveSkill(Skill skill);
        Task<IActionResult> UpdateSkill(Skill skill);
        Task<Skill> GetSkillById(int skillId);
        Task<IActionResult> DeleteSkillById(int skillId);
    }
}
