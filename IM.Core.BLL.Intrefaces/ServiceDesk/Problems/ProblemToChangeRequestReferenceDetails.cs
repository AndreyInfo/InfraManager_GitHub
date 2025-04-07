using System;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemToChangeRequestReferenceDetails
{
    public long ID { get; init; }
    public Guid ProblemID { get; init; }
    public Guid ChangeRequestID { get; init; }
}