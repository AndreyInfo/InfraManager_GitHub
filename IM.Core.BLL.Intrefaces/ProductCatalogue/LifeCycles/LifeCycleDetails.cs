using System;
using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

public class LifeCycleDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public bool Fixed { get; init; }

    public Guid? FormID { get; init; }

    public byte[] RowVersion { get; init; }

    public LifeCycleType Type { get; init; }

    public LifeCycleStateDetails[] States { get; init; }
}
