using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillTracker.Domain
{
    public class UserSkillMapping : BaseEntity
    {
        [Key]
        public int UserSkillMappingID { get; set; }
        [ForeignKey("SkillDetailID")]

        public int SkillDetailID { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }

        [Required]
        [Range(0, 20, ErrorMessage = "Expertise Level must be between 0 and 20")]
        public int ExpertiseLevel { get; set; }
    }
}
