namespace InfraManager.DAL.ProductCatalogue;

public enum ServiceContractFeatureEnum : byte
{
    /// <summary>
    /// Простой контракт
    /// </summary>
    SimpleContract = 0,
    /// <summary>
    /// Контракт поставки ПО
    /// </summary>
    SoftwareSupply = 1,
    /// <summary>
    /// Контракт аренды ПО
    /// </summary>
    SoftwareRent = 2,
    /// <summary>
    /// Контракт на обновление ПО
    /// </summary>
    SoftwareUpdate = 3,
    /// <summary>
    /// Контракт на обслуживание/поддержку ПО
    /// </summary>
    SoftwareMaintenance = 4,
    /// <summary>
    /// Контракт на обслуживание/поддержку оборудования
    /// </summary>
    EquipmentMaintenance = 5,
}
