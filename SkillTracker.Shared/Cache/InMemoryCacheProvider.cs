using Microsoft.Extensions.Caching.Memory;
using SkillTracker.Core;
using System;
using System.Threading.Tasks;

namespace SkillTracker.Shared
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        public InMemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }
        public void AddItem<T>(string key, T t)
        {
            if (t == null) return;
            _cache.Set(key, t);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task AddItemAsync<T>(string key, T t)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (t == null) return;
            _cache.Set(key, t);
        }

        public T GetItem<T>(string key)
        {
            T retval = _cache.Get<T>(key);
            return retval;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<T> GetItemAsync<T>(string key)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            T retval = _cache.Get<T>(key);
            return retval;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(string key)
        {
            _cache.Remove(key);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task RemoveItemAsync(string key)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            _cache.Remove(key);
        }

    }
}
