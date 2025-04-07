using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    /*Важно! 
     * 1. Здесь нет идентификатора RFCType, но есть идентификатор версии
     * 2. Каждое свойство и сам класс имеет описание, что в нем содержится и с точки зрения бизнеса
     * и технически (если значение как-то трансформируется).
     */
    /// <summary>
    /// Этот класс описывает модель изменяемых данных типа запросов на изменения
    /// </summary>
    public class RfcTypeModel
    {
        /// <summary>
        /// Возвращает FormID 
        /// </summary>
        public Guid? FormID { get; init; }

        /// <summary>
        /// Возвращает наименование типа запроса на изменение
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Возвращает идентификатор схемы рабочей процедуры, ассоциированной с данным типом запросов на изменения
        /// </summary>
        public string WorkflowSchemeIdentifier { get; init; }

        /// <summary>
        /// Возвращает Base64 представление идентификатора версии типа запроса на изменения
        /// </summary>
        public string RowVersion { get; init; }
        
        public string IconName { get; set; }
        
        public bool Removed { get; init; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidObjectException("Требуется имя типа запроса на изменения"); // TODO: Add localization
            }
        }
    }
}
