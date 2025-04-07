namespace InfraManager.CrossPlatform.WebApi.Contracts.ELP
{
    /// <summary>
    /// Модель фильтра поиска схем лицензирования
    /// </summary>
    public sealed class ELPVendorsListFilter
    {
        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string SearchText { get; set; }
        public int StartRecordIndex { get; set; }
        public int CountRecords { get; set; }
    }
}
