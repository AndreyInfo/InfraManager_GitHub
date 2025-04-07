using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Events
{
    public class EventSubject
    {
        public static int MaxLength = 255;
        public EventSubject()
        {
            EventSubjectParam = new HashSet<EventSubjectParam>();
        }

        public EventSubject(string name, string value) : this()
        {
            Id = Guid.NewGuid();
            SubjectName = name;
            SubjectValue = value.Slice(MaxLength);
        }

        public EventSubject(string name, string value, InframanagerObject subject) : this(name, value)
        {
            ObjectId = subject.Id;
            ClassId = subject.ClassId;
        }

        public Guid Id { get; init; }
        public string SubjectName { get; init; }
        public string SubjectValue { get; init; }
        public Guid? ObjectId { get; init; }
        public ObjectClass? ClassId { get; init; }
        public Guid EventID { get; init; }
        public virtual ICollection<EventSubjectParam> EventSubjectParam { get; }
    }
}