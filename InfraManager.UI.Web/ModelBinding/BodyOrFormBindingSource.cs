using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InfraManager.UI.Web.ModelBinding
{
    public class BodyOrFormBindingSource : BindingSource
    {
        public static readonly BindingSource BodyOrDefault = new BodyOrFormBindingSource("BodyOrFrom", "BodyOrFrom", true, true);

        public BodyOrFormBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest) : base(id, displayName, isGreedy, isFromRequest)
        {
        }

        public override bool CanAcceptDataFrom(BindingSource bindingSource)
        {
            return bindingSource == Body || bindingSource == this;
        }
    }
}
