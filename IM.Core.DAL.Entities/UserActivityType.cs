using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот класс представляет сущность Виды деятельности
    /// </summary>
    [ObjectClassMapping(ObjectClass.UserActivityType)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UserActivityType_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UserActivityType_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UserActivityType_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class UserActivityType : Lookup, IMarkableForDelete
    {
        public UserActivityType() : base()
        {
            ID = Guid.NewGuid();
        }

        public UserActivityType(string name) : base(name)
        {
        }

        /// <summary>
        /// Возвращает или задает родителя
        /// </summary>
        public Guid? ParentID { get; set; }
        public virtual UserActivityType Parent { get; set; }
        public bool Removed { get; set; }
        public virtual ICollection<UserActivityType> Childs { get; init; }
        public virtual ICollection<UserActivityTypeReference> References { get; init; }

        public string FullName => string.Concat(Parent == null ? string.Empty : $"{Parent.FullName} \\ ", Name);

        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
