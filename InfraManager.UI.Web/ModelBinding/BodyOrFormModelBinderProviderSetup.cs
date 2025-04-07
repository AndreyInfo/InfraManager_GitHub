using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections.Generic;
using System.Linq;
using ComplexDataModelBinderProvider = Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexObjectModelBinderProvider;

namespace InfraManager.UI.Web.ModelBinding
{
    public static class BodyOrFormModelBinderProviderSetup
    {
        public static void InsertBodyOrFormBinding(this IList<IModelBinderProvider> providers)
        {
            var bodyProvider = providers.Single(provider => provider.GetType() == typeof(BodyModelBinderProvider)) as BodyModelBinderProvider;
            var complexDataProvider = providers.OfType<ComplexDataModelBinderProvider>().Single();

            var bodyOrDefault = new BodyOrFormModelBinderProvider(bodyProvider, complexDataProvider);

            providers.Insert(0, bodyOrDefault);
        }
    }
}
