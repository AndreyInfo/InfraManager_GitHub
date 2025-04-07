namespace InfraManager.DAL.ITAsset;
public enum ITAssetUndisposedReasonCodeEnum : byte
{
    /// <summary>
    /// Внешний иденитификатор не уникален 
    /// </summary>
    ExternalIDNotUniq = 0,

    /// <summary>
    /// Инвентарный номер не уникален
    /// </summary>
    InvNumberNotUniq = 1,

    /// <summary>
    /// Серийный номер не уникален
    /// </summary>
    SerialNumberIDNotUniq = 2,

    /// <summary>
    /// Код не уникален
    /// </summary>
    CodeNotUniq = 3,

    /// <summary>
    /// AssetTag не уникален
    /// </summary>
    AssetTagNotUniq = 4,

    /// <summary>
    /// Тип не указан в данных импорта
    /// </summary>
    TypeNotSpecified = 5,

    /// <summary>
    /// Указанный тип не найден
    /// </summary>
    TypeNotFound = 6,

    /// <summary>
    /// Модель не указана в данных импорта
    /// </summary>
    ModelNotSpecified = 7,

    /// <summary>
    /// Указанная модель не найдена
    /// </summary>
    ModelNotFound = 8,

    /// <summary>
    /// Местоположение не указано в данных импорта
    /// </summary>
    LocationNotSpecified = 9,

    /// <summary>
    /// Указанное местоположение не найдено
    /// </summary>
    LocationNotFound = 10,

    /// <summary>
    /// Указан неверный тип местоположения
    /// </summary>
    LocationNotCorrect = 11
}
