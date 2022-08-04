using System.Threading.Tasks;

namespace SkillTracker.Core
{
    public interface ICacheProvider
    {
        void Init();
        void AddItem<T>(string key, T t);
        T GetItem<T>(string key);
        void RemoveItem(string key);
        Task AddItemAsync<T>(string key, T t);
        Task<T> GetItemAsync<T>(string key);
        Task RemoveItemAsync(string key);
    }
}
