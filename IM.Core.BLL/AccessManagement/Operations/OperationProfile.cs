using System.Linq;
using AutoMapper;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.AccessManagement.Operations;

internal class OperationProfile : Profile
{
    public OperationProfile()
    {
        CreateMap<Operation, RoleOperationNode<int>>()
            .ForMember(dst => dst.Name, m=> m.MapFrom(scr => scr.OperationName))
            .ForMember(dst => dst.Note, m=> m.MapFrom(scr => scr.Description));

        CreateMap<InframanagerObjectClass, RoleOperationNode<int>>()
            .ForMember(dst => dst.HasChild, m=> m.MapFrom(scr => scr.Operations.Any()))
            .ForMember(dst => dst.ClassID, m=> m.MapFrom(scr => scr.ID));
    }
}
