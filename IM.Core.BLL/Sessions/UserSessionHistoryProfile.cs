using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.FormBuilder.Enums;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class UserSessionHistoryProfile : Profile
{
    public UserSessionHistoryProfile()
    {
        CreateMap<UserSessionHistory, UserSessionHistoryDetails>()
            .ForMember(x => x.ExecutorFullName,
                x => x.MapFrom(x => x.Executor == null ? string.Empty : x.Executor.FullName))
            .ForMember(x => x.UserFullName, x => x.MapFrom(x => x.User.FullName))
            .ForMember(x => x.TypeString, x =>
                x.MapFrom<LocalizedEnumResolver<UserSessionHistory, UserSessionHistoryDetails, SessionHistoryType>,
                    SessionHistoryType>(x => x.Type));
    }
}