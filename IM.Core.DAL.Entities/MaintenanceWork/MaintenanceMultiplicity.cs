namespace InfraManager.DAL.MaintenanceWork;

public enum MaintenanceMultiplicity : byte
{
    /// <summary>
    /// Создается одно задание для всех элементов конфигурационного списка
    /// </summary>
    All = 0,

    /// <summary>
    /// Для каждого элемента конфигурационного списка создается свое задание
    /// </summary>
    Everyone = 1
}
