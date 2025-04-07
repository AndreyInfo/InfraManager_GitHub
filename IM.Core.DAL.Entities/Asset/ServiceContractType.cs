using System;

namespace InfraManager.DAL.Asset;

public class ServiceContractType
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public byte[] RowVersion { get; init; }
}