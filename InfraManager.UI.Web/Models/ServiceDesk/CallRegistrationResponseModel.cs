using InfraManager.Web.Controllers;
using System;

namespace InfraManager.UI.Web.Models.ServiceDesk
{
    public class CallRegistrationResponseModel
    {
        public RequestResponceType Type { get; init; }
        
        public string Message { get; init; }

        public Guid? CallID { get; init; }
    }
}
