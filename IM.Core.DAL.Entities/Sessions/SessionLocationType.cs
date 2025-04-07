namespace InfraManager.DAL.Sessions;

/// <summary>
///     Описывает к какой системе подключена сессия
/// </summary>
public enum SessionLocationType : short
{
    /// <summary>
    ///  Если система не опознана
    /// </summary>
    Other = 0,
    
    /// <summary>
    ///  Админ консоль
    /// </summary>
    AdminConsole = 1,
    
    /// <summary>
    ///  Портал(web)
    /// </summary>
    Web = 2
}