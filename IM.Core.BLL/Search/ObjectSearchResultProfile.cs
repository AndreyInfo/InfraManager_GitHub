using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Search
{
    public class ObjectSearchResultProfile : Profile
    {
        public ObjectSearchResultProfile()
        {
            CreateMap<User, ObjectSearchResult>()
                .ForMember(
                    result => result.ID,
                    mapper => mapper.MapFrom(user => user.IMObjID))
                .ForMember(
                    result => result.ClassID,
                    mapper => mapper.MapFrom(_ => ObjectClass.User))
                .ForMember(
                    result => result.FullName,
                    mapper => mapper.MapFrom(user => User.GetFullName(user.IMObjID))); // TODO: Заменить на спецификацию
        }
    }
}
