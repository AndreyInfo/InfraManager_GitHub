namespace InfraManager.DAL.MaintenanceWork;

public enum MaintenanceState : byte
{
    /// <summary>
    /// Активна
    /// </summary>
    Active = 0,

    /// <summary>
    /// Блокирована
    /// </summary>
    Blocked = 1,

    /// <summary>
    /// Разработка
    /// </summary>
    Development = 2
}
