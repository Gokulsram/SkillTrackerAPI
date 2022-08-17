using MediatR;
using SkillTracker.Domain;
using System.Collections.Generic;

namespace SkillTracker.Core
{
    public class GetUserSkillByTypeQuery : IRequest<List<UserProfileDetail>>
    {
        public SearchCrteria SearchCrteria { get; set; }
    }
}
