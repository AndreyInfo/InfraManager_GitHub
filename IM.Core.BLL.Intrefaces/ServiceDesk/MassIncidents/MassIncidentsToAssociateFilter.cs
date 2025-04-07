using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

/// <summary>
/// Представляет фильтр отчета "Массовые инциденты для ассоциации".
/// </summary>
public class MassIncidentsToAssociateFilter : ListFilter
{
    /// <summary>
    /// Возвращает или задает уникальный идентификатор проблемы, с которой ассоциирутся массовые инциденты.
    /// </summary>
    public Guid ProblemID { get; init; }
}