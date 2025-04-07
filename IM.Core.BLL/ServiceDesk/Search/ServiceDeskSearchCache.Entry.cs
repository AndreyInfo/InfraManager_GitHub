using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Search;

public partial class ServiceDeskSearchCache
{
    private class Entry : IEnumerable<FoundObject>
    {
        private readonly IReadOnlyList<FoundObject> _foundObjects;
        private int _startIdx;

        public string Key { get; }
        public int Size => _foundObjects.Count;

        public Entry(string key, IReadOnlyList<FoundObject> foundObjects)
        {
            Key = key;
            _foundObjects = foundObjects;
        }

        public IEnumerable<FoundObject> TakeNext(int amount)
        {
            amount = Math.Min(amount, Size - _startIdx);
            var toSkip = _startIdx;
            _startIdx += amount;
            return _foundObjects.Take(new Range(toSkip, toSkip + amount));
        }

        public IEnumerator<FoundObject> GetEnumerator()
        {
            return _foundObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}