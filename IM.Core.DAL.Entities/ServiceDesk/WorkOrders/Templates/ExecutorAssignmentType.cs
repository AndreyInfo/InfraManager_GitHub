namespace InfraManager.DAL.ServiceDesk.WorkOrders.Templates;

public enum ExecutorAssignmentType : byte
{
    /// <summary>
    /// Автоназначение не используется
    /// </summary>
    None = 0,                         //xxxxx000
    /// <summary>
    /// Фиксированный сотрудник
    /// </summary>
    User = 1,                         //xxxxx001
    /// <summary>
    /// Фиксированная группа
    /// </summary>
    Queue = 2,                        //xxxxx010
    /// <summary>
    /// Сотрудник с наименьшей нагрузкой
    /// </summary>
    LessBusyUser = 3,                 //xxxxx011
    /// <summary>
    /// Сотрудник с наименьшей нагрузкой из фиксированной группы
    /// </summary>
    LessBusyUserFromQueue = 4,        //xxxxx100
    /// <summary>
    /// Ответственный за сервис (сотрудник / группа / сервисный блок)
    /// </summary>
    ServiceResponsible = 5,           //xxxxx101
    /// <summary>
    /// Сотрудник с наименьшей нагрузкой из группы поддержки/администрирования
    /// </summary>
    LessBusyUserFromSupportLine = 6,  //xxxxx110

    /// <summary>
    /// С учетом ТТЗ
    /// </summary>
    FlagTTZ = 1 << 3,                 //xxxx1***
    /// <summary>
    /// С учетом ТОЗ
    /// </summary>
    FlagTOZ = 1 << 4,                 //xxx1x***
    /// <summary>
    /// С учетом ответственности за сервис
    /// </summary>
    FlagServiceResponsible = 1 << 5,    //xx1xx***
    /// <summary>
    /// С учетом графиков рабочего времени
    /// </summary>
    FlagCalendarWorkSchedule = 1 << 6,   //x1xxx***

    /// <summary>
    /// Все флаги выделены
    /// </summary>
    Flags = FlagTTZ | FlagTOZ | FlagServiceResponsible | FlagCalendarWorkSchedule
}
