namespace InfraManager.DAL.ServiceCatalogue;

/// <summary>
/// Тип услуг сервиса
/// </summary>
public enum AttendanceType : byte
{
    /// <summary>
    /// Системная
    /// </summary>
    System = 0,

    /// <summary>
    /// Пользовательская
    /// </summary>
    User = 1,

    /// <summary>
    /// Регламентные работы
    /// </summary>
    Maintenance = 2
}
