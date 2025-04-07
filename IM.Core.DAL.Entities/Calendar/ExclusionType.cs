namespace InfraManager.DAL.Calendar;

public enum ExclusionType : short
{
    None = 0,

    /// <summary>
    /// Пользователь
    /// </summary>
    User = 1,

    /// <summary>
    /// Сервис
    /// </summary>
    Service = 2,

    /// <summary>
    /// Пользователь и сервис
    /// </summary>
    Both = 3
}
