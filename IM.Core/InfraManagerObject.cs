using System;

namespace InfraManager
{
    public struct InframanagerObject
    {
        public InframanagerObject(Guid id, ObjectClass classID)
        {
            Id = id;
            ClassId = classID;
        }

        public Guid Id { get; }
        public ObjectClass ClassId { get; }

        public bool IsDefault => Id == default && ClassId == default;
        public override string ToString()
        {
            return $"object (ID = {Id} Class = {Enum.GetName(ClassId)})";
        }
    }
}
