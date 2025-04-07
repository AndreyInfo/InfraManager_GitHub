using System;

namespace InfraManager.DAL.Events
{
    public class EntityEvent
    {
        public EntityEvent(
            EventSource source, 
            EventType type, 
            ObjectClass entityClassId, 
            Guid entityId,
            Guid? causerId = null,
            string targetStateId = null)
        {
            Id = Guid.NewGuid();
            UtcRegisteredAt = DateTime.UtcNow;
            Source = source;
            Type = type;
            EntityClassId = entityClassId;
            EntityId = entityId;
            CauserId = causerId;
            TargetStateId = targetStateId;
        }

        public EntityEvent(
            Guid id,
            EventSource source, 
            EventType type, 
            ObjectClass entityClassId, 
            Guid entityId,
            Guid? causerId = null,
            string targetStateId = null)
        {
            Id = id;
            UtcRegisteredAt = DateTime.UtcNow;
            Source = source;
            Type = type;
            EntityClassId = entityClassId;
            EntityId = entityId;
            CauserId = causerId;
            TargetStateId = targetStateId;
        }
        
        protected EntityEvent()
        {
        }

        public Guid Id { get; private set; }
        public DateTime UtcRegisteredAt { get; private set; }
        public long Order { get; init; }
        public EventSource Source { get; private set; }
        public EventType Type { get; private set; }
        public ObjectClass EntityClassId { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid? OwnerId { get; set; }
        public DateTime? UtcOwnedUntil { get; set; }
        public Guid? CauserId { get; init; }
        public bool IsProcessed { get; set; }
        public string TargetStateId { get; init; }
        public byte[] Argument { get; init; }
    }
}
