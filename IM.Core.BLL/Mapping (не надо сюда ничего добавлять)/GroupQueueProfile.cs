using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.BLL.OrganizationStructure.Groups;
using InfraManager.DAL.OrganizationStructure;
using System.Linq;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Mapping
{
    public class GroupQueueProfile : Profile
    {
        public GroupQueueProfile()
        {
            CreateMap<Group, GroupDetails>()
                .ForMember(dst => dst.ID, m => m.MapFrom(src => src.IMObjID))
                .ForMember(dst => dst.ResponsibleUserID, m => m.MapFrom(src => src.ResponsibleID))
                .ForMember(dst => dst.ResponsibleName, m => m.MapFrom(src => src.ResponsibleUser.FullName))
                .ForMember(dst => dst.QueueUserList, m => m.MapFrom(src => src.QueueUsers.Select(gu => gu.User)))
                .ForMember(dst => dst.Type, m => m.MapFrom<GroupTypeResolver, GroupType>(x => x.Type))
                .ForMember(dst => dst.ByteType, m => m.MapFrom(x => x.Type))
                .ForMember(dst => dst.QueueTypeName, m => m.MapFrom<GroupTypeNameResolver, GroupType>(x => x.Type));

            CreateMap<GroupDetails, Group>()
                .ForMember(dst => dst.ResponsibleID, m => m.MapFrom(src => src.ResponsibleUserID))
                .ForMember(dst => dst.QueueUsers, m => m.Ignore())
                .ForMember(dst => dst.Type, m => m.Ignore()) // процессим тип в бизнес логике
                .ForMember(dst => dst.ResponsibleUser, m => m.Ignore());

            CreateMap<GroupData, Group>()
                .ForMember(dst => dst.Type, m => m.Ignore()) // процессим тип в бизнес логике(нужно зарефакторить чтобы Data была одна для обновления и добавления //TODO)
                .ConstructUsing(c => new Group(c.Name, c.Note, c.ResponsibleUserID));

            CreateMap<GroupExecutorListQueryResultItem, GroupDetails>()
                .ForMember(dst => dst.Type, mapper => mapper.Ignore())
                .ForMember(dst => dst.ByteType, mapper => mapper.MapFrom(x => x.Type))
                .ForMember(dst => dst.QueueTypeName, mapper => mapper.Ignore());
        }
    }
}
