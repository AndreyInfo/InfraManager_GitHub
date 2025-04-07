
namespace InfraManager.DAL.Software;

public enum SoftwareInstallationState : byte
{
    /// <summary>
    /// Инсталляция активна
    /// </summary>
    Active = 0,

    /// <summary>
    /// Инсталляция в архиве
    /// </summary>
    Archive = 1
}
