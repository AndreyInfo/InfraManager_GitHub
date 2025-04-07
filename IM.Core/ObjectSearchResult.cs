using System;

namespace InfraManager
{
    public class ObjectSearchResult
    {
        public ObjectClass ClassID { get; init; }
        public Guid ID { get; init; }
        public string FullName { get; init; }
        public string Details { get; init; }
    }
}
