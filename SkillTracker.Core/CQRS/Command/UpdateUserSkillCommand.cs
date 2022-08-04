using MediatR;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillTracker.Core
{
    public class UpdateUserSkillCommand : IRequest<BaseResponse>
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        public List<Skills> Skills { get; set; }

    }
}
