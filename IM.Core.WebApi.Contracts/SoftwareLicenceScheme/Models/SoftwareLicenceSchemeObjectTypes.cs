using System.ComponentModel.DataAnnotations;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Типы объектоы лицензирования для схемы лицензирования
    /// </summary>
    public enum SoftwareLicenceSchemeObjectTypes : byte
    {
        /// <summary>
        /// Компьютер физический
        /// </summary>
        [Display(Name = "Компьютер физический")]
        RealComputer = 1,

        /// <summary>
        /// Сервер физический
        /// </summary>
        [Display(Name = "Сервер физический")]
        RealSever = 2,

        /// <summary>
        /// Пользователь
        /// </summary>
        [Display(Name = "Пользователь")]
        User = 3,

        /// <summary>
        /// Устройство (компьютер/сервер)
        /// </summary>
        [Display(Name = "Устройство (компьютер/сервер)")]
        Device = 4
    }
}
