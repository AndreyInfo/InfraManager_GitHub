namespace InfraManager.DAL.Software
{
    public enum SoftwareModelDependencyType
    {
        /// <summary>
        /// Обычная модель
        /// </summary>
        CommercialModel = 1,

        /// <summary>
        /// Пакет ПО
        /// </summary>
        SoftwarePackage = 2,

        /// <summary>
        /// Upgrade
        /// </summary>
        Upgrade = 3,

        /// <summary>
        /// Обновление / Исправление
        /// </summary>
        UpdateCorrection = 4,

        /// <summary>
        /// Компонент / Дополнение
        /// </summary>
        Component = 5,

        /// <summary>
        /// Техническая модель
        /// </summary>
        TechnicalModel = 6
    }
}
