using AutoMapper;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

internal class UIDBFieldsProfile : Profile
{
    public UIDBFieldsProfile()
    {
        CreateMap<UIDBFields, UIDBFieldsOutputDetails>()
            .ForMember(x => x.FieldName,
                member => member.MapFrom(x => Enum.GetName((ConcordanceObjectType) x.FieldID)));

        CreateMap<UIDBFieldsData, UIDBFields>()
            .ForMember(x=>x.FieldID, member=>member.MapFrom(x=>(long)Enum.Parse<ConcordanceObjectType>(x.FieldName)));

        CreateMap<UIDBFieldsData,UIDBFieldsOutputDetails>().ReverseMap();
    }
}