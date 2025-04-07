using System;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class FoundObject : IEquatable<FoundObject>
    {
        public ObjectClass ClassID { get; init; }
        public Guid ID { get; init; }
        public string Description { get; init; }
        public string Name { get; init; }
        public Guid? ClientID { get; init; }
        public Guid? ExecutorID { get; init; }
        public Guid? OwnerID { get; init; }

        public string ClientFullName { get; init; }
        public string ExecutorFullName { get; init; }
        public int Number { get; init; }
        public string EntityStateName { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime UtcDateModified { get; init; }

        public bool Equals(FoundObject other)
        {
            return ID == other?.ID && ClassID == other.ClassID;
        }

        public override bool Equals(object obj)
        {
            if (obj is FoundObject other) return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, ClassID);
        }
    }
}