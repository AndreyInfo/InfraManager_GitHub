using System.Collections.Generic;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.AccessManagement
{
    /// <summary>
    /// Этот класс представляет сущность Операция
    /// </summary>
    public class Operation
    {
        protected Operation()
        {
        }

        /// <summary>
        /// Создает эксземпляр класса Operation
        /// </summary>
        /// <param name="id">Идентификатор операции</param>
        /// <param name="classID">Класс объекта операции</param>
        /// <param name="name">Наименование</param>
        /// <param name="operationName">Наименование операции</param>
        /// <param name="description">Описание</param>
        public Operation(OperationID id, ObjectClass classID, string name, string operationName, string description)
        {
            ID = id;
            ClassID = classID;
            Name = name;
            OperationName = operationName;
            Description = description;
        }

        /// <summary>
        /// Возвращает идентификатор операции
        /// </summary>
        public OperationID ID { get; private set; }

        /// <summary>
        /// Возвращает класс объекта операции
        /// </summary>
        public ObjectClass ClassID { get; private set; }

        /// <summary>
        /// Возвращает наименование
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Возвращает наименование операции
        /// </summary>
        public string OperationName { get; private set; }

        /// <summary>
        /// Возвращает или задает описание операции
        /// </summary>
        public string Description { get; set; }

        public virtual InframanagerObjectClass Class { get; init; } 
        
        public virtual ICollection<RoleOperation> RoleOperations { get; init; }
    }
}
