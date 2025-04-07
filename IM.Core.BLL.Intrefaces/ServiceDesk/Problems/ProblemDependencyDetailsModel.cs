using System;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemDependencyDetailsModel
{
    public Guid ProblemID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ClassID { get; init; }
    public string ObjectName { get; init; }
    public string ObjectLocation { get; init; }
}
