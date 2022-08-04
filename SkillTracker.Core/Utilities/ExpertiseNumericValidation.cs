using SkillTracker.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace SkillTracker.Core
{
    public class ExpertiseNumericValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userSkillMapping = (UserSkillMapping)validationContext.ObjectInstance;

            return IsNumeric(Convert.ToString(userSkillMapping.ExpertiseLevel))
                ? ValidationResult.Success
                : new ValidationResult("Expertise Level must be numeric value");
        }
        public static bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}
