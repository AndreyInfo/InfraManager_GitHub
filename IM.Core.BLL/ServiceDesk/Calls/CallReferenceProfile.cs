using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallReferenceProfile : Profile
    {
        public CallReferenceProfile()
        {
            CreateMap<CallReference, CallReferenceData>();
            this.CreateCallReferenceMap<CallReferenceListItem>();
        }
    }
}
