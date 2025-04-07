using System;
using System.Collections.Generic;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk;

/// <summary>
/// Фильтр получения списка исполнителей.
/// </summary>
public class ExecutorListFilter : BaseFilter
{
    /// <summary>
    /// Признак, что нужны именно исполнители.
    /// </summary>
    public bool SDExecutor { get; init; } = false;

    /// <summary>
    /// Уникальный идентификатор объекта, к которому исполнитель должен иметь доступ, чтобы быть исполнителем.
    /// </summary>
    public Guid? HasAccessToObjectID { get; init; }

    /// <summary>
    /// Класс объекта, к которому исполнитель должен иметь доступ, чтобы быть исполнителем.
    /// </summary>
    public ObjectClass? HasAccessToObjectClassID { get; init; }

    /// <summary>
    /// Получить список исполнителей с учетом доступа к объекту по ТТЗ.
    /// </summary>
    public bool TTZEnabled { get; init; }

    /// <summary>
    /// Получить список исполнителей с учетом доступа к объекту по ТОЗ.
    /// </summary>
    public bool TOZEnabled { get; init; }

    /// <summary>
    /// Получить список исполнителей с учетом ответственности за сервис.
    /// </summary>
    public bool ServiceResponsibilityEnabled { get; init; }

    /// <summary>
    /// Получить список исполнителей из этой очереди.
    /// </summary>
    public Guid? QueueID { get; init; }

    /// <summary>
    /// Список ID пользователей.
    /// </summary>
    public List<Guid> UserIDList { get; init; }

    /// <summary>
    /// Список ID групп.
    /// </summary>
    public Guid[] QueueIDList { get; init; } = Array.Empty<Guid>();

    /// <summary>
    /// Возвращает или задает признак того, что пользователь должен "Участвовать в автоназначении".
    /// </summary>
    public bool ShouldParticipateAutoAssign { get; init; }
}