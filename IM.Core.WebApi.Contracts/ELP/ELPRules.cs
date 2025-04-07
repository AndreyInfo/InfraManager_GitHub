namespace InfraManager.CrossPlatform.WebApi.Contracts.ELP
{
    /// <summary>
    /// Бизнес правила схем лицензирования, проверяемые при сохранении данных
    /// </summary>
    public enum ELPRules
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
        /// Свзь используется
        /// </summary>
        InUse,
        /// <summary>
        /// Ошибка запсис события
        /// </summary>
        EventFault,
    }
}
