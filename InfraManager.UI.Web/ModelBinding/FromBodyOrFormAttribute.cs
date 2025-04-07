using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace InfraManager.UI.Web.ModelBinding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromBodyOrFormAttribute : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource => BodyOrFormBindingSource.BodyOrDefault;
    }
}
