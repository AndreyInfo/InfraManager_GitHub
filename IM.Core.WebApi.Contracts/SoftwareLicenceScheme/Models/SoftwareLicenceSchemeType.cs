using System.ComponentModel.DataAnnotations;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    public enum SoftwareLicenceSchemeType : byte
    {
        /// <summary>
        /// Пользовательская схема
        /// </summary>
        [Display(Name ="Пользовательская схема")]
        User = 0,

        /// <summary>
        /// Системная схема
        /// </summary>
        [Display(Name = "Системная схема")]
        System = 1,
    }
}
