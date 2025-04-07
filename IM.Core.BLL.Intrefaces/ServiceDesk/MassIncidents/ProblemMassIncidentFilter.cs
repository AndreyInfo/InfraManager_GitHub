using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

/// <summary>
/// Представляет фильтр отчета "Массовые инциденты ассоциированные с проблемой".
/// </summary>
public class ProblemMassIncidentFilter : ListFilter
{
    /// <summary>
    /// Возвращает или задает уникальный идентификатор проблемы.
    /// </summary>
    public Guid? ProblemID { get; init; }
}