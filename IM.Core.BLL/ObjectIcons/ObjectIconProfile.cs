using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.ObjectIcons
{
    public class ObjectIconProfile : Profile
    {
        public ObjectIconProfile()
        {
            CreateMap<ObjectIcon, ObjectIconDetails>();
            CreateMap<InframanagerObject, ObjectIconDetails>()
                .ForMember(
                    details => details.ObjectID,
                    mapper => mapper.MapFrom(id => id.Id))
                .ForMember(
                    details => details.ObjectClassID,
                    mapper => mapper.MapFrom(id => id.ClassId));
            CreateMap<ObjectIconData, ObjectIcon>();
        }
    }
}
