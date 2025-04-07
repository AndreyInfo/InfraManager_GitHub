using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software
{
    [ObjectClassMapping(ObjectClass.SoftwareType)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareType_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.SoftwareType_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareType_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.SoftwareType_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SoftwareType_Properties)]
    public partial class SoftwareType
    {
        /// <summary>
        /// Идентификатор типа ПО
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Ссылка на родительский тип ПО
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Название типа ПО
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание типа ПО
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Версия строки
        /// </summary>
        public byte[] RowVersion { get; set; }
        public Guid? ComplementaryId { get; set; }

        public virtual SoftwareType Parent { get; set; }
        public virtual ICollection<SoftwareType> InverseParent { get; set; }
        public virtual ICollection<SoftwareModel> SoftwareModel { get; set; }
    }
}