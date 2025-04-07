using System;

namespace InfraManager.BLL.ProductCatalogue.Units;

public class UnitDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid? ComplementaryID { get; init; }

    public string Code { get; init; }

    public string ExternalID { get; init; }
}