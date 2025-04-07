using Inframanager;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.Software;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ServiceContractLicence)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ServiceContractLicence_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ServiceContractLicence_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ServiceContractLicence_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ServiceContractLicence_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ServiceContractLicence_Properties)]
public class ServiceContractLicence
{
    public Guid ID { get; init; }
    /// <summary>
    /// ID сервисного контракта
    /// </summary>
    public Guid ServiceContractID { get; init; }

    /// <summary>
    /// ID модели ПО
    /// </summary>
    public Guid SoftwareModelID { get; init; }

    /// <summary>
    /// Тип лицензии - клиент/сервер
    /// </summary>
    public LicenceType LicenceType { get; init; }

    /// <summary>
    /// Схема лицензирования
    /// </summary>
    public SoftwareLicenceSchemeEnum? LicenceSchemeEnum { get; init; }

    /// <summary>
    /// Количество прав
    /// </summary>
    public int? Count { get; init; }

    /// <summary>
    /// Разрешен Downgrade
    /// </summary>
    public bool CanDowngrade { get; init; }

    /// <summary>
    /// Полная версия
    /// </summary>
    public bool IsFull { get; init; }

    /// <summary>
    /// Тип
    /// </summary>
    public Guid? ProductCatalogTypeID { get; init; }

    /// <summary>
    /// Модель лицензии
    /// </summary>
    public Guid? SoftwareLicenceModelID { get; init; }

    /// <summary>
    /// Созданная лицензия
    /// </summary>
    public Guid? SoftwareLicenceID { get; init; }

    /// <summary>
    /// Стоимость
    /// </summary>
    public Decimal Cost { get; init; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Ограничение дней
    /// </summary>
    public int? LimitInDays { get; init; }

    /// <summary>
    /// Схема лицензирования
    /// </summary>
    public Guid LicenceScheme { get; init; }

    public byte[] RowVersion { get; init; }

    public virtual ServiceContract ServiceContract { get; }
    public virtual SoftwareModel SoftwareModel { get; }
    public virtual ProductCatalogType ProductCatalogType { get; }
    public virtual SoftwareLicence SoftwareLicence { get; }
}
