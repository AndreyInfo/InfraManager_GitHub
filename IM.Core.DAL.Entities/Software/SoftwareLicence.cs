using Inframanager;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software
{
    [ObjectClassMapping(ObjectClass.SoftwareLicence)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareLicence_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.SoftwareLicence_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareLicence_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.SoftwareLicence_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SoftwareLicence_Properties)]
    public partial class SoftwareLicence
    {
        /// <summary>
        /// Идентификатор лицензии ПО
        /// </summary>
        public Guid ID { get; init; }
        /// <summary>
        /// Название лицензии ПО
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание лицензии ПО
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Дата начала действия лицензии ПО
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// Дата окончания действия лицензии ПО
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Ссылка на родительскую лицензию ПО
        /// </summary>
        public Guid? ParentID { get; set; }
        /// <summary>
        /// Ссылка на адаптер, если это лицензия на HASP
        /// </summary>
        public Guid? HaspadapterID { get; set; }
        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        public int RoomIntID { get; set; }
        public string InventoryNumber { get; set; }
        /// <summary>
        /// Версия строки
        /// </summary>
        public byte[] RowVersion { get; init; }
        public Guid? PeripheralDatabaseID { get; set; }
        public Guid? ComplementaryID { get; set; }
        public Guid? SoftwareLicenceModelID { get; set; }
        public Guid? ProductCatalogTypeID { get; set; }
        public Guid SoftwareModelID { get; set; }
        //TODO это скорее всего отдельная enum найти поменять
        public byte SoftwareLicenceType { get; set; }
        public byte? SoftwareLicenceSchemeEnum { get; set; }
        public int? SoftwareExecutionCount { get; set; }
        public int? LimitInHours { get; set; }
        public bool DowngradeAvailable { get; set; }
        public bool IsFull { get; set; }
        public Guid SoftwareLicenceScheme { get; set; }
        public int? RestrictionsCpuFrom { get; set; }
        public int? RestrictionsCpuTill { get; set; }
        public int? RestrictionsCoreFrom { get; set; }
        public int? RestrictionsCoreTill { get; set; }
        public int? RestrictionsHzFrom { get; set; }
        public int? RestrictionsHzTill { get; set; }
        public Guid? OemdeviceID { get; set; }
        public ObjectClass? OemdeviceClassID { get; set; }

        public virtual SoftwareLicence Parent { get; set; }
        public virtual ProductCatalogType ProductCatalogType { get; set; }
        public virtual SoftwareModel SoftwareModel { get; set; }
        public virtual ICollection<SoftwareLicence> InverseParent { get; set; }
        public virtual ICollection<SoftwareInstallation> SoftwareInstallation { get; set; }
        public virtual ICollection<SoftwareLicenceReference> SoftwareLicenceReference { get; set; }
    }
}
