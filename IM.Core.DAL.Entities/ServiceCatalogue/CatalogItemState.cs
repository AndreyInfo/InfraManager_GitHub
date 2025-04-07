namespace InfraManager.DAL.ServiceCatalogue;

/// <summary>
/// Состояние объекта каталога сервисов
/// </summary>
public enum CatalogItemState : byte
{
    /// <summary>
    /// Проектируется
    /// </summary>
    Projected = 0,

    /// <summary>
    /// Работает
    /// </summary>
    Worked = 1,

    /// <summary>
    /// Блокирован
    /// Временно недоступен
    /// </summary>
    Blocked = 2,
    
    /// <summary>
    /// Исключен
    /// </summary>
    Excluded = 3
}
