using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentProfile : Profile
    {
        public MassIncidentProfile()
        {
            CreateMap<MassIncident, MassIncidentDetails>()
                .ForMember(
                    details => details.CreatedByUserID,
                    mapper => mapper.MapFrom(entity => entity.CreatedByUserID == User.NullUserId ? null : (Guid?)entity.CreatedBy.IMObjID))
                .ForMember(
                    details => details.ExecutedByUserID,
                    mapper => mapper.MapFrom(entity => entity.ExecutedByUserID == User.NullUserId ? null : (Guid?)entity.ExecutedByUser.IMObjID))
                .ForMember(
                    details => details.PriorityColor,
                    mapper => mapper.MapFrom(entity => entity.Priority.Color))
                .ForMember(
                    details => details.OwnedByUserID,
                    mapper => mapper.MapFrom(entity => entity.OwnedByUserID == User.NullUserId ? null : (Guid?)entity.OwnedBy.IMObjID))
                .ForMember(
                    details => details.ExecutedByGroupID,
                    mapper => mapper.MapFrom(entity => entity.ExecutedByGroupID == Group.NullGroupID ? null : (Guid?)entity.ExecutedByGroupID))
                .ForMember(
                    details => details.ServiceID,
                    mapper => mapper.MapFrom(entity => entity.ServiceID))
                .ForMember(
                    details => details.FormValues,
                    mapper =>
                    {
                        mapper.PreCondition(entity => entity.FormValues is not null);
                        mapper.MapFrom(entity => entity.FormValues);
                    })
                .ForMember(details => details.ProblemCount,
                    mapper => mapper.MapFrom(entity => entity.Problems.Count()))
                .ForMember(details => details.CallCount,
                    mapper => mapper.MapFrom(entity => entity.Calls.Count()))
                .ForMember(details => details.RFCCount,
                    mapper => mapper.MapFrom(entity => entity.ChangeRequests.Count()))
                ;

            IgnoreUsers(CreateMap<MassIncidentData, MassIncident>())                
                .ForMember(
                    entity => entity.UtcCreatedAt,
                    mapper => mapper.MapFrom(data => data.UtcDateCreated))
                .ForMember(
                    entity => entity.UtcRegisteredAt,
                    mapper => mapper.MapFrom(data => data.UtcDateRegistered))
                .ForNullableProperty(x => x.CauseID)
                .ForMember(
                    x => x.FormValues,
                    mapper =>
                    {
                        mapper.PreCondition(data => data.FormValuesData is not null);
                        mapper.MapFrom(data => data.FormValuesData);
                    })
                .IgnoreNulls();

            IgnoreUsers(CreateMap<NewMassIncidentData, MassIncident>())
                .ForMember(x => x.Calls, mapper => mapper.Ignore()) // не мапится автоматически
                .ForMember(
                    entity => entity.FormValues,
                    mapper =>
                    {
                        mapper.PreCondition(data => data.FormValuesData is not null);
                        mapper.MapFrom(data => data.FormValuesData);
                    });

            CreateMap<AllMassIncidentsReportQueryResultItem, AllMassIncidentsReportItem>()
                .ForMember(
                    reportItem => reportItem.OwnerID,
                    mapper => mapper.MapFrom(queryItem => queryItem.OwnerID == User.NullUserGloablIdentifier ? null : (Guid?)queryItem.OwnerID))
                .ForMember(
                    reportItem => reportItem.InformationChannel,
                    mapper => mapper.MapFrom<InformationChannelResolver, short>(queryItem => queryItem.InformationChannelID));

            CreateMap<MassIncidentsReportQueryResultItem, MassIncidentsToAssociateReportItem>();

            CreateMap<MassIncidentsReportQueryResultItem, ProblemMassIncidentsReportItem>();
        }

        private static IMappingExpression<T, MassIncident> IgnoreUsers<T>(IMappingExpression<T, MassIncident> mapper)
            where T : BaseMassIncidentData
        {
            return mapper
                .ForMember(x => x.ExecutedByGroupID, mapper => mapper.Ignore()) // не мапится автоматически
                .ForMember(x => x.CreatedByUserID, mapper => mapper.Ignore()) // не мапится автоматически                
                .ForMember(x => x.OwnedByUserID, mapper => mapper.Ignore()) // не мапится автоматически
                .ForMember(x => x.ExecutedByUserID, mapper => mapper.Ignore()); // не мапится автоматически
        }

        private class InformationChannelResolver : IMemberValueResolver<AllMassIncidentsReportQueryResultItem, AllMassIncidentsReportItem, short, string>
        {
            private readonly IMassIncidentInformationChannelBLL _informationChannelsBLL;

            public InformationChannelResolver(IMassIncidentInformationChannelBLL informationChannelsBLL)
            {
                _informationChannelsBLL = informationChannelsBLL;
            }

            public string Resolve(AllMassIncidentsReportQueryResultItem source, AllMassIncidentsReportItem destination, short sourceMember, string destMember, ResolutionContext context)
            {
                return _informationChannelsBLL.Find(sourceMember).Name;
            }
        }
    }
}
