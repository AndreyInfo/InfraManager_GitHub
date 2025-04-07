namespace InfraManager.DAL.CalendarWorkSchedules;

public enum CalendarDayType : byte
{
    /// <summary>
    /// Неопределенный
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Рабочий
    /// </summary>
    Work = 1,

    /// <summary>
    /// Сокращенный
    /// </summary>
    WorkShort = 2,

    /// <summary>
    /// Удлиненный
    /// </summary>
    WorkLong = 3,

    /// <summary>
    /// Предпраздничный
    /// </summary>
    WorkPreHoliday = 4,

    /// <summary>
    /// Праздничный (фикс.)
    /// </summary>
    Holiday = 5,

    /// <summary>
    /// Выходной (фикс.)
    /// </summary>
    Weekend = 6,

    /// <summary>
    /// Выходной
    /// </summary>
    WeekendByTemplate = 7
}
