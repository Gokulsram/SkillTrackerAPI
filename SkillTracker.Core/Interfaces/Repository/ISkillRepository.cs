using Microsoft.AspNetCore.Mvc;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface ISkillRepository
    {
        Task<List<Skill>> GetSkill();
        Task<IActionResult> SaveSkill(Skill skillId);
        Task<IActionResult> UpdateSkill(Skill skills);
        Task<Skill> GetSkillById(int skilldetailId);
        Task<IActionResult> DeleteSkillById(int skillId);
    }
}
