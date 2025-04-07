using System;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public class PortfolioServicesItem
{
    public Guid? ID { get; init; }

    public string Name { get; init; }

    public string IconName { get; init; }

    public ObjectClass ClassId { get; init; }

    public bool HasChild { get; init; }

    public string Note { get; init; }

    public Guid? ParentId { get; init; }

    public byte[] RowVersion { get; init; }

    public bool IsSelectFull { get; set; }

    public bool IsSelectPart { get; set; }
}