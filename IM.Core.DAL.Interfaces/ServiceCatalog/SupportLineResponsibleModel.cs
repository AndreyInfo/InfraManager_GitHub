using System;

namespace InfraManager.DAL.ServiceCatalog;

public class SupportLineResponsibleModelItem
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public ObjectClass ClassID { get; init; }

    public byte Line { get; init; }

    public Guid ObjectID { get; init; }

    public ObjectClass ObjectClassID { get; init; }
}
