using System;

namespace InfraManager.DAL.Asset.Subclasses;

/// <summary>
/// Интерфейс, описывающий подкласс ИТ-актива.
/// </summary>
public interface IAssetSubclass
{
    public Guid ID { get; init; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
