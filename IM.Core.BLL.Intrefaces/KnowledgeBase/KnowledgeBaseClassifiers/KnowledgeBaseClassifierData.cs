using System;

namespace InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierData
{
    public string Name { get; init; }
    public int UpdatePeriod { get; init; }
    public Guid? ParentID { get; init; }
    public Guid ExpertID { get; init; }
    public string Note { get; init; } = string.Empty;
    public byte[] RowVersion { get; init; }
}