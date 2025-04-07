using System.ComponentModel.DataAnnotations;

namespace InfraManager.Web.Models.LicenceObjects
{
    /// <summary>
    /// Типы объектов лиценирования
    /// </summary>
    public enum LicenceObjectType
    {
        /// <summary>
        /// На компьютер физический
        /// </summary>
        [Display(Name = "На компьютер физический")]
        ToPhysicalСomputer = 1,

        /// <summary>
        /// На сервер физический
        /// </summary>
        [Display(Name = "На сервер физический")]
        ToPhysicalServer = 2,

        /// <summary>
        /// На пользователя
        /// </summary>
        [Display(Name = "На пользователя")]
        PerUser = 3,

        /// <summary>
        /// На устройство (компьютер/сервер)
        /// </summary>
        [Display(Name = "На устройство (компьютер/сервер)")]
        PerDevice = 4,
    }
}