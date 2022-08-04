using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillTracker.Domain
{
    public class UserSkill
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name must have a minimum of 5 and a maximum of 30 characters", MinimumLength = 5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Associate ID is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Associate ID must have a minimum of 5 characters and a maximum of 30")]
        [RegularExpression("CTS.*", ErrorMessage = "Please enter a valid Associate ID must start with CTS")]
        public string AssociateID { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address with format of test@mail.com")]
        public string Email { get; set; }

        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "Mobile number must have 10 digits")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter Numberic only")]
        public long Mobile { get; set; }

        [Required]
        public List<Skills> SkillList { get; set; }
    }
}
