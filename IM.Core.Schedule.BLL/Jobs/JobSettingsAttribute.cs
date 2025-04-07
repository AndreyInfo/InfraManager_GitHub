using IM.Core.ScheduleBLL.Interfaces;

namespace IM.Core.Schedule.BLL.Jobs;

[AttributeUsage(AttributeTargets.Class)]  
public class JobSettingsAttribute : Attribute
{
    public readonly JobSettings settings;

    public JobSettingsAttribute(bool isLongRunning)
    {
        settings = new JobSettings(isLongRunning);
    }
}