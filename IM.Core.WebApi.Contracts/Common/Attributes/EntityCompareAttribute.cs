using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityCompareAttribute : Attribute
    {
        public EntityCompareAttribute(int objectClassID)
        {
            ObjectClassID = objectClassID;
        }
        public EntityCompareAttribute(int objectClassID, int classID)
        {
            ObjectClassID = objectClassID;
            ClassID = classID;
        }
        public EntityCompareAttribute(int objectClassID, string entityName)
        {
            ObjectClassID = objectClassID;
            EntityName = entityName;
        }
        public EntityCompareAttribute(int objectClassID, int classID, string entityName)
        {
            ObjectClassID = objectClassID;
            ClassID = classID;
            EntityName = entityName;
        }
        /// <summary>
        /// Идентификатр класса объекта сущности
        /// </summary>
        public int ObjectClassID { get; private set; }
        /// <summary>
        /// Уточняющий идентификатор класса
        /// </summary>
        public int ClassID { get; private set; }
        /// <summary>
        /// Имя сущности (типа) для отображения в событии
        /// </summary>
        public string EntityName { get; private set; }
        /// <summary>
        /// строка описания дейсвия добавления
        /// </summary>
        public string AddActionLabel { get; set; }
        /// <summary>
        /// строка описания дейсвия изменения
        /// </summary>
        public string EditActionLabel { get; set; }
        /// <summary>
        /// строка описания дейсвия удаления
        /// </summary>
        public string DeleteActionLabel { get; set; }
        /// <summary>
        /// имя поля идентификатора
        /// </summary>
        public string IdentifierField { get; set; }
        /// <summary>
        /// Идентификатор операции добавления
        /// </summary>
        public int AddOperationID { get; set; }
        /// <summary>
        /// Идентификатор операции измененеия
        /// </summary>
        public int EditOperationID { get; set; }
        /// <summary>
        /// Идентификатор операции удаления (отметки удлаления)
        /// </summary>
        public int DeleteOperationID { get; set; }
        /// <summary>
        /// Имя поля, содрежащее отметку удаления.
        /// По Умолчанию: IsDeleted
        /// </summary>
        public string IsDeletedFieldName { get; set; }
    }
}
