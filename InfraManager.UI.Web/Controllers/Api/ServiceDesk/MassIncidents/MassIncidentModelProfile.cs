using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;
using System;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MassIncidents
{
    public class MassIncidentModelProfile : Profile
    {
        public MassIncidentModelProfile()
        {
            CreateMap<MassIncidentDetails, MassIncidentDetailsModel>()
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(
                    model => model.SlaID,
                    mapper => mapper.MapFrom(details => details.ServiceLevelAgreementID ?? Guid.Empty))
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom(details => $"/api/massincidents/{details.ID}"))
                .ForMember(
                    model => model.InformationChannelUri,
                    mapper => mapper.MapFrom(details => $"/api/massincidentinformationchannels/{details.InformationChannelID}"))
                .ForMember(
                    model => model.TypeUri,
                    mapper => mapper.MapFrom(details => $"/api/massincidenttypes/{details.TypeID}"))
                .ForMember(
                    model => model.PriorityUri,
                    mapper => mapper.MapFrom(details => $"/api/priorities/{details.PriorityID}"))
                .ForMember(
                    model => model.CauseUri,
                    mapper => mapper.MapFrom(details => details.CauseID.HasValue ? $"/api/massincidentcauses/{details.CauseID.Value}" : null))
                .ForMember(
                    model => model.CreatedByUserUri,
                    mapper => mapper.MapFrom(details => details.CreatedByUserID.HasValue ? $"/api/users/{details.CreatedByUserID}" : null))
                .ForMember(
                    model => model.OwnedByUserUri,
                    mapper => mapper.MapFrom(details => details.OwnedByUserID.HasValue ? $"/api/users/{details.OwnedByUserID.Value}" : null))
                .ForMember(
                    model => model.ExecutedByUserUri,
                    mapper => mapper.MapFrom(details => details.ExecutedByUserID.HasValue ? $"/api/users/{details.ExecutedByUserID.Value}" : null))
                .ForMember(
                    model => model.ExecutedByGroupUri,
                    mapper => mapper.MapFrom(details => details.ExecutedByGroupID.HasValue ? $"/api/groups/{details.ExecutedByGroupID.Value}" : null))
                .ForMember(
                    model => model.ServiceUri,
                    mapper => mapper.MapFrom(details => $"/api/services/{details.ServiceID}"))
                .ForMember(
                    model => model.CriticalityUri,
                    mapper => mapper.MapFrom(details => $"/api/criticalities/{details.CriticalityID}"))
                .ForMember(
                    model => model.TechnicalFailureCategoryUri,
                    mapper => mapper.MapFrom(
                        details => details.TechnicalFailureCategoryID.HasValue
                            ? $"/api/technicalFailuresCategories/{details.TechnicalFailureCategoryID.Value}"
                            : null))
                .ForMember(
                    model => model.FormValues,
                    mapper =>
                    {
                        mapper.PreCondition(details => details.FormValues is not null);
                        mapper.MapFrom(details => details.FormValues);
                    });

            CreateMap<AllMassIncidentsReportItem, AllMassIncidentsReportItemModel>()
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom(item => $"/api/massIncidents/{item.ID}"))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());

            CreateMap<MassIncidentReferenceDetails, ServiceReferenceModel>()
                .ForMember(
                    model => model.ServiceUri,
                    mapper => mapper.MapFrom(
                        details =>  $"/api/services/{details.ReferenceID}"));

            CreateMap<MassIncidentsToAssociateReportItem, MassIncidentsReportItemModel>();
            CreateMap<ProblemMassIncidentsReportItem, MassIncidentsReportItemModel>();
            CreateMap<MassIncidentReferencedCallListItem, CallReferenceListItemModel>();
            CreateMap<MassIncidentReferencedChangeRequestListItem, ChangeRequestReferenceListItemModel>();
            CreateMap<MassIncidentReferencedProblemListItem, ProblemReferenceListItemModel>();
            CreateMap<MassIncidentReferencedWorkOrderListItem, WorkOrderReferenceListItemModel>();
        }
    }
}
