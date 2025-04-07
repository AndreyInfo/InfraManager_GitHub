namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Модель фильтра поиска схем лицензирования
    /// </summary>
    public sealed class SoftwareLicenceSchemeListFilter
    {
        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string SearchText { get; set; }
    }
}
