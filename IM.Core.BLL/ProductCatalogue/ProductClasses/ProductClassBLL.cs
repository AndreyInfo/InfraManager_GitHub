using InfraManager.BLL.ProductCatalogue.ProductClass;

namespace InfraManager.BLL.ProductCatalogue.ProductClasses;

internal class ProductClassBLL : IProductClassBLL
    , ISelfRegisteredService<IProductClassBLL>
{
    public ObjectClass? GetModelClassByProductClass(ObjectClass productClass)
    {
        return productClass switch
        {
            ObjectClass.TerminalDevice => ObjectClass.TerminalDeviceModel,
            ObjectClass.Adapter => ObjectClass.AdapterModel,
            ObjectClass.Peripherial => ObjectClass.PeripherialModel,
            ObjectClass.ActiveDevice => ObjectClass.NetworkDeviceModel,
            ObjectClass.VirtualServer => ObjectClass.NetworkDeviceModel, // точно ли так работает?
            ObjectClass.SoftwareLicence => ObjectClass.SoftwareLicenseModel,
            ObjectClass.Material => ObjectClass.MaterialModel,
            ObjectClass.Rack => ObjectClass.CabinetType,
            ObjectClass.ServiceContract => ObjectClass.ServiceContractModel,
            _ => null //может лучше ошибку выкидывать?
        };
    }
}