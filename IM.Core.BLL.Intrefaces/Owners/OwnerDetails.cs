using System;

namespace InfraManager.BLL.Owners;

public class OwnerDetails
{
    public string Name { get; init; }

    public int? VisioId { get; init; }

    public Guid IMObjID { get; init; }
}