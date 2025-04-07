using AutoMapper;
using InfraManager.BLL.Accounts.Tags;
using InfraManager.DAL.Accounts;

namespace InfraManager.BLL.Accounts;

internal sealed class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagData, Tag>()
            .ForMember(dst => dst.Name, opt => opt.MapFrom(c => c.Tag));
    }
}