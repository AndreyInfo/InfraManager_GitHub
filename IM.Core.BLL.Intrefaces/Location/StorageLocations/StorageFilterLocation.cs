using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.Location.StorageLocations;

public class StorageFilterLocation : BaseFilter
{
    public Guid StorageLocationID { get; init; }

}
