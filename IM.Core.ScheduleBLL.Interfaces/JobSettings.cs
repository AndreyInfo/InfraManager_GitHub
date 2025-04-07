namespace IM.Core.ScheduleBLL.Interfaces;

public class JobSettings
{
    public JobSettings(bool isLongRunning)
    {
        IsLongRunning = isLongRunning;
    }
    
    public JobSettings()
    {
    }

    /// <summary>
    /// Описывает, выполняется ли задача долго и ее не надо сразу завершать (импорт пользователей и тд)
    /// </summary>
    public bool IsLongRunning { get; init; }

    public static JobSettings EmptySetting => new();
}