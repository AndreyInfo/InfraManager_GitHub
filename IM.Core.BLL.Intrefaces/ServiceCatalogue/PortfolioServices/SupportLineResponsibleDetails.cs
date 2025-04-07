using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public class SupportLineResponsibleDetails
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public ObjectClass ClassId { get; init; }

    public byte Line { get; set; }

    public Guid ObjectID { get; set; }

    public ObjectClass ObjectClassID { get; set; }
}
