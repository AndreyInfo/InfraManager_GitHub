using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

public class CalendarWorkScheduleDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public int Year { get; init; }
    public string ShiftTemplate { get; init; }
    public byte ShiftTemplateLeft { get; init; }
    public byte[] RowVersion { get; init; }
}

