using System;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;

/// <summary>
/// Базовый класс для характеристик модели/объекта
/// </summary>
public class EntityCharacteristicsDetailsBase
{
    public Guid ID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
}
