using System;

namespace InfraManager.BLL.Calendar.CalendarWeekends;

public class CalendarWeekendDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public bool Sunday { get; init; }
    public bool Monday { get; init; }
    public bool Tuesday { get; init; }
    public bool Wednesday { get; init; }
    public bool Thursday { get; init; }
    public bool Friday { get; init; }
    public bool Saturday { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ComplementaryID { get; init; }
}
