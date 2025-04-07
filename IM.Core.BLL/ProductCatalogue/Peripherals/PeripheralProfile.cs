using AutoMapper;
using InfraManager.BLL.Asset.Peripherals;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.Peripherals;
using PeripheralDetails = InfraManager.BLL.Asset.Peripherals.PeripheralDetails;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class PeripheralProfile : Profile
    {
        public PeripheralProfile()
        {
            CreateMap<Peripheral, PeripheralDetails>();

            CreateMap<PeripheralItem, PeripheralListItemDetails>();
        }
    }
}
