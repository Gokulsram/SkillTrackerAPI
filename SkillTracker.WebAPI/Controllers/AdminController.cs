using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillTracker.WebAPI.Controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{criteria}/{criteriavalue}")]
        public async Task<ActionResult<List<UserProfileDetail>>> GetAllUserProfile([FromRoute] string criteria, [FromRoute] string criteriavalue)
        {
            SearchCrteria searchCrteria = new SearchCrteria
            {
                AssociateID = string.Empty,
                Name = string.Empty,
                SkillName = string.Empty
            };
            string searchType = string.IsNullOrEmpty(criteria) ? string.Empty : criteria.Trim();
            switch (searchType)
            {
                case "name":
                    searchCrteria.Name = criteriavalue.Trim();
                    break;
                case "associateid":
                    searchCrteria.AssociateID = criteriavalue.Trim();
                    break;
                case "skillname":
                    searchCrteria.SkillName = criteriavalue.Trim();
                    break;
                default:
                    break;
            }
            try
            {
                return await _mediator.Send(new GetUserSkillByTypeQuery { SearchCrteria = searchCrteria });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
