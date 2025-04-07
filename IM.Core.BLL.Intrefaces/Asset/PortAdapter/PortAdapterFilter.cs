using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.Asset.PortAdapter;

public class PortAdapterFilter : BaseFilter
{
    public Guid ObjectID { get; init; }
}

