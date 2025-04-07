using AutoMapper;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallFromMeListItemMappingProfile : Profile
    {
        public CallFromMeListItemMappingProfile()
        {
            CreateMap<CallsFromMeListQueryResultItem, CallFromMeListItem>()
                .ForMember(m => m.ReceiptTypeString, m => m.MapFrom(x => x.ReceiptType.ToString())); //todo Add method DisplayAttributeHelper.GetResourceValueForEnum(ReceiptType, Thread.CurrentThread.CurrentUICulture.Name)

        }
    }
}
