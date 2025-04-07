using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Events
{
    public class EventDetails
    {
        public Guid ID { get; init; }
        public string UserName { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; init; }
        public Guid UserID { get; init; }
        public ObjectClass? ClassID { get; init; }
        
        public List<EventSubjectDetails> EventSubject { get; init; }
        public List<EventDetails> ChildEvents { get; init; }
    }
}
