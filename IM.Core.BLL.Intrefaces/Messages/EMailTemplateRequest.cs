using System;

namespace InfraManager.BLL.Messages;

public class EMailTemplateRequest
{
    /// <summary>
    /// Идентификатор объекта
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    /// Класс объекта
    /// </summary>
    public ObjectClass ClassID { get; init; }
        
    /// <summary>
    /// Идентификатор шаблона оповещения.
    /// </summary>
    public Guid NotificationID { get; init; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserID { get; init; }
}