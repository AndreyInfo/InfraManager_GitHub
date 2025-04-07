using System;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class EntityCharacteristicsDataBase
{
    #region Общие поля характеристик модели/объекта
    public Guid ID { get; set; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
    #endregion

    #region Поля CD/DVD привода
    public string WriteSpeed { get; init; }
    public string ReadSpeed { get; init; }
    public string DriveCapabilities { get; init; }
    #endregion

    #region Поля видеоадаптера
    public string MemorySize { get; init; }
    public string VideoModeDescription { get; init; }
    public string ChipType { get; init; }
    public string DacType { get; init; }
    #endregion

    #region Поля жесткого диска
    public string FormattedCapacity { get; init; }
    public string RecordingSurfaces { get; init; }
    public string InterfaceType { get; init; }
    #endregion

    #region Поля звуковой карты
    public string DMa { get; init; }
    public string IRq { get; init; }
    #endregion

    #region Поля контроллера системы хранения данных
    public string WWn { get; init; } = "";
    #endregion

    #region Поля материнской платы
    public string PrimaryBusType { get; init; }
    public string SecondaryBusType { get; init; }
    public string ExpansionSlots { get; init; }
    public string RamSlots { get; init; }
    public string MotherboardSize { get; init; }
    public string MotherboardChipset { get; init; }
    public string MaximumSpeed { get; init; }
    #endregion

    #region Поля модема (адаптер)
    public string DataRate { get; init; }
    public string ModemTechnology { get; init; }
    public string ConnectorType { get; init; }
    #endregion

    #region Поля модуля оперативной памяти
    public string Capacity { get; init; }
    public string DeviceLocator { get; init; }
    #endregion

    #region Поля привода гибких дисков
    public string Heads { get; init; }
    public string Cylinders { get; init; }
    public string Sectors { get; init; }
    #endregion

    #region Поля процессора
    public string MaxClockSpeed { get; init; }
    public string CurrentClockSpeed { get; init; }
    public string L2cacheSize { get; init; }
    public string SocketDesignation { get; init; }
    public string Platform { get; init; }
    public string NumberOfCores { get; init; }
    #endregion

    #region Поля сетевой карты
    public string MacAddress { get; init; }
    public string MaxSpeed { get; init; }
    #endregion
}
