using System.ComponentModel.DataAnnotations;

namespace SkillTracker.Domain
{
    public class SkillDetail : BaseEntity
    {
        [Key]
        public int SkillDetailID { get; set; }
        public string SkillName { get; set; }
        public bool? IsTechnical { get; set; }
    }
}
