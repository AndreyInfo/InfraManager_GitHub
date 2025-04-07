using System.Collections.Generic;

namespace InfraManager.BLL.Notification;

/// <summary>
/// Интерфейс позволяет получать настройки оповещений у определенного класса
/// </summary>
public interface INotificationProvider
{
    /// <summary>
    /// Возвращает список ролей оповещений конкретного класса
    /// </summary>
    Dictionary<int, string> GetRoles();
    
    /// <summary>
    /// Возвращает список шаблонов конкретного класса
    /// </summary>
    ParameterTemplate[] GetAvailableParameterList();
}