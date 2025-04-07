using System;

namespace InfraManager.BLL.ServiceDesk
{
    /// <summary>
    /// Описание присоединяемого документа
    /// </summary>
    public class DocumentInfoDetails
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// ID родительского объекта
        /// </summary>
        public Guid ObjectId { get; set; }
        
        /// <summary>
        /// Наименование документа / имя файла
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Расширение документа
        /// </summary>
        public string Extension { get; init; }

        /// <summary>
        /// Размер данных документа
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Идентификатор пользователя загрузившего документ
        /// </summary>
        public Guid AuthorId { get; init; }
        
        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        public DateTime DateCreated { get; init; }
    }
}
