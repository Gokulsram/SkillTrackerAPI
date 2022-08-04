namespace SkillTracker.Domain
{
    public class CacheConfiguration
    {
        public bool UseInMemoryCache { get; set; }
        public int CacheLifeTime { get; set; }
        public string CacheHost { get; set; }
        public string CachePort { get; set; }
    }
}
