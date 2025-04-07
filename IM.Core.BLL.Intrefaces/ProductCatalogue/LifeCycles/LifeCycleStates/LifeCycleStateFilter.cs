using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

public class LifeCycleStateFilter : BaseFilter
{
    public Guid? LifeCycleID { get; init; }
}
