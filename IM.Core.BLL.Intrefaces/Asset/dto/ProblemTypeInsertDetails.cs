using System;

namespace InfraManager.BLL.Asset;

public class ProblemTypeInsertDetails
{
    public string Name { get; init; }

    public Guid? ParentProblemTypeID { get; init; }

    public byte[] Image { get; init; }

    public string WorkflowSchemeIdentifier { get; init; }

    public Guid? FormID { get; init; }

    public string ImageName { get; init; }
}
