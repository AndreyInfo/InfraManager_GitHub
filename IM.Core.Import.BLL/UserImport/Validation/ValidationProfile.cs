using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap;
using InfraManager.ServiceBase.ImportService.LdapModels;

namespace IM.Core.Import.BLL;

public class ValidationProfile : Profile
{
    public ValidationProfile()
    {
        CreateMap<string, FieldError>()
            .ForMember(x => x.Name, x => x.MapFrom(y => y));
        
        CreateMap<IGrouping<string, FieldError>, FieldErrors>()
            .ConstructUsing(x => new FieldErrors(x.Key, x.Select(x => x.Error).ToArray()));

        CreateMap<FieldErrors[], FieldProtocol>()
            .ForMember(x => x.FieldData, x => x.MapFrom(y => y));
    }
}