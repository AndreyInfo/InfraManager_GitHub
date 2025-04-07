namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Применимость схемы для Виртуальных машины
    /// </summary>
    public enum SoftwareLicenceSchemeForVM
    {
        /// <summary>
        /// Не применима
        /// </summary>
        NotAllowed,

        /// <summary>
        /// Применима без ограничений
        /// </summary>
        AllowedUnrestricted,

        /// <summary>
        /// применима с ограничением по кол-ву, определенным свойством InstallationLimitPerVM
        /// </summary>
        AllowedWithRestriction,
    }
}
