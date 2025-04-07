using AutoMapper;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhoursProfile : Profile
    {
        public ManhoursProfile()
        {
            CreateMap<ManhoursWork, ManhoursWorkDetails>()
                .ForMember(
                    details => details.ID, 
                    mapper => mapper.MapFrom(entity => entity.IMObjID))
                .ForMember(details => details.UserActivityTypeName, mapper => mapper.MapFrom(entity => entity.UserActivityType.Name));

            CreateMap<ManhoursEntry, ManhoursDetails>();
            CreateMap<ManhoursWorkData, ManhoursWork>()
                .ConstructUsing(d => new ManhoursWork(d.ObjectID, d.ObjectClassID));
        }
    }
}
