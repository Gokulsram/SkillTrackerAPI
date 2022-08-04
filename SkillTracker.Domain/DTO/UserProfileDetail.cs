using System.Collections.Generic;

namespace SkillTracker.Domain
{
    public class UserProfileDetail
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string AssociateID { get; set; }
        public string Email { get; set; }
        public long Mobile { get; set; }
        public List<UserTechnicalSkill> UserTechnicalSkillDetails { get; set; }
    }
    public class UserTechnicalSkill
    {
        public int SkillDetailID { get; set; }
        public string SkillName { get; set; }
        public int ExpertiseLevel { get; set; }
        public bool? IsTechnical { get; set; }
    }
}
