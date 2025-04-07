using System;

namespace InfraManager.BLL.ServiceCatalogue;

public class ServiceCustomerDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public string FullName { get; init; }

    public string PositionName { get; init; }

    public int ClassId { get; init; }
}
