using System;

namespace InfraManager.WebApi.Contracts.Models.Documents;

public class DocumentReferenceDetailsModel
{
    public Guid DocumentID { get; init; }
    public Guid ObjectID { get; init; }
}