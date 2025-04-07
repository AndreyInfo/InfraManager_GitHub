using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;
internal class SlotTemplateProfile : Profile
{
    public SlotTemplateProfile()
    {
        CreateMap<SlotTemplate, SlotTemplateDetails>();
        CreateMap<SlotTemplateData, SlotTemplate>();
    }
}
