namespace InfraManager.DAL.Import.ITAsset;
public enum ITAssetIdentificationParameterEnum : byte
{
    /// <summary>
    /// AssetTag
    /// </summary>
    AssetTag = 0,

    /// <summary>
    /// MAC
    /// </summary>
    MAC = 1,

    /// <summary>
    /// Инвентарный номер
    /// </summary>
    InvNumber = 2,

    /// <summary>
    /// Серийный номер
    /// </summary>
    SerialNumber = 3,

    /// <summary>
    /// Внешний ИД
    /// </summary>
    ExternalIdentifier = 4,

    /// <summary>
    /// Код
    /// </summary>
    Code = 5,

    /// <summary>
    /// IP адрес
    /// </summary>
    IpAddress = 6,

    /// <summary>
    /// Название (сетевое имя)
    /// </summary>
    NetworkName = 7,

    /// <summary>
    /// IP адрес и Название
    /// </summary>
    IPWithNetwork = 8
}
