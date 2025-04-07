namespace InfraManager.DAL.Software;

public enum SoftwareLicenceSchemeEnum : byte
{
    /// <summary>
    /// Лицензия на компьютеры
    /// </summary>
    ComputerLicence = 0,

    /// <summary>
    /// Лицензия на пользователей
    /// </summary>
    UserLicence = 1,

    /// <summary>
    /// Лицензия CAL на пользователей
    /// </summary>
    UserCALLicence = 2,

    /// <summary>
    /// Лицензия CAL на компьютеры
    /// </summary>
    DeviceCALLicence = 3,

    /// <summary>
    /// Конкурентная лицензия
    /// </summary>
    ConcurentLicence = 4,

    /// <summary>
    /// Лицензия на процессоры
    /// </summary>
    ProcessorsLicence = 5,

    /// <summary>
    /// Freeware
    /// </summary>
    Freeware =  6,

    /// <summary>
    /// Shareware
    /// </summary>
    Shareware = 7,

    /// <summary>
    /// Demo
    /// </summary>
    Demo = 8,

    /// <summary>
    /// SITE
    /// </summary>
    Site = 12,
}
