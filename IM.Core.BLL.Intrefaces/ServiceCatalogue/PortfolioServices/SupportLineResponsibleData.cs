using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public class SupportLineResponsibleData
{
    public Guid ObjectID { get; init; }

    public ObjectClass ObjectClassID { get; init; }

    public byte LineNumber { get; init; }

    public Guid OrganizationItemID { get; init; }

    public ObjectClass OrganizationItemClassID { get; init; }
}
