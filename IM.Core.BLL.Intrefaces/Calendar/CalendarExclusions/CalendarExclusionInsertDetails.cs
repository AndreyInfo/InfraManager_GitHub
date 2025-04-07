using System;

namespace InfraManager.BLL.Calendar.CalendarExclusions;

public class CalendarExclusionInsertDetails
{
    /// <summary>
    /// ClassID связанного объекта
    /// </summary>
    public ObjectClass ObjectClassID { get; init; }

    /// <summary>
    /// ID связаннного объекта
    /// </summary>
    public Guid ObjectID { get; init; }

    /// <summary>
    /// Идентификатор причины
    /// </summary>
    public Guid ExclusionID { get; init; }

    /// <summary>
    /// Начала периода отклонения
    /// </summary>
    public DateTime UtcPeriodStart { get; init; }

    /// <summary>
    /// Конец периода отклонения
    /// </summary>
    public DateTime UtcPeriodEnd { get; init; }

    /// <summary>
    /// Рабочий или не рабочий период был
    /// </summary>
    public bool IsWorkPeriod { get; init; }

    /// <summary>
    /// Идентификатор Сервиса
    /// </summary>
    public Guid ServiceID { get; init; }

    /// <summary>
    /// Класс ID сервиса
    /// </summary>
    public ObjectClass ServiceClassID { get; init; }
}
