using MediatR;
using SkillTracker.Domain;
using System.Collections.Generic;

namespace SkillTracker.Core
{
    public class GetUserSkillByTypeQuery : IRequest<List<UserSkill>>
    {
        public SearchCrteria SearchCrteria { get; set; }
    }
}
