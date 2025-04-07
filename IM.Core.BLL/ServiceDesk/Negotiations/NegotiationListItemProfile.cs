using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationListItemProfile : Profile
    {
        public NegotiationListItemProfile()
        {
            CreateMap<NegotiationListQueryResultItem, NegotiationListItem>()
                .ForMember(
                    reportItem => reportItem.NegotiationModeString, 
                    mapper => mapper.MapFrom<LocalizedEnumResolver<NegotiationListQueryResultItem, NegotiationListItem, NegotiationMode>, NegotiationMode>(queryItem => queryItem.NegotiationMode))
                .ForMember(
                    reportItem => reportItem.NegotiationStatusString,
                    mapper => mapper.MapFrom<LocalizedEnumResolver<NegotiationListQueryResultItem, NegotiationListItem, NegotiationStatus>, NegotiationStatus>(queryItem => queryItem.NegotiationStatus))
                .ForMember(
                    reportItem => reportItem.CategoryName,
                    mapper => mapper.MapFrom<LocalizedEnumResolver<NegotiationListQueryResultItem, NegotiationListItem, Issues>, Issues>(
                        queryItem => queryItem.CategorySortColumn));
            
        }
    }
}
