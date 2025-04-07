using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common
{
    /// <summary>
    /// Базовый фильтр для отсеивания каких-либо объектов.
    /// <para>Содержит в себе параметры пагинации и строку поиска.</para>
    /// </summary>
    public class BaseFilter
    {
        
        /// <summary>
        /// Название справочника
        /// </summary>
        public string ViewName { get; init; }
        
        /// <summary>
        /// Строка для поиска
        /// </summary>
        public string SearchString { get; init; }

        /// <summary>
        /// Начальная запись
        /// </summary>
        public int StartRecordIndex { get; init; }

        /// <summary>
        /// Кол-во записей
        /// </summary>
        public int CountRecords { get; init; }

        /// <summary>
        /// Проверяет является ли фильтр пустым.
        /// </summary>
        /// <returns>True - если фильтр не имеет никаких значений; иначе False.</returns>
        public bool IsEmpty() => CountRecords == 0 && StartRecordIndex == 0 && string.IsNullOrWhiteSpace(SearchString);
    }
}