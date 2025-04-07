using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers
{
    public interface ITokenAuthenticationErrorFeature
    {
        string ErrorReason { get; }
    }

    public class TokenAuthenticationErrorFeature : ITokenAuthenticationErrorFeature
    {
        public TokenAuthenticationErrorFeature(string reason)
        {
            ErrorReason = reason;
        }
        public string ErrorReason { get; private set; }
    }
}
