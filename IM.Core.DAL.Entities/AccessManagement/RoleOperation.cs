using System;
using Inframanager;

namespace InfraManager.DAL.AccessManagement
{
    /// <summary>
    /// Этот класс представляет сущность Роль - Операция
    /// </summary>
    
    [ObjectClassMapping(ObjectClass.Role)]
    [OperationIdMapping(ObjectAction.Delete, InfraManager.OperationID.Role_Delete)]
    [OperationIdMapping(ObjectAction.Insert, InfraManager.OperationID.Role_Add)]
    [OperationIdMapping(ObjectAction.Update, InfraManager.OperationID.Role_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, InfraManager.OperationID.Role_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, InfraManager.OperationID.Role_Properties)]
    public class RoleOperation
    {
        protected RoleOperation()
        {
        }

        /// <summary>
        /// Создает экземпляр класса RoleOperation
        /// </summary>
        /// <param name="roleID"> Идентификатор Роли</param>
        /// <param name="operationID">Идентификатор Операция</param>
        public RoleOperation(Guid roleID, OperationID operationID)
        {
            RoleID = roleID;
            OperationID = operationID;
        }        

        /// <summary>
        /// Возвращает идентификатор роли
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// Возвращает идентификатор операции
        /// </summary>
        public OperationID OperationID { get; set; }

        /// <summary>
        /// Возвращает операцию
        /// </summary>
        public virtual Operation Operation { get; private set; }
    }
}
