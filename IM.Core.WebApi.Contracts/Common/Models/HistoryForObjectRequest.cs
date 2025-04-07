using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    /// <summary>
    /// Запрос истрии объекта
    /// </summary>
    public class HistoryForObjectRequest
    {
        /// <summary>
        /// Объект запроса
        /// </summary>
        public ObjectIDModel ObjectID { get; set; }

        /// <summary>
        /// Отбор только событи после указанной даты (опционально)
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Отбор событий только до указаннйо даты (опционально)
        /// </summary>
        public DateTime? DateTill { get; set; }

        /// <summary>
        /// Начальная запись
        /// </summary>
        public int StartRecordIndex { get; set; }

        /// <summary>
        /// Коль-во записей
        /// </summary>
        public int CountRecords { get; set; }
    }
}
