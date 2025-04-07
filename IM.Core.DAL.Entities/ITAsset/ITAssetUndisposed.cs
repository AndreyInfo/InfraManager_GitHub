using InfraManager.DAL.Import.ITAsset;
using System;

namespace InfraManager.DAL.ITAsset;
public class ITAssetUndisposed
{
    public Guid ID { get; init; }

    // Код причины
    public ITAssetUndisposedReasonCodeEnum ReasonCode { get; set; }

    // Идентификатор задания импорта
    public Guid? ITAssetImportSettingID { get; init; }

    // Дата и время обнаружения
    public DateTime DetectionTime { get; init; }

    // Наименование
    public string Name { get; init; }

    // Инвентарный номер
    public string InvNumber { get; init; }

    // Серийный номер
    public string SerialNumber { get; init; }

    // Код
    public string Code { get; init; }

    // Внешний идентификатор актива
    public string ExternalID { get; init; }

    // Asset tag
    public string AssetTag { get; set; }

    // Примечание
    public string Note { get; init; }

    // Описание
    public string Description { get; init; }

    // Тип Внешний идентификатор
    public string TypeExternalID { get; set; }

    // Тип Наименование
    public string TypeName { get; set; }

    // Модель Внешний идентификатор
    public string ModelExternalID { get; init; }

    // Модель Наименование
    public string ModelName { get; init; }

    // Производитель Внешний идентификатор
    public string VendorExternalID { get; init; }

    // Производитель Наименование
    public string VendorName { get; init; }

    // Поставщик Внешний идентификатор
    public string SupplierExternalID { get; init; }

    // Поставщик Наименование
    public string SupplierName { get; init; }

    // Владеет Внешний идентификатор
    public string OwnerExternalID { get; init; }

    // Владеет Наименование
    public string OwnerName { get; init; }

    // Использует Внешний идентификатор
    public string UtilizerExternalID { get; init; }

    // Использует Наименование
    public string UtilizerName { get; init; }

    // Оборудование Внешний идентификатор
    public string EquipmentExternalID { get; init; }

    // Оборудование Наименование
    public string EquipmentName { get; init; }

    // Шкаф Внешний идентификатор
    public string RackExternalID { get; init; }

    // Шкаф Наименование
    public string RackName { get; init; }

    // Рабочее место Внешний идентификатор
    public string WorkplaceExternalID { get; init; }

    // Рабочее место Наименование
    public string WorkplaceName { get; init; }

    // Комната Внешний идентификатор
    public string RoomExternalID { get; init; }

    // Комната Наименование
    public string RoomName { get; init; }

    // Местоположение
    public string Location { get; init; }

    // Здание Внешний идентификатор
    public string BuildingExternalID { get; init; }

    // Здание Наименование
    public string BuildingName { get; init; }

    // Документ
    public string Agreement { get; init; }

    // МОЛ
    public string UserName { get; init; }

    // Основание
    public string Founding { get; init; }

    // Стоимость
    public decimal? Cost { get; init; }

    // Гарантия
    public DateTime? Warranty { get; init; }

    // Дата принятия
    public DateTime? DateReceived { get; init; }

    // Пользовательское поле
    public string UserField1 { get; init; }

    // Пользовательское поле
    public string UserField2 { get; init; }

    // Пользовательское поле
    public string UserField3 { get; init; }

    // Пользовательское поле
    public string UserField4 { get; init; }

    // Пользовательское поле
    public string UserField5 { get; init; }

    // IP адрес
    public string IpAddress { get; set; }

    // MAC
    public string MacAddress { get; init; }

    // Домен
    public string Domain { get; init; }

    // Домен\Логин
    public string DomainLogin { get; init; }

    // Логин
    public string Login { get; init; }

    // Операционная система
    public string OS { get; init; }

    // Маска подсети
    public string IpMask { get; set; }

    public virtual ITAssetImportSetting ITAssetImportSetting { get; init; }
}
