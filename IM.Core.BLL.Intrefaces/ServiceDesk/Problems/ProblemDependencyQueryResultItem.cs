using System;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemDependencyQueryResultItem
{
    public long ID { get; init; }
    public Guid ProblemID { get; init; }
    public Guid ObjectID { get; init;  }
    public ObjectClass ObjectClassID { get; init; }
    public string ObjectName { get; init; }
    public string ObjectLocation { get; init; }
    public string Note { get; init; }
    public byte Type { get; init; }
    public bool Locked { get; init; }
}