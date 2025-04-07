using System;

namespace InfraManager.BLL.CalendarService;

public class WorkTimeByGroupData
{
    /// <summary>
    /// Время начала периода UTC.
    /// </summary>
    public DateTime utcStartDate { get; init; }

    /// <summary>
    /// Время окончания периода UTC.
    /// </summary>
    public DateTime utcFinishDate { get; init; }

    /// <summary>
    /// Уникальный идентификатор группы, для которой производится расчет.
    /// </summary>
    public Guid GroupID { get; init; }
}