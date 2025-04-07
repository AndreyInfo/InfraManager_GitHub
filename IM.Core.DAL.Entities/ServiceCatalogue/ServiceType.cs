namespace InfraManager.DAL.ServiceCatalogue;

/// <summary>
/// Тип сервиса
/// </summary>
public enum ServiceType : byte
{
    /// <summary>
    /// Системный
    /// </summary>
    Internal = 0,

    /// <summary>
    /// Пользовательский
    /// </summary>
    External = 1
}
