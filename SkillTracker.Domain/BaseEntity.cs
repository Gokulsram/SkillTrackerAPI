using System;

namespace SkillTracker.Domain
{
    public class BaseEntity
    {
        public DateTime? CreatedTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
