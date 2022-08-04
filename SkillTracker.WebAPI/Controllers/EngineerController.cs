using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.WebAPI.Controllers
{
    [Route("api/v1/engineer")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EngineerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("add-profile")]
        public async Task<ActionResult<BaseResponse>> AddUserProfile(UserSkill userSkill)
        {
            try
            {
                return await _mediator.Send(new CreateUserSkillCommand
                {
                    UserSkill = userSkill
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-profile/{userid}")]
        public async Task<ActionResult<BaseResponse>> EditUserProfile(int userid, [FromBody] List<Skills> skillList)
        {
            try
            {
                var user = await _mediator.Send(new GetUserSkillByUserIdQuery
                {
                    UserId = userid
                });

                if (user == null)
                {
                    return BadRequest($"No User found with this UserId - {userid}");
                }

                return await _mediator.Send(new UpdateUserSkillCommand
                {
                    Skills = skillList,
                    UserId = userid
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
