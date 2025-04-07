using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Settings.UserFields;
using System;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderProfile : Profile
    {
        public WorkOrderProfile()
        {
            CreateMap<WorkOrderFinancePurchaseData, WorkOrderFinancePurchase>().IgnoreNulls();

            CreateMap<WorkOrderData, WorkOrder>()
                .ConstructUsing(data => data.FinancePurchase != null 
                    ? new WorkOrder(data.TypeID.Value, data.FinancePurchase.Concord, data.FinancePurchase.Bill, data.FinancePurchase.DetailBudget) 
                    : new WorkOrder(data.TypeID.Value))
                .ForMember(
                    entity => entity.Description,
                    mapping =>
                    {
                        mapping.Condition(data => data.Description != null);
                        mapping.MapFrom(data => data.Description.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLDescription,
                    mapping =>
                    {
                        mapping.Condition(data => data.Description != null);
                        mapping.MapFrom(data => data.Description);
                    })
                .ForNullableProperty(data => data.ExecutorID)
                .ForNullableProperty(data => data.AssigneeID)
                .ForMember(entity => entity.RowVersion, mapper => mapper.Condition(data => data.RowVersion != null))
                .ForMember(
                    entity => entity.EntityStateID,
                    mapping => mapping.MapFrom((data, entity) => {
                        if(data.EntityStateIDIsNull)
                            return null;
                        else if (entity.EntityStateID == null)
                            return data.EntityStateID;
                        else if (entity.EntityStateID != null && data.EntityStateID != null)
                            return data.EntityStateID;
                        else
                            return entity.EntityStateID;
                    }))
                .ForMember(
                    entity => entity.WorkflowSchemeID,
                    mapping => mapping.MapFrom(data => data.WorkflowSchemeID))
                .ForMember(
                    entity => entity.FormValues,
                    opt => {
                        opt.PreCondition(src => src.FormValuesData is not null);
                        opt.MapFrom(data => data.FormValuesData);
                    })
                .ForMember(entity => entity.FinancePurchase, mapper =>
                {
                    mapper.Condition(data => data.FinancePurchase != null);
                    mapper.MapFrom((data,_,entity,ctx) =>
                        ctx.Mapper.Map(data.FinancePurchase, entity));
                })
                .ForMember(entity => entity.QueueID,
                    mapper => mapper.MapFrom(data => data.QueueID)) // Добавлено чтобы не игнорировались NULL
                .IgnoreOtherNulls();

            CreateMap<WorkOrderFinancePurchase, WorkOrderFinancePurchaseDetails>();

            CreateMap<WorkOrder, WorkOrderDetails>()
                .ForMember(
                    details => details.ID,
                    mapper => mapper.MapFrom(entity => entity.IMObjID))
                .ForMember(
                    details => details.PriorityColor,
                    mapper => mapper.MapFrom(entity => entity.Priority.Color))
                .ForMember(
                    details => details.ReferencedObjectID,
                    mapper => mapper.MapFrom(
                        entity => entity.WorkOrderReference == null
                                || entity.WorkOrderReference.ObjectID == Guid.Empty 
                            ? (Guid?)null 
                            : entity.WorkOrderReference.ObjectID))
                .ForMember(
                    details => details.ReferencedObjectClassID,
                    mapper => mapper.MapFrom(
                        entity => entity.WorkOrderReference == null
                                || entity.WorkOrderReference.ObjectID == Guid.Empty
                            ? (ObjectClass?)null
                            : entity.WorkOrderReference.ObjectClassID))
                .ForMember(
                    details => details.WorkOrderReferenceString,
                    mapper => mapper.MapFrom(
                        entity => entity.WorkOrderReference == null 
                            ? string.Empty 
                            : entity.WorkOrderReference.ReferenceName))
                .ForMember(
                    details => details.WorkOrderReference,
                    mapper => mapper.MapFrom(
                        entity => entity.WorkOrderReference == null
                                || entity.WorkOrderReference.ObjectID == Guid.Empty
                            ? (InframanagerObject?)null
                            : new InframanagerObject(entity.WorkOrderReference.ObjectID, entity.WorkOrderReference.ObjectClassID)))
                .ForMember(
                    details => details.BudgetObject,
                    mapper => mapper.MapFrom(
                        entity => entity.BudgetUsage != null 
                                && entity.BudgetUsage.BudgetObjectID.HasValue
                            ? new InframanagerObject(entity.BudgetUsage.BudgetObjectID.Value, entity.BudgetUsage.BudgetObjectClassID.Value)
                            : (InframanagerObject?)null))
                .ForMember(
                    details => details.WorkOrderTypeClass,
                    mapper => mapper.MapFrom(
                        entity => entity.Type.TypeClass))
                .ForMember(
                    details => details.DocumentCount,
                    mapper => mapper.MapFrom(
                        entity => entity.Aggregate.DocumentCount))
                .ForMember(
                    details => details.IsActive,
                    mapper => mapper.MapFrom(
                        entity => entity.IsActive > 0))
                .ForMember(
                    details => details.UserField1Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<WorkOrder, WorkOrderDetails>,
                        UserField>(_ => new UserField(UserFieldType.WorkOrder, FieldNumber.UserField1)))
                .ForMember(
                    details => details.UserField2Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<WorkOrder, WorkOrderDetails>,
                        UserField>(_ => new UserField(UserFieldType.WorkOrder, FieldNumber.UserField2)))
                .ForMember(
                    details => details.UserField3Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<WorkOrder, WorkOrderDetails>,
                        UserField>(_ => new UserField(UserFieldType.WorkOrder, FieldNumber.UserField3)))
                .ForMember(
                    details => details.UserField4Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<WorkOrder, WorkOrderDetails>,
                        UserField>(_ => new UserField(UserFieldType.WorkOrder, FieldNumber.UserField4)))
                .ForMember(
                    details => details.UserField5Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<WorkOrder, WorkOrderDetails>,
                        UserField>(_ => new UserField(UserFieldType.WorkOrder, FieldNumber.UserField5)))
                .ForMember(
                    entity => entity.FormValues,
                    mapping => mapping.MapFrom(data => data.FormValues))
                .ForMember(
                    entity => entity.QueueName,
                    mapping =>
                    {
                        mapping.Condition(m => m.QueueID.HasValue);
                        mapping.MapFrom(data => data.Group.Name);
                    }
                );
        }
    }
}
