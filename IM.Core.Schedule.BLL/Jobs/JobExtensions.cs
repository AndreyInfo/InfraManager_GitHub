using IM.Core.ScheduleBLL.Interfaces;

namespace IM.Core.Schedule.BLL.Jobs;

public static class JobExtensions
{
    public static JobSettings GetSettings(this IBaseJob job)
    {
        var settingAttribute =
            (JobSettingsAttribute)Attribute.GetCustomAttribute(job.GetType(), typeof(JobSettingsAttribute));

        if (settingAttribute == null)
        {
            return JobSettings.EmptySetting;
        }

        return settingAttribute.settings;
    }
}