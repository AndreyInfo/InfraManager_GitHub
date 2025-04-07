namespace InfraManager.DAL.Sessions;

/// <summary>
///     Enum описывает все возможные состояния пользовательских сессий
/// </summary>
public enum SessionHistoryType : byte
{
    /// <summary>
    /// Подключение пользователя
    /// </summary>
    Connect = 0,
    
    /// <summary>
    /// Отключение пользователя
    /// </summary>
    Disconnect = 1,

    /// <summary>
    /// Принудительное отключение пользователя
    /// </summary>
    KillConnection = 2,

    /// <summary>
    /// Неудачное подключение
    /// </summary>
    ConnectionFail = 3,
    
    /// <summary>
    /// Сессия отключена из за неактивности
    /// </summary>
    InactiveDisabled = 4
}