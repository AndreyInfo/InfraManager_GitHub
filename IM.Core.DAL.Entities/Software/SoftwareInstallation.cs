using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software;

public class SoftwareInstallation
{
    /// <summary>
    /// Идентификатор инсталляции ПО
    /// </summary>
    public Guid ID { get; init; }
    /// <summary>
    /// Ссылка на модель ПО
    /// </summary>
    public Guid SoftwareModelID { get; set; }
    /// <summary>
    /// Регистрационный ключ (или серийный номер) инсталляции
    /// </summary>
    public string UniqueNumber { get; set; }
    /// <summary>
    /// Дата установки ПО
    /// </summary>
    public DateTime? InstallDate { get; set; }
    /// <summary>
    /// Путь установки ПО
    /// </summary>
    public string InstallPath { get; set; }
    /// <summary>
    /// Ссылка на устройство, на котором установлена инсталляция
    /// </summary>
    public Guid DeviceID { get; set; }
    /// <summary>
    /// Класс устройства, на котором установлена инсталляция
    /// </summary>
    public ObjectClass DeviceClassID { get; set; }
    /// <summary>
    /// Ссылка на лицензию ПО
    /// </summary>
    public Guid? SoftwareLicenceID { get; set; }
    /// <summary>
    /// Версия строки
    /// </summary>
    public byte[] RowVersion { get; init; }
    /// <summary>
    /// Ссылка на серийный номер
    /// </summary>
    public Guid? SoftwareLicenceSerialNumberID { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
    public int? SoftwareExecutionCount { get; set; }
    public DateTime UtcDateCreated { get; set; }
    public DateTime? UtcDateLastDetected { get; set; }
    public byte State { get; set; }
    public string RegistryID { get; set; }

    public virtual SoftwareLicence SoftwareLicence { get; set; }
    public virtual SoftwareModel SoftwareModel { get; set; }
    public virtual ICollection<SoftwareInstallationDependances> SoftwareInstallationDependancesDependantInstallation { get; set; }
    public virtual ICollection<SoftwareInstallationDependances> SoftwareInstallationDependancesInstallation { get; set; }
}