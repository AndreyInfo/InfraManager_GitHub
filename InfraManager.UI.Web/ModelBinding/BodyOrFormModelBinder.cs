using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.ModelBinding
{
    public class BodyOrFormModelBinder : IModelBinder
    {
        private readonly IModelBinder _bodyBinder;
        private readonly IModelBinder _complexBinder;

        public BodyOrFormModelBinder(IModelBinder bodyBinder, IModelBinder complexBinder)
        {
            _bodyBinder = bodyBinder;
            _complexBinder = complexBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _bodyBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet)
            {
                return;
            }
            bindingContext.ModelState.Clear();
            if(_complexBinder != null)
                await _complexBinder.BindModelAsync(bindingContext);
        }
    }
}
