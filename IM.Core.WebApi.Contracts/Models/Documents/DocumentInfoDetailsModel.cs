using System;

namespace InfraManager.WebApi.Contracts.Models.Documents
{
    public class DocumentInfoDetailsModel
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// ID родительского объекта
        /// </summary>
        public Guid ObjectID { get; init; }

        /// <summary>
        /// Наименование документа / имя файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Расширение документа
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Размер данных документа
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Идентификатор пользователя загрузившего документ
        /// </summary>
        public Guid AuthorID { get; set; }

        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
