using System;

namespace InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierDetails : KnowledgeBaseClassifierData
{
    public Guid ID { get; init; }
    public string ParentName { get; init; }
    public string ExpertName { get; init; }
}