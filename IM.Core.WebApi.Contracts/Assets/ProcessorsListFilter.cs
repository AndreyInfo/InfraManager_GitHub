using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Assets
{
    /// <summary>
    /// Модель фильтра поиска инсталляций
    /// </summary>
    public sealed class ProcessorsListFilter
    {
        /// <summary>
        /// Начальная запись
        /// </summary>
        public int StartRecordIndex { get; set; }

        /// <summary>
        /// Коль-во записей
        /// </summary>
        public int CountRecords { get; set; }
        /// <summary>
        /// Идентификатор типа для отбора
        /// </summary>
        public Guid? ProductCatalogID { get; set; }
        /// <summary>
        /// Идентификатор категории для отбора
        /// </summary>
        public int? ProductCatalogClassID { get; set; }
        /// <summary>
        /// Строка поиска наименования
        /// </summary>
        public string SearchText { get; set; }
    }
}
