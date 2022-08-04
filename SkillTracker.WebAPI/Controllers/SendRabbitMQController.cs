using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Threading.Tasks;

namespace SkillTracker.WebAPI.Controllers
{
    [Route("api/publish")]
    [ApiController]
    public class SendRabbitMQController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SendRabbitMQController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("publish-profile")]
        public async Task<ActionResult<BaseResponse>> PublishUserProfile(UserSkill userSkill)
        {
            try
            {
                return await _mediator.Send(new CreateUserSkillCommand
                {
                    UserSkill = userSkill,
                    IsRabbitMqPublish = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
