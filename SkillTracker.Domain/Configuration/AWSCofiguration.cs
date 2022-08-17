namespace SkillTracker.Domain
{
    public class AWSCofiguration
    {
        public string Profile { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string DynamoDBUrl { get; set; }

    }
}
