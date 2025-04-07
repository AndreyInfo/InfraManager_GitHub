using FluentValidation;
using Inframanager.BLL;
using Inframanager.BLL.Validation;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderDataValidator : FluentValidator<WorkOrderData>,
        ISelfRegisteredService<IValidateObject<WorkOrderData>>
    {
        public WorkOrderDataValidator(ILocalizeText localizer) : base(localizer)
        {
        }

        protected override void Initializing()
        {
            RuleFor(x => x.TypeID).NotNull().WithMessage("WorkOrder Type is required");
        }
    }
}
