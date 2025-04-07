using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Events;

public class EventSubjectDetails
{
    public Guid ID { get; init; }
    public string SubjectName { get; init; }
    public string SubjectValue { get; init; }
    public Guid? ObjectID { get; init; }
    public ObjectClass? ClassID { get; init; }
    
    public List<EventSubjectParamDetails> EventSubjectParam { get; init; }
}