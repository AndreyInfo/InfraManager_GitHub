namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation
{
    /// <summary>
    /// Бизнес правила схем лицензирования, проверяемые при сохранении данных
    /// </summary>
    public enum SoftwareInstallationRules
    {
        /// <summary>
        /// Имя долно быть задано
        /// </summary>
        NameObligatory,
        /// <summary>
        /// Нарушена уникальность имени схемы
        /// </summary>
        UniqueName,
        /// <summary>
        /// Техническая Модель обязательна
        /// </summary>
        ModelObligatory,
        /// <summary>
        /// Место установки обязательна
        /// </summary>
        DeviceObligatory,
        /// <summary>
        /// Ошибка записи истории
        /// </summary>
        EventFault,
    }
}
