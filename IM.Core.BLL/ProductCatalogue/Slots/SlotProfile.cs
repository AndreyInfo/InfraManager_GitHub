using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public class SlotProfile : Profile
{
    public SlotProfile()
    {
        CreateMap<SlotData, Slot>();

        CreateMap<Slot, SlotDetails>();
    }
}