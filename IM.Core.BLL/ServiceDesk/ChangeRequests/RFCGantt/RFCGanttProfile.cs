using AutoMapper;
using InfraManager.DAL.Interface.ServiceDesk.ChangeRequests.RFCGantt;
using InfraManager.DAL.ServiceDesk.ChangeRequests.RFCGantt;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.RFCGantt;

public class RFCGanttProfile : Profile
{
    public RFCGanttProfile()
    {
        CreateMap<RFCGanttResultItem, RFCGanttDetails>()
            .ForMember(m => m.Owner, o => o.MapFrom(f => new RFCUserDetails
            {
                ID = f.Owner.ID,
                FullName = f.OwnerFullName,
                PositionName = f.Owner.PositionName,
                Phone = f.Owner.Phone,
                PhoneInternal = f.Owner.PhoneInternal,
                SubdivisionFullName = f.Owner.SubdivisionFullName,
                Email = f.Owner.Email
            }))
            .ForMember(m => m.ClassID, o => o.MapFrom(f => f.TypeClass));
        CreateMap<RFCGanttUserResultItem, RFCUserDetails>();
    }
}