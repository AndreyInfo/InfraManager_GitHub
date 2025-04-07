using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestProfile : Profile
    {
        public ChangeRequestProfile()
        {
            CreateMap<ChangeRequest, ChangeRequestDetails>()
                .ForMember(
                    model => model.WorkflowSchemeID,
                    mapper => mapper.MapFrom(entity => entity.WorkflowSchemeID.HasValue ? entity.WorkflowSchemeID.ToString() : string.Empty))
                .ForMember(
                    model => model.ID,
                    mapper => mapper.MapFrom(entity => entity.IMObjID))
                .ForMember(
                    model => model.Description,
                    mapper => mapper.MapFrom(entity => entity.HTMLDescription))
                 .ForMember(
                    model => model.ServiceName,
                    mapper => mapper.MapFrom(entity => entity.Service.Name))
                .ForMember(
                    model => model.QueueName,
                    mapper => mapper.MapFrom(entity => entity.Group.Name))
                .ForMember(
                    entity => entity.FormValues,
                    mapping => mapping.MapFrom(data => data.FormValues))
                .ForMember(
                    entity => entity.TypeID,
                    mapping => mapping.MapFrom(data => data.RFCTypeID));

            CreateMap<ChangeRequestData, ChangeRequest>()
                .ConstructUsing(data => new ChangeRequest(data.ChangeRequestTypeID.Value))
                .ForMember(
                    entity => entity.Description,
                    mapper => mapper.MapFrom((data, entity) => data.HTMLDescription.RemoveHtmlTags() ?? entity.Description))
                .ForMember(
                    entity => entity.FundingAmount,
                    mapper => mapper.MapFrom(data => data.FundingAmountNumber.ToIntNullable()))
                .ForMember(
                    entity => entity.WorkflowSchemeID,
                    mapping => mapping.MapFrom(data => data.WorkflowSchemeID))
                .ForMember(
                    entity => entity.RFCTypeID,
                    mapping =>
                    {
                        mapping.Condition(data => data.TypeID != null);
                        mapping.MapFrom(data => data.TypeID);
                    })
                .ForMember(
                    entity => entity.EntityStateID,
                    mapping => mapping.MapFrom((data, entity) =>
                    {
                        if (data.EntityStateIDIsNull)
                            return null;
                        else if (entity.EntityStateID == null)
                            return data.EntityStateID;
                        else if (entity.EntityStateID != null && data.EntityStateID != null)
                            return data.EntityStateID;
                        else
                            return entity.EntityStateID;
                    }))
                .ForNullableProperty(x => x.InfluenceID)
                .ForNullableProperty(x => x.InitiatorID)
                .ForNullableProperty(x => x.UrgencyID)
                .ForNullableProperty(x => x.OwnerID)
                .ForNullableProperty(x => x.QueueID)
                .ForNullableProperty(x => x.RealizationDocumentID)
                .ForNullableProperty(x => x.RollbackDocumentID)
                .ForMember(
                    entity => entity.FormValues,
                    opt =>
                    {
                        opt.PreCondition(src => src.FormValuesData is not null);
                        opt.MapFrom(data => data.FormValuesData);
                    })
                .IgnoreOtherNulls();
            this.CreateChangeRequestReferenceMap<ChangeRequestReferenceListItem>();
        }
    }
}
