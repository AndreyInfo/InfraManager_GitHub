using System;

namespace InfraManager.DAL.Events;

/// <summary>
/// Представляет результат выполнения запроса <see cref="IEventsForObjectQuery"/>.
/// </summary>
public class EventForObjectQueryResultItem
{
    /// <summary>
    /// Возвращает уникальный идентификатор события.
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    /// Возвращает время события.
    /// </summary>
    public DateTime UtcDate { get; init; }

    /// <summary>
    /// Возвращает уникальный идентификатор пользователя-автора события.
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
    /// Возвращает имя параметра, связанного с событием.
    /// </summary>
    public string ParameterName { get; init; }

    /// <summary>
    /// Возвращает значение параметра с именем <see cref="ParameterName"/> до изменения.
    /// </summary>
    public string ParameterOldValue { get; init; }

    /// <summary>
    /// Возвращает значение параметра с именем <see cref="ParameterName"/> после изменения.
    /// </summary>
    public string ParameterNewValue { get; init; }

    /// <summary>
    /// Возвращает порядок возникновения события.
    /// </summary>
    public long Order { get; init; }
}