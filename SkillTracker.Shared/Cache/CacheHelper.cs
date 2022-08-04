using SkillTracker.Core;
using SkillTracker.Domain;
using System.Collections.Generic;

namespace SkillTracker.Shared
{
    public class CacheHelper : ICacheHelper
    {
        const string _skillDetail = "SkillTracker_Dev.SkillDetail";
        public ICacheProvider CacheProvider { get; set; }
        public CacheHelper(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }
        public List<SkillDetail> SkillDetail
        {
            get
            {
                return CacheProvider.GetItem<List<SkillDetail>>(GetCacheKeyName(_skillDetail));
            }
            set
            {
                CacheProvider.AddItemAsync(GetCacheKeyName(_skillDetail), value);
            }
        }
        private string GetCacheKeyName(string key)
        {
            return key;
        }
    }
}
