using System;

namespace InfraManager.DAL.Events;

/// <summary>
/// Предоставляет данные для построения запроса на получение списка событий по объекту.
/// </summary>
public class EventsForObjectQueryRequest
{
    /// <summary>
    /// Возвращает уникальный идентификатор объекта, с которым связаны события.
    /// </summary>
    public Guid? ObjectID { get; init; }

    /// <summary>
    /// Возвращает класс объекта, с которым связаны события.
    /// </summary>
    public ObjectClass? ClassID { get; init;}

    /// <summary>
    /// Возвращает или задает минимальную дату событий для выгрузки.
    /// </summary>
    public DateTime? DateFrom { get; init;}

    /// <summary>
    /// Возвращает или задает максимальную дату событий для выгрузки.
    /// </summary>
    public DateTime? DateTill { get; init; }
    
    /// <summary>
    /// Возвращает уникальный идентификатор родительского события, для выгрузки дочерних событий.
    /// </summary>
    public Guid? ParentID { get; init; }

    /// <summary>
    /// Возвращает или задает список имен параметров объекта, изменения которых должны быть выгружены.
    /// </summary>
    public string[] Parameters { get; init; }
}