using System;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Фильтра для получения списка исполнителей.
/// </summary>
public class ExecutorListQueryFilter
{
    /// <summary>
    /// Уникальный идентификатор объекта для которого выполняется поиск исполнителей.
    /// </summary>
    public Guid ObjectID { get; init; }

    /// <summary>
    /// Список ID пользователей, среди которых выполняется поиск исполнителей.
    /// </summary>
    public Guid[] UserIDs { get; init; }
    
    /// <summary>
    /// Список ID групп, в которых выполняется поиск исполнителей.
    /// </summary>
    public Guid[] QueueIDs { get; init; }

    /// <summary>
    /// Исполнитель должет иметь доступ к объекту с учетом ТТЗ.
    /// </summary>
    public bool TTZEnabled { get; init; }

    /// <summary>
    /// Исполнитель должет иметь доступ к объекту с учетом ТОЗ.
    /// </summary>
    public bool TOZEnabled { get; init; }

    /// <summary>
    /// Исполнитель должен быть ответственным за сервис.
    /// </summary>
    public bool ServiceResponsibilityEnabled { get; init; }

    /// <summary>
    /// Возвращает или задает признак того, что пользователь должен "Участвовать в автоназначении".
    /// </summary>
    public bool ShouldParticipateAutoAssign { get; init; }
}