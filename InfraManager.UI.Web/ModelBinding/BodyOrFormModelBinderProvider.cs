using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ComplexDataModelBinderProvider = Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexObjectModelBinderProvider;

namespace InfraManager.UI.Web.ModelBinding
{
    public class BodyOrFormModelBinderProvider : IModelBinderProvider
    {
        private BodyModelBinderProvider _bodyModelBinderProvider;
        private ComplexDataModelBinderProvider _complexDataModelBinderProvider;

        public BodyOrFormModelBinderProvider(BodyModelBinderProvider bodyModelBinderProvider, ComplexDataModelBinderProvider complexDataModelBinderProvider)
        {
            _bodyModelBinderProvider = bodyModelBinderProvider;
            _complexDataModelBinderProvider = complexDataModelBinderProvider;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.CanAcceptDataFrom(BodyOrFormBindingSource.BodyOrDefault))
            {
                var bodyBinder = _bodyModelBinderProvider.GetBinder(context);
                var complexBinder = _complexDataModelBinderProvider.GetBinder(context);
                return new BodyOrFormModelBinder(bodyBinder, complexBinder);
            }
            return null;
        }
    }
}
