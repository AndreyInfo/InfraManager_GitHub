using System;

namespace InfraManager.WebApi.Contracts.Models.Documents;

public class DocumentReferenceData
{
    public ObjectClass ClassID { get; init; }
    public Guid EntityID { get; init; } 
    public Guid[] DocumentsID { get; init; }
}