using System;
using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL.Events
{
    [ObjectClassMapping(ObjectClass.Event)]
    [OperationIdMapping(ObjectAction.ViewDetails, InfraManager.OperationID.Event_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, InfraManager.OperationID.Event_Properties)]
    public class Event
    {
        protected Event()
        {
            EventSubject = new HashSet<EventSubject>();
        }

        public Event(string message, int? operationId, Guid userId)
            : this()
        {
            Id = Guid.NewGuid();
            Message = message;
            OperationID = operationId;
            UserId = userId;
            Date = DateTime.UtcNow;
        }

        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public Guid UserId { get; init; }
        public int? OperationID { get; init; }
        private string _message;
        public string Message 
        { 
            get => _message; 
            set => _message = value?.Length > MessageMaxLength ? value[..(MessageMaxLength - 1)] : value; 
        }
        public Guid? ParentID { get; set; }
        public long InsertOrder { get; init; }
        public virtual Event Parent { get; init; }
        public virtual ICollection<Event> ChildEvents { get; init; }
        public virtual ICollection<EventSubject> EventSubject { get; }
        public virtual User User { get; init; }
        public static int MessageMaxLength => 255;
    }
}