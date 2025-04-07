namespace InfraManager.DAL.ServiceCatalogue;

/// <summary>
/// Перечисление возможных полей для импорта сервисов
/// </summary>
public enum ConcordanceSCObjectType : int
{
    /// <summary>
    /// Имя категории
    /// </summary>
    Category_Name = 0,
    /// <summary>
    /// Внешний идентификатор
    /// </summary>
    ExternalIdentifier,
    /// <summary>
    /// Имя сервиса
    /// </summary>
    Service_Name,
    /// <summary>
    /// Состояние
    /// </summary>
    State
}
