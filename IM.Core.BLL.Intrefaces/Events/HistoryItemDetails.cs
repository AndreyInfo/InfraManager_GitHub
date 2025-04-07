using System;

namespace InfraManager.BLL.Events;

/// <summary>
/// Предоставляет данные о событии.
/// </summary>
public class HistoryItemDetails
{
    /// <summary>
    /// Возвращает уникальный идентификатор события.
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    /// Возвращает время события UTC.
    /// </summary>
    public DateTime UtcDate { get; init; }

    /// <summary>
    /// Возвращает уникальный идентификатор пользователя-автора события
    /// </summary>
    public Guid UserID { get; init; }

    /// <summary>
    /// Возвращает полное имя пользователя-автора события. 
    /// </summary>
    public string UserName { get; init; }
    
    /// <summary>
    /// Возвращает текст, описывающий событие.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Возвращает порядок записи события.
    /// </summary>
    public long Order { get; init; }
}