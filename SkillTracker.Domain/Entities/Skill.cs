using Amazon.DynamoDBv2.DataModel;

namespace SkillTracker.Domain
{
    [DynamoDBTable("Skill")]
    public class Skill
    {
        [DynamoDBHashKey("SkillId")]
        public int? SkillId { get; set; }
        [DynamoDBProperty("SkillName")]
        public string? SkillName { get; set; }
        [DynamoDBProperty("IsTechnical")]
        public bool? IsTechnical { get; set; }
    }
}
