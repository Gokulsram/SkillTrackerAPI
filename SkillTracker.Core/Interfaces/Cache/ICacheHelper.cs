using SkillTracker.Domain;
using System.Collections.Generic;

namespace SkillTracker.Core
{
    public interface ICacheHelper
    {
        List<SkillDetail> SkillDetail { get; set; }
    }
}
