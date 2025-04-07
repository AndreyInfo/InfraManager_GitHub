using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.CrossPlatform.WebApi.Contracts.ELP
{
    /// <summary>
    /// Модель фильтра поиска схем лицензирования
    /// </summary>
    public sealed class ELPListFilter : BaseFilter
    {
        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        public int StartRecordIndex { get; set; }
        public int CountRecords { get; set; }
    }
}