using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using Inframanager.DAL.ProductCatalogue.Synonyms;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

internal class SynonymProfile : Profile
{
    public SynonymProfile()
    {
        CreateMap<Synonym, SynonymOutputDetails>();

        CreateMap<SynonymDetails, Synonym>();

        CreateMap<ProductModelOutputDetails, Synonym>()
            .ForMember(x => x.ID, x => x.MapFrom(y => y.ID))
            .ForMember(x=>x.ModelName,x=>x.MapFrom(y=>y.Name))
            .ForMember(x=>x.ModelProducer,x=>x.MapFrom(y=>y.VendorName))
            .ForMember(x=>x.ClassID,x=>x.MapFrom(y=>y.TemplateClassID))
            .ForMember(x=>x.ModelID,x=>x.MapFrom(y=>y.ID));

    }
}