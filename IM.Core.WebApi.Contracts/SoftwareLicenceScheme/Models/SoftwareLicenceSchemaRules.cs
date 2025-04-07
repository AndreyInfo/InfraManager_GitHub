namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Бизнес правила схем лицензирования, проверяемые при сохранении данных
    /// </summary>
    public enum SoftwareLicenceSchemeRules
    {
        /// <summary>
        /// Нарушена уникальность имени схемы
        /// </summary>
        UniqueName,

        /// <summary>
        /// Не указано имя схемы
        /// </summary>
        NameObligatory,

        /// <summary>
        /// Вид схемы не допускает ее редактирования
        /// </summary>
        ModificationProhibited,

        /// <summary>
        /// схема не найдена
        /// </summary>
        SchemeNotFound,

        /// <summary>
        /// Проблемы формирования события при изменении
        /// </summary>
        EventFault,
    }
}
