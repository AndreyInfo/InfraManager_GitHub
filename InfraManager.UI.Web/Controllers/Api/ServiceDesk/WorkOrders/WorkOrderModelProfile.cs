using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.Web.BLL.Helpers;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    public class WorkOrderModelProfile : Profile
    {
        public WorkOrderModelProfile()
        {
            CreateMap<WorkOrderFinancePurchaseDataModel, WorkOrderFinancePurchaseData>();
            CreateMap<WorkOrderDataModel, WorkOrderData>()
                .ForMember(
                    data => data.TypeID,
                    mapper => mapper.MapFrom(model => model.WorkOrderTypeID))
                .ForMember(
                    data => data.RowVersion,
                    mapper => mapper.Condition(model => model.RowVersion != null && model.RowVersion.Length > 0));

            CreateMap<WorkOrderFinancePurchaseDetails, WorkOrderFinancePurchaseDetailsModel>();
            CreateMap<WorkOrderDetails, WorkOrderDetailsModel>()
                .ForMember(
                    model => model.PriorityColor,
                    mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(
                    model => model.ManhoursString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<WorkOrderDetails, WorkOrderDetailsModel>,
                            int>(
                            details => details.ManhoursInMinutes))
                .ForMember(
                    model => model.ManhoursNormString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<WorkOrderDetails, WorkOrderDetailsModel>,
                            int>(
                            details => details.ManhoursNormInMinutes))
                .ForMember(
                    model => model.WorkOrderReference,
                    mapper =>
                        mapper.MapFrom<
                            PathResolver<WorkOrderDetails, WorkOrderDetailsModel>,
                            InframanagerObject?>(
                                details => details.WorkOrderReference))
                .ForMember(
                    model => model.FinishedString,
                    mapper =>
                        mapper.MapFrom<
                            YesNoResolver<WorkOrderDetails, WorkOrderDetailsModel>, bool>(
                                details => details.IsFinished))
                 .ForMember(
                    model => model.OverdueString,
                    mapper =>
                        mapper.MapFrom<
                            YesNoResolver<WorkOrderDetails, WorkOrderDetailsModel>, bool>(
                                details => details.IsOverdue))
                 .ForMember(
                    model => model.IsActiveString,
                    mapper =>
                        mapper.MapFrom<
                            YesNoResolver<WorkOrderDetails, WorkOrderDetailsModel>, bool>(
                                details => details.IsActive))
                 .ForMember(
                    model => model.BudgetObject,
                    mapper =>
                        mapper.MapFrom<
                            PathResolver<WorkOrderDetails, WorkOrderDetailsModel>,
                            InframanagerObject?>(
                                details => details.BudgetObject))
                 .ForMember(
                        wo => wo.WorkflowImageSource, 
                        mapper => mapper.MapFrom(
                            model => ImageHelper.GetValidPath(
                                ImageHelper.GetWorkflowIconSource(model.WorkflowSchemeIdentifier, model.WorkflowSchemeVersion, model.EntityStateName))));
            CreateMap<ReferencedWorkOrderListItem, WorkOrderListItemModel>();
            CreateMap<WorkOrderListItem, WorkOrderListItemModel>()
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                            PathResolver<WorkOrderListItem, WorkOrderListItemModel>,
                            InframanagerObject?>(
                                item => new InframanagerObject(item.ID, item.ClassID)))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());
            CreateMap<InventoryListItem, InventoryListItemModel>();
            CreateMap<WorkOrderReferenceListItem, WorkOrderReferenceListItemModel>();
        }
    }
}
