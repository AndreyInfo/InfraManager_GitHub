using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.Core;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.Notification.Templates;

public class MassiveIncidentTemplateProfile : Profile
{
    public MassiveIncidentTemplateProfile()
    {
        CreateMap<MassIncident, MassIncidentTemplate>()
            .ForMember(x => x.ShortSummary, x => x.MapFrom(x => x.Name))
            .ForMember(x => x.Summary, x => x.MapFrom(x => x.Description.Plain))
            .ForMember(x => x.DateCreatedString,
                x => x.MapFrom(
                    x => DateTimeExtensions.Format(x.UtcCreatedAt, Global.DateTimeFormat)))
            .ForMember(x => x.DateRegisteredString,
                x => x.MapFrom(
                    x => x.UtcRegisteredAt.HasValue
                        ? DateTimeExtensions.Format(x.UtcRegisteredAt.Value, Global.DateTimeFormat)
                        : string.Empty))
            .ForMember(x => x.DateOpenedString,
                x => x.MapFrom(
                    x => x.UtcOpenedAt.HasValue
                        ? DateTimeExtensions.Format(x.UtcOpenedAt.Value, Global.DateTimeFormat)
                        : string.Empty))
            .ForMember(dst => dst.DatePromisedString,
                x => x.MapFrom(
                    x => x.UtcCloseUntil.HasValue
                        ? DateTimeExtensions.Format(x.UtcCloseUntil.Value, Global.DateTimeFormat)
                        : string.Empty))
            .ForMember(dst => dst.DateAccomplishedString,
                x => x.MapFrom(
                    x => x.UtcDateAccomplished.HasValue
                        ? DateTimeExtensions.Format(x.UtcDateAccomplished.Value, Global.DateTimeFormat)
                        : string.Empty))
            .ForMember(dst => dst.DateClosedString,
                x => x.MapFrom(
                    x => x.UtcDateClosed.HasValue
                        ? DateTimeExtensions.Format(x.UtcDateClosed.Value, Global.DateTimeFormat)
                        : string.Empty))
            .ForMember(x => x.MassIncidentTypeFullName, x => x.MapFrom(x => x.Type.Name))
            .ForMember(x => x.MassIncidentPriorityName, x => x.MapFrom(x => x.Priority.Name))
            .ForMember(x => x.CriticalityName, x => x.MapFrom(x => x.Criticality.Name))
            .ForMember(x => x.Remedy, x => x.MapFrom(x => x.Solution.Plain))
            .ForMember(x => x.MainService, x => x.MapFrom(x => x.Service.Name))
            .ForMember(x => x.NumberString, x => x.MapFrom(x => x.ReferenceName))
            .ForMember(dst => dst.NumberOnly, opt => opt.MapFrom(src => src.ID.ToString()))
            .ForMember(x => x.RequestForChangeCountString, x => x.MapFrom(x => x.ChangeRequests.Count))
            .ForMember(x => x.CategoryName, x => x.MapFrom(x => x.TechnicalFailureCategory.Name))
            .ForMember(x => x.GroupName, x => x.MapFrom(x => x.ExecutedByGroup.Name))
            .ForMember(x => x.HTMLSummary, x => x.MapFrom(x => x.Description.Formatted))
            .ForMember(x => x.HTMLRemedy, x => x.MapFrom(x => x.Solution.Formatted))
            .ForMember(x => x.Cause, x => x.MapFrom(x => x.Cause.Plain))
            .ForMember(x => x.HTMLCause, x => x.MapFrom(x => x.Cause.Formatted))
            .ForMember(x => x.MICause, x => x.MapFrom(x => x.MassIncidentCause.Name))
            .ForMember(x => x.ReceiptTypeString,
                x =>
                {
                    x.PreCondition(x => x.MassIncidentInformationChannel != null);
                    x.MapFrom<MassiveIncidentInformationStringChannelNameResolver, string>(x =>
                        x.MassIncidentInformationChannel.Name);
                })
            .ForMember(dst => dst.CreatedByBuildingName, opt => opt.MapFrom(src => src.CreatedBy== null ? null : src.CreatedBy.BuildingName))
            .ForMember(dst => dst.CreatedByEmail, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Email))
            .ForMember(dst => dst.CreatedByFax, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Fax))
            .ForMember(dst => dst.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Name))
            .ForMember(dst => dst.CreatedByFullName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.FullName))
            .ForMember(dst => dst.CreatedBySurname, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Surname))
            .ForMember(dst => dst.CreatedByNumber, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Number))
            .ForMember(dst => dst.CreatedByOrganizationName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.OrganizationName))
            .ForMember(dst => dst.CreatedByPager, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Pager))
            .ForMember(dst => dst.CreatedByPatronymic, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Patronymic))
            .ForMember(dst => dst.CreatedByPhone, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.Phone))
            .ForMember(dst => dst.CreatedByPositionName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.PositionName))
            .ForMember(dst => dst.CreatedByRoomName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.RoomName))
            .ForMember(dst => dst.CreatedBySubdivisionName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.SubdivisionName))
            .ForMember(dst => dst.CreatedByWorkplaceName, opt => opt.MapFrom(src => src.CreatedBy == null ? null : src.CreatedBy.WorkplaceName))
            .ForMember(dst => dst.ExecutorBuildingName, opt => opt.MapFrom(src => src.ExecutedByUser== null ? null : src.ExecutedByUser.BuildingName))
            .ForMember(dst => dst.ExecutorEmail, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Email))
            .ForMember(dst => dst.ExecutorFax, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Fax))
            .ForMember(dst => dst.ExecutorName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Name))
            .ForMember(dst => dst.ExecutorFullName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.FullName))
            .ForMember(dst => dst.ExecutorSurname, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Surname))
            .ForMember(dst => dst.ExecutorNumber, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Number))
            .ForMember(dst => dst.ExecutorOrganizationName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.OrganizationName))
            .ForMember(dst => dst.ExecutorPager, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Pager))
            .ForMember(dst => dst.ExecutorPatronymic, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Patronymic))
            .ForMember(dst => dst.ExecutorPhone, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.Phone))
            .ForMember(dst => dst.ExecutorPositionName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.PositionName))
            .ForMember(dst => dst.ExecutorRoomName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.RoomName))
            .ForMember(dst => dst.ExecutorSubdivisionName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.SubdivisionName))
            .ForMember(dst => dst.ExecutorWorkplaceName, opt => opt.MapFrom(src => src.ExecutedByUser == null ? null : src.ExecutedByUser.WorkplaceName))
            .ForMember(dst => dst.OwnedByBuildingName, opt => opt.MapFrom(src => src.OwnedBy== null ? null : src.OwnedBy.BuildingName))
            .ForMember(dst => dst.OwnedByEmail, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Email))
            .ForMember(dst => dst.OwnedByFax, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Fax))
            .ForMember(dst => dst.OwnedByName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Name))
            .ForMember(dst => dst.OwnedByFullName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.FullName))
            .ForMember(dst => dst.OwnedBySurname, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Surname))
            .ForMember(dst => dst.OwnedByNumber, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Number))
            .ForMember(dst => dst.OwnedByOrganizationName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.OrganizationName))
            .ForMember(dst => dst.OwnedByPager, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Pager))
            .ForMember(dst => dst.OwnedByPatronymic, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Patronymic))
            .ForMember(dst => dst.OwnedByPhone, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.Phone))
            .ForMember(dst => dst.OwnedByPositionName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.PositionName))
            .ForMember(dst => dst.OwnedByRoomName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.RoomName))
            .ForMember(dst => dst.OwnedBySubdivisionName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.SubdivisionName))
            .ForMember(dst => dst.OwnedByWorkplaceName, opt => opt.MapFrom(src => src.OwnedBy == null ? null : src.OwnedBy.WorkplaceName));
    }
    
    
    private class MassiveIncidentInformationStringChannelNameResolver : IMemberValueResolver<MassIncident, MassIncidentTemplate, string, string>
    {
        private readonly ILocalizeText _localizer;

        public MassiveIncidentInformationStringChannelNameResolver(ILocalizeText localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(MassIncident source, MassIncidentTemplate destination, string sourceMember, string destMember,
            ResolutionContext context)
        {
            return _localizer.Localize(source.MassIncidentInformationChannel.ResourceKey, "ru-RU");
        }
    }
}
