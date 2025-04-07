using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.FormBuilder.Enums;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<SessionDetailsListItem, SessionDetails>()
            .ForMember(dst => dst.LocationName, m =>
                m.MapFrom<LocalizedEnumResolver<SessionDetailsListItem, SessionDetails, SessionLocationType>,
                    SessionLocationType>(
                    queryItem => queryItem.Location))
            .ForMember(dst => dst.LicenceType, m =>
                m.MapFrom<LocalizedEnumResolver<SessionDetailsListItem, SessionDetails, SessionLicenceType>,
                    SessionLicenceType>(
                    queryItem => queryItem.LicenceType));
    }
}