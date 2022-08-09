using Enyim.Caching;
using Microsoft.Extensions.Options;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Threading.Tasks;

namespace SkillTracker.Shared
{
    public class MemCacheProvider : ICacheProvider
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly int _cacheLifTime;
        public MemCacheProvider(IMemcachedClient memcachedClient, IOptions<CacheConfiguration> cacheOptions)
        {
            _memcachedClient = memcachedClient;
            _cacheLifTime = cacheOptions.Value.CacheLifeTime;
        }

        public void AddItem<T>(string key, T t)
        {
            _memcachedClient.Set(key, t, 60 * 1);
        }

        public async Task AddItemAsync<T>(string key, T t)
        {
            await _memcachedClient.SetAsync(key, t, _cacheLifTime);
        }

        public T GetItem<T>(string key)
        {
            return _memcachedClient.Get<T>(key);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            return (T)await _memcachedClient.GetAsync<T>(key);
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(string key)
        {
            _memcachedClient.Remove(key);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _memcachedClient.RemoveAsync(key);
        }
    }
}
