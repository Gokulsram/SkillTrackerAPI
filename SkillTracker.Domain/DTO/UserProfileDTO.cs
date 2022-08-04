using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillTracker.Domain
{
    public class UserProfileDTO
    {
        public List<Skills> SkillList { get; set; }
    }
    public class Skills
    {
        public string SkillName { get; set; }
        [Required(ErrorMessage = "Expertise Level should not be empty")]
        [Range(0, 20, ErrorMessage = "Expertise Level must be between 0 and 40")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter Numberic only")]
        public int ExpertiseLevel { get; set; }
    }
}
