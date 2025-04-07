using System;

namespace InfraManager.DAL.Software
{    public partial class SoftwareLicenceReference
    {
        /// <summary>
        /// Идентификатор лицензии ПО
        /// </summary>
        public Guid SoftwareLicenceId { get; set; }
        /// <summary>
        /// Ссылка лицензии на внешний объект (оборудование, либо пользователя)
        /// </summary>
        public Guid ObjectId { get; set; }
        /// <summary>
        /// Класс объекта
        /// </summary>
        public int ClassId { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public int? SoftwareExecutionCount { get; set; }
        public string UniqueNumber { get; set; }
        public Guid? SoftwareLicenceSerialNumberId { get; set; }
        public Guid? SoftwareSubLicenceId { get; set; }

        public virtual SoftwareLicence SoftwareLicence { get; set; }
    }
}