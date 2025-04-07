using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallListItemMappingProfile : Profile
    {
        public CallListItemMappingProfile()
        {
            CreateMap<AllCallsQueryResultItem, CallListItem>()
                .ForMember(m => m.ManhoursInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<AllCallsQueryResultItem, CallListItem>,
                            int>(
                            item => item.Manhours))
                .ForMember(m => m.ManhoursNormInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<AllCallsQueryResultItem, CallListItem>,
                            int>(
                            item => item.ManhoursNorm))
                .ForMember(
                    m => m.ReceiptTypeString,
                    mapper => mapper.MapFrom<
                        LocalizedEnumResolver<AllCallsQueryResultItem, CallListItem, CallReceiptType>,
                        CallReceiptType>(call => call.ReceiptType));

        }
    }
}
