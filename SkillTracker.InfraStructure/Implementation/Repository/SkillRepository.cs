using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class SkillRepository : ISkillRepository
    {
        private readonly IDynamoDBContext _dbContext;
        public SkillRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> DeleteSkillById(int skillId)
        {
            try
            {
                var skill = await _dbContext.LoadAsync<Skill>(skillId);
                if (skill == null) return new NotFoundObjectResult(new { message = "404 Not Found", currentDate = DateTime.Now });
                await _dbContext.DeleteAsync(skill);
                return new OkObjectResult(new { message = HttpStatusDetail.SuccessCode, currentDate = DateTime.Now });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in Delete SkillById()");
                throw ex;
            }

        }
        public async Task<Skill> GetSkillById(int skillId)
        {
            try
            {
                return await _dbContext.LoadAsync<Skill>(skillId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Skill>> GetSkill()
        {
            try
            {
                return await _dbContext.ScanAsync<Skill>(default).GetRemainingAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> SaveSkill(Skill skills)
        {
            try
            {
                var skill = await _dbContext.LoadAsync<Skill>(skills.SkillId);
                if (skill != null) return new BadRequestObjectResult(new { message = $"Skill with Id {skills.SkillId} Already Exists", currentDate = DateTime.Now });
                await _dbContext.SaveAsync(skills);
                return new OkObjectResult(new { message = HttpStatusDetail.SuccessCode, currentDate = DateTime.Now });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in SaveSkill()");
                throw ex;
            }
        }
        public async Task<IActionResult> UpdateSkill(Skill skills)
        {
            try
            {
                var skill = await _dbContext.LoadAsync<Skill>(skills.SkillId);
                if (skill == null) return new NotFoundObjectResult(new { message = "404 Not Found", currentDate = DateTime.Now });
                await _dbContext.SaveAsync(skills);
                return new OkObjectResult(new { message = HttpStatusDetail.SuccessCode, currentDate = DateTime.Now });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in UpdateSkill()");
                throw ex;
            }

        }
    }
}
