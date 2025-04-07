using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

public class LifeCycleData
{
    public string Name { get; init; }
    public Guid? FormID { get; init; }
    public byte[] RowVersion { get; init; }
    public LifeCycleType Type { get; init; }
    public LifeCycleStateSubData[] States { get; init; }
}