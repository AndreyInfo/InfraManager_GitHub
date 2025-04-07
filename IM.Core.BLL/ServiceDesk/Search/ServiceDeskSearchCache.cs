using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public partial class ServiceDeskSearchCache : IServiceDeskSearchCache
    {
        private readonly int _capacity;
        private readonly Dictionary<string, Entry> _entries = new();
        private readonly LinkedList<Entry> _entryList = new();

        private int _currentCacheSize;

        public ServiceDeskSearchCache(int capacity)
        {
            _capacity = capacity;
        }

        public void Cache(string key, IReadOnlyList<FoundObject> searchResult)
        {
            lock (_entryList)
            {
                while (searchResult.Count + _currentCacheSize > _capacity && _entryList.Count > 0)
                {
                    _entries.Remove(_entryList.Last.Value.Key, out var removed);
                    _entryList.RemoveLast();
                    _currentCacheSize -= removed.Size;
                }

                _currentCacheSize += searchResult.Count;
                var entry = new Entry(key, searchResult);
                if (_entries.Remove(key, out var existingItem))
                {
                    _entryList.Remove(existingItem);
                }

                _entries[key] = entry;
                _entryList.AddFirst(entry);
            }
        }

        public bool TryTakeNext(string key, int amount, out IEnumerable<FoundObject> result)
        {
            lock (_entryList)
            {
                if (!_entries.TryGetValue(key, out var entry))
                {
                    result = Enumerable.Empty<FoundObject>();
                    return false;
                }

                _entryList.Remove(entry);
                _entryList.AddFirst(entry);
                result = entry.TakeNext(amount);
                return true;
            }
        }

        public bool TryGet(string key, out IEnumerable<FoundObject> result)
        {
            if (_entries.TryGetValue(key, out var entry))
            {
                result = entry;
                return true;
            }

            result = Enumerable.Empty<FoundObject>();
            return false;
        }
    }
}